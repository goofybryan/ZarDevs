using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency instance configuration
    /// </summary>
    public interface IDependencyInstanceConfiguration
    {
        #region Methods

        /// <summary>
        /// Add an instance to the configuration for the Type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance that will always be resolved.</param>
        /// <param name="key">An optional key</param>
        void AddInstanceResolution<T>(T instance, object key);

        /// <summary>
        /// Configure the request type <paramref name="requestType"/> to the resolution <paramref name="info"/>
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="info">The resolution that will be implemented.</param>
        void Configure(Type requestType, IDependencyResolution info);

        #endregion Methods
    }
}