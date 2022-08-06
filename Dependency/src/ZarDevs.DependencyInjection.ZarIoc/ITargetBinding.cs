using Microsoft.CodeAnalysis;

namespace ZarDevs.DependencyInjection.SourceGenerator;

public interface ITargetBinding
{
    Microsoft.CodeAnalysis.TypeInfo TargetType { get; }
}
