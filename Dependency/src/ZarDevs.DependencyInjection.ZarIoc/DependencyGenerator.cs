using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

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
        var generatorTypes = new GeneratorTypes(context.Compilation);

        DiagnosticLogger logger = new(context);

        var bindingFiles = context.AdditionalFiles
            .Where(at => at.Path.EndsWith(".zargen.xml", StringComparison.OrdinalIgnoreCase))
            .Select(at => at.Path)
            .ToArray();

        GenerationLoader loader = new(logger, context.CancellationToken, bindingFiles);        
        GenerationContainer container = new(loader);
        GenerationTypeLoader typeLoader = new(logger, context.CancellationToken, container, generatorTypes);

        var syntaxReceiver = (DependencyRegistrationSyntaxReceiver)context.SyntaxReceiver;

        foreach(var method in typeLoader.MethodRegistrations)
        {
        }
    }

    /// <inheritdoc/>
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new DependencyRegistrationSyntaxReceiver());
    }

    #endregion Methods
}

public class DependencyRegistrationSyntaxReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> AllClassSyntax { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax declarationSyntax || declarationSyntax.AttributeLists.Count == 0) return;
        
        foreach(var a in declarationSyntax.AttributeLists)
        {
            Debug.WriteLine(a);
        }

        AllClassSyntax.Add(declarationSyntax);
    }
}
