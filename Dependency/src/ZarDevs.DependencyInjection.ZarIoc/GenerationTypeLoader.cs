using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZarDevs.DependencyInjection.SourceGenerator;

public interface IGenerationTypeLoader
{
    /// <summary>
    /// Get a list of <see cref="INamedTypeSymbol"/> that implement <see cref="IDependencyRegistration"/>
    /// </summary>
    IList<INamedTypeSymbol> DependencyRegistrations { get; }

    /// <summary>
    /// Get a list of <see cref="IMethodSymbol"/> that implement the signature method(<see cref="IDependencyBuilder"/>)
    /// </summary>
    IList<IMethodSymbol> MethodRegistrations { get; }
}

/// <inheritdoc/>
public class GenerationTypeLoader : IGenerationTypeLoader
{
    private readonly IDiagnosticLogger _logger;
    private readonly CancellationToken _cancellation;
    private readonly IEnumerable<GenerationNamespace> _generationNamespaces;
    private readonly IGeneratorTypes _generatorTypes;

    /// <summary>
    /// Create a new instance of the <see cref="GenerationTypeLoader"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="cancellation"></param>
    /// <param name="generationNamespaces"></param>
    /// <param name="generatorTypes"></param>
    public GenerationTypeLoader(IDiagnosticLogger logger, CancellationToken cancellation, IEnumerable<GenerationNamespace> generationNamespaces, IGeneratorTypes generatorTypes)
    {
        _logger = logger;
        _cancellation = cancellation;
        _generationNamespaces = generationNamespaces;
        _generatorTypes = generatorTypes;

        DependencyRegistrations = new List<INamedTypeSymbol>();
        MethodRegistrations = new List<IMethodSymbol>();

        Load();
    }

    /// <inheritdoc/>
    public IList<INamedTypeSymbol> DependencyRegistrations { get; }

    /// <inheritdoc/>
    public IList<IMethodSymbol> MethodRegistrations { get; }

    private void Load()
    {
        foreach (var ns in _generationNamespaces)
        {
            string @namespace = ns.Name;
            LoadNamespace(@namespace, ns);

            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private void LoadNamespace(string @namespace, IEnumerable<BindingInfo> bindings)
    {
        foreach (var binding in bindings)
        {
            if (string.IsNullOrWhiteSpace(binding.Class))
            {
                _logger.NamespaceClassNotSpecified(@namespace, binding.Class);
                continue;
            }

            string fullName = $"{@namespace}.{binding.Class}";
            INamedTypeSymbol namedType = _generatorTypes.FindTypeSymbol(fullName);

            if (namedType == null)
            {
                _logger.NamespaceClassNotFound(@namespace, binding.Class);
                continue;
            }

            if (binding.IsMethodBinding() && MethodIsValid(namedType, binding.Method, out var method))
            {
                MethodRegistrations.Add(method);
                continue;
            }

            if (TypeIsValid(namedType))
            {
                DependencyRegistrations.Add(namedType);
            }

            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private bool MethodIsValid(ITypeSymbol typeSymbol, string methodName, out IMethodSymbol method)
    {
        method = null;
        if (typeSymbol.TypeKind != TypeKind.Class) return false;

        var members = typeSymbol.GetMembers(methodName);

        foreach (var member in members)
        {
            if (member.Kind != SymbolKind.Method) continue;

            method = (IMethodSymbol)member;
            if (method.Parameters.Length != 1) continue;

            var parameter = method.Parameters[0];
            if (_generatorTypes.IsValidBuilderType(parameter.Type)) return true;
        }

        method = null;
        _logger.ClassMethodNotFound(typeSymbol, methodName);

        return false;
    }

    private bool TypeIsValid(ITypeSymbol typeSymbol)
    {
        if (typeSymbol.TypeKind != TypeKind.Class) return false;
        if (typeSymbol.AllInterfaces.Any(i => _generatorTypes.IsValidDependencyType(i))) return true;

        _logger.ClassWithNoDependencyRegistration(typeSymbol);

        return false;
    }
}
