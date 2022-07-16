using System.Collections.Generic;
using System.Linq;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ioc Kernel Builder that will create instances of the underlying IOC technology
    /// </summary>
    public sealed class IocKernelBuilder : IIocKernelBuilder
    {
        /// <summary>
        /// Supported scopes
        /// </summary>
        public const DependyBuilderScopes SupportedScopes = DependyBuilderScopes.Singleton | DependyBuilderScopes.Transient | DependyBuilderScopes.Thread;

        #region Constructors

        /// <summary>
        /// Create a new instance of the IOC kernel builder.
        /// </summary>
        public IocKernelBuilder(params IDependencyScopeBinder<IDependencyResolutionConfiguration>[] additionalBinders)
        {
            DependencyResolutionConfiguration configuration = new();
            InstanceResolution = new DependencyInstanceResolution(configuration);
            Activator = new RuntimeDependencyActivator(InspectConstructor.Instance, Create.Instance, new RuntimeResolutionPlanCreator(InspectConstructor.Instance, true));
            DependencyResolutionFactory resolutionFactory = new(Activator, new DependencyFactory(InspectMethod.Instance));
            DependencyScopeCompiler<IDependencyResolutionConfiguration> compiler = new(CreateBinders(resolutionFactory).Union(additionalBinders));
            Container = new DependencyContainer(configuration, compiler);
        }

        #endregion Constructors

        #region Properties

        private IDependencyTypeActivator Activator { get; }
        private IDependencyContainer Container { get; }
        private IDependencyInstanceResolution InstanceResolution { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create the Ioc Bindings, <see cref="IIocContainer"/> that will be used by this IOC
        /// implementation to resolve the request using the underlying IOC methodology.
        /// </summary>
        /// <returns></returns>
        public IDependencyBuilder CreateDependencyBuilder()
        {
            var builder = new DependencyBuilder(Container);

            builder.BindInstance(Activator).Resolve<IDependencyTypeActivator>();
            builder.BindInstance(InspectConstructor.Instance).Resolve<IInspectConstructor>();
            builder.BindInstance(InspectMethod.Instance).Resolve<IInspectMethod>();
            builder.BindInstance(Create.Instance).Resolve<ICreate>();
            builder.BindInstance(InstanceResolution).Resolve<IDependencyInstanceResolution>();

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

        private static IEnumerable<IDependencyScopeBinder<IDependencyResolutionConfiguration>> CreateBinders(DependencyResolutionFactory resolutionFactory)
        {
            yield return new TypedDependencyScopeCompiler(resolutionFactory, SupportedScopes);
            yield return new InstanceDependencyScopeCompiler(resolutionFactory);
            yield return new FactoryDependencyScopeCompiler(resolutionFactory, SupportedScopes);
            yield return new MethodDependencyScopeCompiler(resolutionFactory, SupportedScopes);
        }

        #endregion Methods
    }
}