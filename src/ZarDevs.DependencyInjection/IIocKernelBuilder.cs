using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ioc Kernel Builder that will create instances of the underlying IOC technology
    /// </summary>
    public interface IIocKernelBuilder
    {
        #region Methods

        /// <summary>
        /// Create the Ioc Container, <see cref="IIocContainer"/> that will be used by this IOC implementation to resolve the request using the underlying IOC methodology.
        /// </summary>
        /// <returns></returns>
        IIocContainer CreateIocContainer();

        /// <summary>
        /// Create the dependency container that will be used to bind the dependencies and the transform it to the underlying IOC methodology.
        /// </summary>
        /// <returns></returns>
        IDependencyBuilder CreateDependencyBuilder();

        #endregion Methods
    }
}