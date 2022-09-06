using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ioc Kernel Builder that will create instances of the underlying IOC technology
    /// </summary>
    public interface IIocKernelBuilder
    {
        #region Methods

        /// <summary>
        /// Build the Kernel from the list of <paramref name="dependencyInfos"/>
        /// </summary>
        /// <param name="dependencyInfos">A list of dependency information.</param>
        void Build(IList<IDependencyInfo> dependencyInfos);

        /// <summary>
        /// Create the dependency container that will be used to bind the dependencies and the
        /// transform it to the underlying IOC methodology.
        /// </summary>
        /// <returns></returns>
        IDependencyBuilder CreateDependencyBuilder();

        /// <summary>
        /// Create the Ioc Bindings, <see cref="IIocContainer"/> that will be used by this IOC
        /// implementation to resolve the request using the underlying IOC methodology.
        /// </summary>
        /// <returns></returns>
        IIocContainer CreateIocContainer();

        #endregion Methods
    }
}