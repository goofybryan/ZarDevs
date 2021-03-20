using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ioc Kernel Builder that will create instances of the underlying IOC technology
    /// </summary>
    public sealed class IocKernelBuilder : IIocKernelBuilder
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the IOC kernel builder.
        /// </summary>
        public IocKernelBuilder()
        {
            DependencyResolutionConfiguration configuration = new();
            InstanceResolution = new DependencyInstanceResolution(configuration);
            Activator = new RuntimeDependencyActivator(InspectConstructor.Instance, Create.Instance, new RuntimeResolutionPlanCreator(InspectConstructor.Instance, true));
            Container = new DependencyContainer(configuration, Activator, new DependencyFactory(InspectMethod.Instance));
        }

        #endregion Constructors

        #region Properties

        private IDependencyTypeActivator Activator { get; }
        private IDependencyContainer Container { get; }
        private IDependencyInstanceResolution InstanceResolution { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create the Ioc Container, <see cref="IIocContainer"/> that will be used by this IOC
        /// implementation to resolve the request using the underlying IOC methodology.
        /// </summary>
        /// <returns></returns>
        public IDependencyBuilder CreateDependencyBuilder()
        {
            var builder = new DependencyBuilder(Container);

            builder.Bind<IDependencyTypeActivator>().To(Activator);
            builder.Bind<IInspectConstructor>().To(InspectConstructor.Instance);
            builder.Bind<IInspectMethod>().To(InspectMethod.Instance);
            builder.Bind<ICreate>().To(Create.Instance);
            builder.Bind<IDependencyInstanceResolution>().To(InstanceResolution);

            return builder;
        }

        /// <summary>
        /// Create the dependency container that will be used to bind the dependencies and the
        /// transform it to the underlying IOC methodology.
        /// </summary>
        /// <returns></returns>
        public IIocContainer CreateIocContainer()
        {
            return new DependencyResolver(InstanceResolution);
        }

        #endregion Methods
    }
}