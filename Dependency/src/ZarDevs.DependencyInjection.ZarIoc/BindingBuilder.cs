using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal abstract class BindingBuilder : IResolveBinding
{
    public IList<TypeInfo> ResolveTypes { get; } = new List<TypeInfo>();
    public SyntaxToken KeyToken { get; set; }
    public DependyBuilderScopes Scope { get; set; } = DependyBuilderScopes.Transient;

    public IList<TypeInfo> IgnoreTypes { get; } = new List<TypeInfo>();

    public bool ResolveAll { get; set; }

    public bool IsValid()
    {
        return (ResolveAll || ResolveTypes.Count > 0) && OnIsValid();
    }

    public virtual bool OnIsValid() => true;
}
