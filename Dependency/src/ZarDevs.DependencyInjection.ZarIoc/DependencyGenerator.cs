using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ZarDevs.DependencyInjection.SourceGenerator;


/// <summary>
/// Main generator class
/// </summary>
[Generator]
public class DependencyGenerator : ISourceGenerator
{
    #region Methods

    /// <inheritdoc/>
    public void Execute(GeneratorExecutionContext context)
    {
        var logger = new DiagnosticLogger(context);

        var bindingFiles = context.AdditionalFiles
            .Where(at => at.Path.EndsWith(".zargen.xml", StringComparison.OrdinalIgnoreCase))
            .Select(at => at.Path)
            .ToArray();

        var trees = context.Compilation.SyntaxTrees.Where(x => x.HasCompilationUnitRoot).ToArray();

        if (bindingFiles.Length == 0)
            return;

        logger.InformationBindingCount(bindingFiles.Length);

        GenerationReferences references = new(context.Compilation);

        GenerationLoader loader = new(new(references, logger, context.CancellationToken), logger, context.CancellationToken, bindingFiles);

        references.Filter(loader.Container.Assemblies);

        AssemblyLoader assemblyLoader = new(references, logger, context.CancellationToken);

        DependencyBuilder builder = new DependencyBuilder();

        foreach (var bindingFile in loader.Container)
        {
            foreach (var configure in assemblyLoader.Get(bindingFile))
            {
                configure(builder);
            }
        }
    }

    /// <inheritdoc/>
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    #endregion Methods
}

/// <summary>
/// Assembly loader used to create instances of the <see cref="AssemblyBindingInfo"/> definitions.
/// </summary>
public class AssemblyLoader
{
    private readonly GenerationReferences _references;
    private readonly IDiagnosticLogger _logger;
    private readonly CancellationToken _cancellation;
    private readonly Dictionary<ISymbol, Assembly> _assemblySymbolMap;

    /// <summary>
    /// Create a new instance of the <see cref="AssemblyLoader"/>
    /// </summary>
    /// <param name="references">The gerneration references</param>
    /// <param name="logger">The logger</param>
    /// <param name="cancellation">cancellation token</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AssemblyLoader(GenerationReferences references, IDiagnosticLogger logger, CancellationToken cancellation)
    {
        _references = references ?? throw new ArgumentNullException(nameof(references));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cancellation = cancellation;
        _assemblySymbolMap = new(SymbolEqualityComparer.Default);

        Load();
    }

    /// <summary>
    /// Get a list of builder actions
    /// </summary>
    /// <param name="info">The assembly to generate from.</param>
    /// <returns>A list of actions than can be done</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IEnumerable<Action<IDependencyBuilder>> Get(AssemblyBindingInfo info)
    {
        var key = _assemblySymbolMap.Keys.Where(k => k.Name == info.Assembly).SingleOrDefault();

        if(key == null)
        {
            throw new InvalidOperationException($"The assembly '{info.Assembly}' was not found.");
        }

        _logger.CreatingInstances(info);

        Assembly assembly = _assemblySymbolMap[key];
        if (info.IsAssemblyScan()) return ScanAssembly(assembly);

        var className = $"{info.Assembly}.{info.Class}";
        if (info.IsMethodBinding()) return  new[] { CreateIDependencyBuilderFrom(assembly, className, info.Method) };
        return new[] { CreateIDependencyBuilderFrom(assembly, className) };
    }

    private IEnumerable<Action<IDependencyBuilder>> ScanAssembly(Assembly assembly)
    {
        Type dependencyRegistrationType = typeof(IDependencyRegistration);
        var dependencyRegistrationTypes = assembly.GetTypes().Where(type => dependencyRegistrationType.IsAssignableFrom(dependencyRegistrationType));

        foreach(var type in dependencyRegistrationTypes)
        {
            var instance = (IDependencyRegistration) Runtime.Create.Instance.New(type);
            yield return instance.Register;
            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private Action<IDependencyBuilder> CreateIDependencyBuilderFrom(Assembly assembly, string className, string methodName)
    {
        Type type = assembly.GetType(className);

        if (type == null) throw new InvalidOperationException($"The class '{className}' does not exist in the assembly '{assembly.FullName}'");

        var methods = Runtime.InspectMethod.Instance.GetMethods(type, methodName).OfType<MethodInfo>().ToArray();

        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (parameters.Length > 1) continue;

            if (parameters[0].ParameterType == typeof(IDependencyBuilder))
            {
                var instance = (IDependencyRegistration)Runtime.Create.Instance.New(type);
                return builder => method.Invoke(instance, new object[] { builder });
            }
        }

        throw new InvalidOperationException($"The method '{methodName}' with signature Method(IDependencyBuilder) does not exist in the class '{className}'");
    }

    private Action<IDependencyBuilder> CreateIDependencyBuilderFrom(Assembly assembly, string className)
    {
        Type type = assembly.GetType(className);

        if(type == null) throw new InvalidOperationException($"The class '{className}' does not exist in the assembly '{assembly.FullName}'");

        var instance = (IDependencyRegistration)Runtime.Create.Instance.New(type);
        return instance.Register;
    }

    private void Load()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        foreach (var reference in _references)
        {
            string path = reference.reference.Display;

            try
            {
                Assembly assembly = Assembly.LoadFrom(path);
                _assemblySymbolMap.Add(reference.symbol, assembly);
                // TODO Log
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
            }

            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        return _references.ReferenceLocations.TryGetValue(args.Name, out string assemble) ? Assembly.LoadFrom(assemble) : null;
    }
}
