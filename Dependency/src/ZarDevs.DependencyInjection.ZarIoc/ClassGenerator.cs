using System;
using System.Collections.Generic;

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
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
#endif

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
