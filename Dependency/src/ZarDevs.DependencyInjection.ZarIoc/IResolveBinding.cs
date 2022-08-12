using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal interface IResolveBinding
{
    #region Properties

    IList<TypeInfo> IgnoreTypes { get; }
    SyntaxToken KeyToken { get; set; }
    bool ResolveAll { get; set; }
    IList<TypeInfo> ResolveTypes { get; }
    DependyBuilderScopes Scope { get; set; }

    #endregion Properties

    #region Methods

    bool IsValid();

    #endregion Methods
}