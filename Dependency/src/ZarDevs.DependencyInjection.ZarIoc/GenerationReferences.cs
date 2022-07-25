using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Generation references
/// </summary>
public class GenerationReferences : IEnumerable<(ISymbol symbol, MetadataReference reference)>
{
    #region Fields

    private readonly Dictionary<ISymbol, MetadataReference> _assemblies;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new instance of the <see cref="GenerationReferences"/>
    /// </summary>
    /// <param name="compilation">Specify the compilation to generate from.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="compilation"/> is null.
    /// </exception>
    public GenerationReferences(Compilation compilation)
    {
        if (compilation is null)
        {
            throw new ArgumentNullException(nameof(compilation));
        }

        _assemblies = ParseAssemblies(compilation);
        ReferenceLocations = ParseReferences(compilation);
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Get a list all reference locations
    /// </summary>
    public IDictionary<string, string> ReferenceLocations { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Check where the <paramref name="assemblyName"/> is referenced
    /// </summary>
    /// <param name="assemblyName">The assembly name to check. Case sensitive</param>
    /// <returns>A true when found.</returns>
    public bool ContainsAssemblyName(string assemblyName)
    {
        return _assemblies.Keys.Any(a => a.Name == assemblyName);
    }

    /// <summary>
    /// Filter the reference list and keep only the assemblies listed in <paramref name="assembliesToKeep"/>
    /// </summary>
    /// <param name="assembliesToKeep"></param>
    public void Filter(IReadOnlyCollection<string> assembliesToKeep)
    {
        if (assembliesToKeep == null || assembliesToKeep.Count == 0) return;

        foreach (var assembly in _assemblies.Keys.ToArray())
        {
            if (assembliesToKeep.Contains(assembly.Name)) continue;

            _assemblies.Remove(assembly);
        }
        // TODO Log before and after
    }

    /// <inheritdoc/>
    public IEnumerator<(ISymbol symbol, MetadataReference reference)> GetEnumerator()
    {
        return _assemblies.Select(a => ValueTuple.Create(a.Key, a.Value)).GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static bool IsValidReference(string symbolName) => !symbolName.StartsWith("System", StringComparison.OrdinalIgnoreCase)
         && !symbolName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase)
         && !symbolName.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase)
         && !symbolName.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase)
         && !symbolName.StartsWith("WindowsBase", StringComparison.OrdinalIgnoreCase)
         && !symbolName.StartsWith("Accessibility", StringComparison.OrdinalIgnoreCase)
         && !symbolName.StartsWith("NuGet", StringComparison.OrdinalIgnoreCase)
         && !symbolName.StartsWith("testhost", StringComparison.OrdinalIgnoreCase);

    private static Dictionary<ISymbol, MetadataReference> ParseAssemblies(Compilation compilation)
    {
        var references = compilation.References
            .Select(r => new { reference = r, symbol = compilation.GetAssemblyOrModuleSymbol(r) as IAssemblySymbol })
            .Where(r => r.symbol != null && IsValidReference(r.symbol.Name));

        return references.ToDictionary(key => key.symbol, value => value.reference, SymbolEqualityComparer.Default);
    }

    private static Dictionary<string, string> ParseReferences(Compilation compilation)
    {
        var references = compilation.References
            .Select(r => new { reference = r, symbol = compilation.GetAssemblyOrModuleSymbol(r) as IAssemblySymbol })
            .Where(r => r.symbol != null && IsValidReference(r.symbol.Name));

        return references.ToDictionary(key => key.symbol.Name, value => value.reference.Display);
    }

    #endregion Methods
}