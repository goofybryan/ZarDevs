using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency builder information for a specific binding.
    /// </summary>
    public interface IDependencyBuilderInfo
    {
        #region Properties

        /// <summary>
        /// Get the dependency info
        /// </summary>
        IDependencyInfo DependencyInfo { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create the binding in Singleton Scopes resolution.
        /// </summary>
        IDependencyBuilderInfo InSingletonScope();

        /// <summary>
        /// Create the binding in Singleton Scopes resolution.
        /// </summary>
        IDependencyBuilderInfo InThreadScope();

        /// <summary>
        /// Create the binding in Singleton Scopes resolution.
        /// </summary>
        IDependencyBuilderInfo InRequestScope();

        /// <summary>
        /// Create the binding in Transient Scopes resolution. This is the default scope.
        /// </summary>
        IDependencyBuilderInfo InTransientScope();

        /// <summary>
        /// Create the binding with the key value.
        /// </summary>
        IDependencyBuilderInfo WithKey(object key);

        #endregion Methods
    }
}