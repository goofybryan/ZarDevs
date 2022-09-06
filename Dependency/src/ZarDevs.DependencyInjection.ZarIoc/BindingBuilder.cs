using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal abstract class BindingBuilder : IResolveBinding
{
    #region Properties

    public IList<TypeInfo> IgnoreTypes { get; } = new List<TypeInfo>();
    public SyntaxToken KeyToken { get; set; }
    public bool ResolveAll { get; set; }
    public IList<TypeInfo> ResolveTypes { get; } = new List<TypeInfo>();
    public DependyBuilderScopes Scope { get; set; } = DependyBuilderScopes.Transient;

    #endregion Properties

    #region Methods

    public bool IsValid()
    {
        return (ResolveAll || ResolveTypes.Count > 0) && OnIsValid();
    }

    protected virtual bool OnIsValid() => true;

    #endregion Methods
}