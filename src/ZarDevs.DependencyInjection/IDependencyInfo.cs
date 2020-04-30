using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyInfo
    {
        string Name { get; }

        DependyBuilderScope Scope { get; }

        Type TypeFrom { get; }
    }
}