using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.SourceGenerator;

public interface IResolveBinding
{
    IList<Microsoft.CodeAnalysis.TypeInfo> ResolveTypes { get; }
    IList<Microsoft.CodeAnalysis.TypeInfo> IgnoreTypes { get; }
    SyntaxToken KeyToken { get; set; }
    DependyBuilderScopes Scope { get; set; }

    bool IsValid();
    bool ResolveAll { get; set; }
}
