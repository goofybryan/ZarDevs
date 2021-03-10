using Autofac.Builder;

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
        public IocKernelBuilder(ContainerBuildOptions buildOptions, IDependencyFactory dependencyFactory)
        {
            _container = DependencyContainer.Create(buildOptions, dependencyFactory);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Create a new instance of the dependency builder
        /// </summary>
        public IDependencyBuilder CreateDependencyBuilder()
        {
            return new DependencyBuilder(_container);
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