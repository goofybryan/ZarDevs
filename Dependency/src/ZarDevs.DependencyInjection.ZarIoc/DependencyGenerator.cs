using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

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
        var bindingFile = context.AdditionalFiles.Where(at => at.Path.EndsWith(".zargen.xml", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        if(bindingFile == null)
            return;

        try
        {
            Bindings binding = Deserialize(bindingFile);
        }
        catch (Exception exc)
        {
            Debug.WriteLine(exc.ToString());
        }
    }

    private Bindings Deserialize(AdditionalText additionalText)
    {
        Debug.WriteLine(additionalText.GetText().ToString());
        using StreamReader reader = new(additionalText.Path);
        XmlSerializer serializer = new(typeof(Bindings));
        return (Bindings)serializer.Deserialize(reader);
    }

    /// <inheritdoc/>
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    #endregion Methods
}

public class Bindings
{
    [XmlElement("Binding", typeof(BindingInfo))]
    public List<BindingInfo> Info { get; set; }
}

public class BindingInfo
{
    [XmlAttribute]
    public string Assembly { get; set; }

    [XmlAttribute]
    public string Class { get; set; }

    [XmlAttribute]
    public string Method { get; set; }
}


public class FilterCompilation
{
    private readonly Compilation _compilation;

    public FilterCompilation(Compilation compilation)
    {
        _compilation = compilation ?? throw new ArgumentNullException(nameof(compilation));
    }

    public IEnumerable<ITypeSymbol> Filter<T>(CancellationToken cancellationToken)
    {
        var fullyQualifiedName = typeof(T).FullName;

        var type = _compilation.GetTypeByMetadataName(fullyQualifiedName) ?? throw new Exception($"Interface '{fullyQualifiedName}' not found in compilation");

        var classDeclarations = _compilation.SyntaxTrees
            .SelectMany(t => t.GetRoot(cancellationToken).DescendantNodes())
            .OfType<ClassDeclarationSyntax>();

        foreach (var declaration in classDeclarations)
        {
            
            if (TryGetImplementingSymbol(type, declaration, cancellationToken, out var classSymbol))
                yield return classSymbol;
        }
    }

    private bool TryGetImplementingSymbol(ITypeSymbol symbol, ClassDeclarationSyntax classDeclaration, CancellationToken cancellationToken, out ITypeSymbol typeSymbol)
    {
        typeSymbol = null;

        if (classDeclaration.BaseList == null) 
            return false;

        var semanticModel = _compilation.GetSemanticModel(classDeclaration.SyntaxTree);

        foreach (var baseType in classDeclaration.BaseList.Types)
        {
            var baseSymbol = _compilation.GetTypeByMetadataName(baseType.Type.ToFullString())!;
            if (baseSymbol == null) continue;

            var conversion = _compilation.ClassifyConversion(baseSymbol, symbol);

            if (conversion.Exists && conversion.IsImplicit)
            {
                typeSymbol = semanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken);
                return true;
            }
        }

        return false;
    }
}