using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Generator types containing the list of types this uses. This can also be used as a look up for types.
/// </summary>
public interface IGeneratorTypes
{
    #region Properties

    /// <summary>
    /// The <see cref="IDependencyBuilder"/><see cref="INamedTypeSymbol"/>
    /// </summary>
    INamedTypeSymbol DependecyBuilderType { get; }

    /// <summary>
    /// The <see cref="IDependencyRegistration"/><see cref="INamedTypeSymbol"/>
    /// </summary>
    INamedTypeSymbol DependencyRegistrationType { get; }

    #endregion Properties

    /// <summary>
    /// Find the <see cref="INamedTypeSymbol"/> for <paramref name="fullName"/>. This must be the
    /// full name e.g. <c>ZarDevs.DependencyInjection.IDependencyRegistration</c>
    /// </summary>
    /// <param name="fullName"></param>
    /// <returns></returns>

    #region Methods

    INamedTypeSymbol FindTypeSymbol(string fullName);

    /// <summary>
    /// Check if the <paramref name="typeSymbol"/> is a valid <see cref="DependecyBuilderType"/> symbol.
    /// </summary>
    /// <param name="typeSymbol">The symbol to check.</param>
    /// <returns>True if it is valid.</returns>
    bool IsValidBuilderType(ITypeSymbol typeSymbol);

    /// <summary>
    /// Check if the <paramref name="typeSymbol"/> is a valid <see
    /// cref="DependencyRegistrationType"/> symbol.
    /// </summary>
    /// <param name="typeSymbol">The symbol to check.</param>
    /// <returns>True if it is valid.</returns>
    bool IsValidDependencyType(ITypeSymbol typeSymbol);

    #endregion Methods
}

/// <inheritdoc/>
public class GeneratorTypes : IGeneratorTypes
{
    #region Fields

    private readonly IEqualityComparer<ISymbol> _comparer;
    private readonly Compilation _compilation;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create new instance of <see cref="GeneratorTypes"/>
    /// </summary>
    /// <param name="compilation">The compilation this covers.</param>
    /// <param name="comparer">Optional comparer, default is <see cref="SymbolEqualityComparer.Default"/></param>
    public GeneratorTypes(Compilation compilation, IEqualityComparer<ISymbol> comparer)
    {
        _compilation = compilation ?? throw new ArgumentNullException(nameof(compilation));
        _comparer = comparer ?? SymbolEqualityComparer.Default;
        DependecyBuilderType = _compilation.GetTypeByMetadataName(typeof(IDependencyBuilder).FullName) ?? throw new InvalidOperationException($"Cannot find '{typeof(IDependencyBuilder).FullName}' symbols.");
        DependencyRegistrationType = _compilation.GetTypeByMetadataName(typeof(IDependencyRegistration).FullName) ?? throw new InvalidOperationException($"Cannot find '{typeof(IDependencyRegistration).FullName}' symbols.");
    }

    #endregion Constructors

    #region Properties

    /// <inheritdoc/>
    public INamedTypeSymbol DependecyBuilderType { get; }

    /// <inheritdoc/>
    public INamedTypeSymbol DependencyRegistrationType { get; }

    #endregion Properties

    #region Methods

    /// <inheritdoc/>
    public INamedTypeSymbol FindTypeSymbol(string fullName) => _compilation.GetTypeByMetadataName(fullName)!;

    /// <inheritdoc/>
    public bool IsValidBuilderType(ITypeSymbol typeSymbol) => _comparer.Equals(typeSymbol, DependecyBuilderType);

    /// <inheritdoc/>
    public bool IsValidDependencyType(ITypeSymbol typeSymbol) => _comparer.Equals(typeSymbol, DependencyRegistrationType);

    #endregion Methods
}