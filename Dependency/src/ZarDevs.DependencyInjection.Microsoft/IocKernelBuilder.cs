using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ioc
    /// </summary>
    public interface IIocKernelServiceProviderBuilder : IIocKernelBuilder
    {
        /// <summary>
        /// Configure the service provider by adding it to the instance resolution configurations.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>

        #region Methods

        void ConfigureServiceProvider(IServiceProvider serviceProvider);

        #endregion Methods
    }

    internal sealed class IocKernelBuilder : IIocKernelServiceProviderBuilder
    {
        #region Fields

        private readonly IDependencyInstanceResolution _instanceResolution;
        private readonly IDependencyResolutionConfiguration _resolutionConfiguration;
        private readonly IServiceCollection _serviceCollection;

        #endregion Fields

        #region Constructors

        public IocKernelBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _resolutionConfiguration = new DependencyResolutionConfiguration();
            _instanceResolution = new DependencyInstanceResolution(_resolutionConfiguration);
        }

        #endregion Constructors

        #region Methods

        public void ConfigureServiceProvider(IServiceProvider serviceProvider)
        {
            _resolutionConfiguration.AddInstance(serviceProvider);
        }

        public IDependencyBuilder CreateDependencyBuilder()
        {
            var activator = new MicrosoftUtilitiesActivator(InspectConstructor.Instance);
            var dependencyContainer = new MicrosoftDependencyContainer(_serviceCollection, _resolutionConfiguration, activator, new DependencyFactory(InspectMethod.Instance));
            var builder = new DependencyBuilder(dependencyContainer);

            builder.BindInstance(InspectConstructor.Instance).Resolve<IInspectConstructor>();
            builder.BindInstance(InspectMethod.Instance).Resolve<IInspectMethod>();
            builder.BindInstance(Create.Instance).Resolve<ICreate>();
            builder.BindInstance(_resolutionConfiguration).Resolve<IDependencyResolutionConfiguration>();
            builder.BindInstance(_instanceResolution).Resolve<IDependencyInstanceResolution>();
            builder.BindInstance(activator).Resolve<IDependencyTypeActivator>();
            builder.Bind<DependencyResolver>().Resolve<IDependencyResolver>();

            return builder;
        }

        public IIocContainer CreateIocContainer()
        {
            var serviceProvider = (IServiceProvider)_instanceResolution.GetResolution(typeof(IServiceProvider)).Resolve();
            var activator = serviceProvider.GetRequiredService<IDependencyResolver>();
            var configuration = serviceProvider.GetRequiredService<IDependencyResolutionConfiguration>();
            return configuration.HasGenericFactoryTypes ? new IocFactoryContainer(activator, serviceProvider) : new IocContainer(activator, serviceProvider);
        }

        #endregion Methods
    }
}