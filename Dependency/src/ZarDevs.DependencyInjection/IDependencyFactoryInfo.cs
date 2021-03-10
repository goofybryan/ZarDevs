using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency factory information. When resolved the specified <see cref="MethodName"/> will be
    /// called from the <see cref="FactoryType"/> and returned.
    /// </summary>
    public interface IDependencyFactoryInfo : IDependencyInfo
    {
        #region Properties

        /// <summary>
        /// Get the factory type
        /// </summary>
        public Type FactoryType { get; }

        /// <summary>
        /// Get the method of the factory that will return the resolved type.
        /// </summary>
        public string MethodName { get; }

        #endregion Properties
    }
}