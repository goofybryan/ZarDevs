using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ZarDevs.DependencyInjection.ZarIoc;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Main generator class
/// </summary>
[Generator]
public class DependencyGenerator : IIncrementalGenerator
{
    #region Fields

    private static readonly string _attributeName = typeof(DependencyRegistrationAttribute).FullName;

    #endregion Fields

    #region Methods

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarationsProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsDependencyRegistration(s),
                transform: static (ctx, c) => GetSemanticTargetForGeneration(ctx, c)
                );

        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarationsProvider.Collect());

        context.RegisterSourceOutput(compilationAndClasses, static (spc, source) => Execute(source.Left, spc, source.Right));
    }

    private static void Execute(Compilation compilation, SourceProductionContext context, ImmutableArray<IEnumerable<MethodDeclarationSyntax>> classDeclarationsMap)
    {
        if (classDeclarationsMap.IsDefaultOrEmpty) return;

        string @namespace = $"{compilation.AssemblyName}.Ioc";
        GeneratedContentPersistor contentPersistor = new(context, @namespace);
        
        INamedTypeSymbol enumerable = compilation.GetTypeByMetadataName(typeof(IEnumerable).FullName)!;

        var typeCodeGenerators = new ITypeCodeGenerator[] { new TypeCodeGenerator(contentPersistor, enumerable, context.CancellationToken), new InstanceCodeGenerator(), new FunctionCodeGenerator(), new FactoryCodeGenerator(contentPersistor, enumerable, context.CancellationToken) };
        ClassGenerator classGenerator = new(new DiagnosticLogger(context), typeCodeGenerators);
        BindingContainerFactoryGenerator factoryGenerator = new(contentPersistor);

        List<string> resolutionMappers = new();

        foreach (var mapping in classDeclarationsMap.SelectMany(cd => cd))
        {
            var parser = new BindParser(compilation, compilation.GetSemanticModel(mapping.SyntaxTree), context.CancellationToken);
            var containerParameter = mapping.ParameterList.Parameters.SingleOrDefault();
            var containerToken = containerParameter.GetLastToken();

            var bindings = parser.ParseSyntax(mapping, containerToken).ToArray();

            var generatedClasses = classGenerator.Generate(bindings);

            var generatedMappers = factoryGenerator.Generate(generatedClasses, mapping);
            resolutionMappers.AddRange(generatedMappers);
        }

        new TypeFactoryExtensionBuilder(contentPersistor).Create(@namespace, resolutionMappers);
    }

    private static IEnumerable<MethodDeclarationSyntax> GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellation)
    {
        var declarationSyntax = (MethodDeclarationSyntax)context.Node;

        var attributes = declarationSyntax.AttributeLists.SelectMany(al => al.Attributes);

        if (declarationSyntax.ParameterList != null && declarationSyntax.ParameterList.Parameters.Count > 1) yield break;

        foreach (AttributeSyntax attributeSyntax in attributes)
        {
            var attributeSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol!;

            INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
            string fullName = attributeContainingTypeSymbol.ToDisplayString();

            if (fullName == _attributeName && declarationSyntax.Parent is ClassDeclarationSyntax)
            {
                yield return declarationSyntax;
            }

            cancellation.ThrowIfCancellationRequested();
        }
    }

    private static bool IsDependencyRegistration(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not MethodDeclarationSyntax declarationSyntax || declarationSyntax.AttributeLists.Count == 0) return false;

        return declarationSyntax.AttributeLists.ContainsAttributeName<DependencyRegistrationAttribute>();
    }

    #endregion Methods
}
