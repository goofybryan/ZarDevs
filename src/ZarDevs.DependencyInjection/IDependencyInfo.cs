using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyInfo
    {
        #region Properties

        /// <summary>
        /// Specify the key of the dependency.
        /// </summary>
        object Key { get; }

        /// <summary>
        /// Specify the scope that this dependency is active in.
        /// </summary>
        DependyBuilderScope Scope { get; }

        /// <summary>
        /// Specify the request type that needs to be resolved.
        /// </summary>
        Type RequestType { get; }

        #endregion Properties
    }
}