using Autofac;
using Autofac.Builder;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ioc Kernerl Builder that will create instances of the underlying IOC technology
    /// </summary>
    public class IocKernelBuilder : IIocKernelBuilder
    {
        #region Fields

        private readonly IAutoFacDependencyContainer _container;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Instanciate a new instance with build options
        /// </summary>
        /// <param name="buildOptions">AutoFac build options.</param>
        /// <param name="dependencyFactory">The dependency factory.</param>
        /// <param name="additionalBinders">Optional additional binders.</param>
        public IocKernelBuilder(ContainerBuildOptions buildOptions, IDependencyFactory dependencyFactory, params IDependencyScopeBinder<ContainerBuilder>[] additionalBinders)
        {
            List<IDependencyScopeBinder<ContainerBuilder>> binders = new(additionalBinders) { new AutoFacDependencyScopeBinder(dependencyFactory) };

            _container = new DependencyContainer(buildOptions, dependencyFactory, new DependencyScopeCompiler<ContainerBuilder>(binders));
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public void Build(IList<IDependencyInfo> dependencyInfos)
        {
            _container.Build(dependencyInfos);
        }

        /// <summary>
        /// Create a new instance of the dependency builder
        /// </summary>
        public IDependencyBuilder CreateDependencyBuilder()
        {
            return new DependencyBuilder();
        }

        /// <summary>
        /// Create a new instance of the IOC container that wraps the AutoFac container.
        /// </summary>
        public IIocContainer CreateIocContainer()
        {
            return new IocContainer(_container);
        }

        #endregion Methods
    }
}