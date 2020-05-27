using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyInfo
    {
        #region Properties

        string Name { get; }

        DependyBuilderScope Scope { get; }

        Type TypeFrom { get; }

        #endregion Properties
    }
}