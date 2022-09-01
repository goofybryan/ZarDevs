using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class BindingContainerFactoryGenerator
{
    private readonly BindingGenerator _bindingGenerator;
    private readonly GeneratedContentPersistor _contentPersistor;

    public BindingContainerFactoryGenerator(GeneratedContentPersistor contentPersistor)
    {
        _contentPersistor = contentPersistor ?? throw new ArgumentNullException(nameof(contentPersistor));
        _bindingGenerator = new();
    }

    public IEnumerable<string> Generate(IDictionary<IResolveBinding, string> classMappings, MethodDeclarationSyntax methodDeclaration)
    {
        foreach (var classMapping in classMappings)
        {
            var className = classMapping.Value;
            var binding = classMapping.Key;

            _bindingGenerator.TryGenerate(binding, className);
        }

        ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)methodDeclaration.Parent!;
        string classNameHint = $"{classDeclaration.Identifier.Text}{methodDeclaration.Identifier.Text}";

        if(_bindingGenerator.TypeMappings.Length > 0)
        {
            string typeMapperContent = Code.TypeMapper(classNameHint, () => _bindingGenerator.TypeMappings.ToString());
            string typeMapperName = $"TypeMapper{classNameHint}";

            _contentPersistor.Persist(typeMapperName, typeMapperContent);

            yield return typeMapperName;
        }

        if (_bindingGenerator.FactoryMappings.Length > 0)
        {
            string factoryMapperContent = Code.FactoryMapper(classNameHint, () => _bindingGenerator.FactoryMappings.ToString());
            string factoryMapperName = $"FactoryMapper{classNameHint}";

            _contentPersistor.Persist(factoryMapperName, factoryMapperContent);

            yield return factoryMapperName;
        }
    }
}
