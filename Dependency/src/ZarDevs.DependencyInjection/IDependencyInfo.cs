using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Base dependency information description.
    /// </summary>
    public interface IDependencyInfo
    {
        #region Properties

        /// <summary>
        /// Specify the key of the dependency, can be null.
        /// </summary>
        object Key { get; set; }

        /// <summary>
        /// Specify the resolved types for this binding.
        /// </summary>
        ISet<Type> ResolvedTypes { get; }

        /// <summary>
        /// Specify the scope that this dependency is active in.
        /// </summary>
        DependyBuilderScope Scope { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create the dependency context
        /// </summary>
        /// <param name="ioc">The ioc container</param>
        /// <returns></returns>
        IDependencyContext CreateContext(IIocContainer ioc);

        #endregion Methods
    }
}