using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class ClassGenerator
{
    #region Fields

    private readonly IList<ITypeCodeGenerator> _generators;
    private readonly IDiagnosticLogger _logger;

    #endregion Fields

    #region Constructors

    public ClassGenerator(IDiagnosticLogger logger, IList<ITypeCodeGenerator> generators)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _generators = generators ?? throw new ArgumentNullException(nameof(generators));
    }

    #endregion Constructors

    #region Methods

    public IDictionary<IResolveBinding, string>? Generate(IEnumerable<IResolveBinding> bindings)
    {
        if (bindings is null)
        {
            return null;
        }

        Dictionary<IResolveBinding, string> classDeclarations = new();

        foreach (var binding in bindings)
        {
            var generated = Generate(binding);

            if (generated!= null)
            {
                classDeclarations.Add(binding, generated);
            }
        }

        return classDeclarations;
    }

    private string? Generate(IResolveBinding binding)
    {
        if (!binding.IsValid())
        {
            // log error

            return null;
        }

        foreach (var generator in _generators)
        {
            if (generator.TryGenerate(binding, out var generated))
            {
                string className = generated;

                return className;
            }
        }

        // log error
        return null;
    }

    #endregion Methods
}

/// <summary>
/// This will persist the current content to a class
/// </summary>
public interface IContentPersistence
{
    /// <summary>
    /// Persist the <paramref name="content"/> to the <paramref name="className"/> to file to be compiled.
    /// </summary>
    /// <param name="className">The name of the class. This will become the filename</param>
    /// <param name="content">The content that will be persisted. This must not include any namespaces as this will be added.</param>
    /// <param name="usings">Add any usings required by the file</param>
    void Persist(string className, string content, params string[] usings);
}

internal class GeneratedContentPersistor : IContentPersistence
{
    private readonly SourceProductionContext _context;
    private readonly string _namespace;

    public GeneratedContentPersistor(SourceProductionContext context, string @namespace)
    {
        if (string.IsNullOrWhiteSpace(@namespace))
        {
            throw new System.ArgumentException($"'{nameof(@namespace)}' cannot be null or whitespace.", nameof(@namespace));
        }

        _context = context;
        _namespace = @namespace;
    }

    public void Persist(string className, string content, params string[] usings)
    {
        StringBuilder fileContentBuilder = new();

        AddUsings(usings, fileContentBuilder);

        fileContentBuilder.AppendLine(Code.Namespace(_namespace))
            .AppendLine(Code.OpenBrace)
            .AppendTab(content).AppendLine()
            .AppendLine(Code.CloseBrace);
        
        _context.AddSource($"{className}.g", fileContentBuilder.ToString());
    }

    private void AddUsings(string[] usings, StringBuilder fileContentBuilder)
    {
        if(usings.Length == 0) return;

        foreach (var @using in usings)
        {
            fileContentBuilder.AppendLine(@using);
        }

        fileContentBuilder.AppendLine();
    }
}