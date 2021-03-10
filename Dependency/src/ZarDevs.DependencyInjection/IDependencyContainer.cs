using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency container that is used to house a list of dependencies. This interface is used by
    /// the implemented IOC technology to transform the binding information.
    /// </summary>
    public interface IDependencyContainer : IDisposable
    {
        #region Methods

        /// <summary>
        /// Build the dependencies.
        /// </summary>
        /// <param name="definitions">The list of definitions to transform.</param>
        void Build(IList<IDependencyInfo> definitions);

        /// <summary>
        /// Retrieve a dependency binding information when required.
        /// </summary>
        /// <param name="requestType">The request type to retrieve.</param>
        /// <param name="key">A key that the binding is associated with, can be null.</param>
        /// <returns></returns>
        IDependencyInfo TryGetBinding(Type requestType, object key);

        #endregion Methods
    }
}