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
        /// Create the binding with the key value.
        /// </summary>
        IDependencyBuilderInfo WithKey(object key);

        #endregion Methods
    }
}