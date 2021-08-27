using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency type information. When resolved the specified type will be instantiated and returned.
    /// </summary>
    public interface IDependencyTypeInfo : IDependencyInfo
    {
        #region Properties

        /// <summary>
        /// Get the resolved type that the IOC will resolved from the <see cref="IDependencyInfo.ResolvedTypes"/>.
        /// </summary>
        Type ResolutionType { get; }

        #endregion Properties
    }
}