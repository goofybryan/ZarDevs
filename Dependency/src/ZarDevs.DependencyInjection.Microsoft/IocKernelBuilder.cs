using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly MicrosoftUtilitiesActivator _activator;
        private readonly IDependencyScopeBinder<IDependencyResolutionConfiguration>[] _additionalBinders;
        private readonly IDependencyInstanceResolution _instanceResolution;
        private readonly IDependencyResolutionConfiguration _resolutionConfiguration;
        private readonly IServiceCollection _serviceCollection;

        #endregion Fields

        #region Constructors

        public IocKernelBuilder(IServiceCollection serviceCollection, params IDependencyScopeBinder<IDependencyResolutionConfiguration>[] additionalBinders)
        {
            _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _additionalBinders = additionalBinders ?? throw new ArgumentNullException(nameof(additionalBinders));
            _resolutionConfiguration = new DependencyResolutionConfiguration();
            _instanceResolution = new DependencyInstanceResolution(_resolutionConfiguration);
            _activator = new(InspectConstructor.Instance);
        }

        #endregion Constructors

        #region Methods

        public void Build(IList<IDependencyInfo> dependencyInfos)
        {
            DependencyFactory dependencyFactory = new(InspectMethod.Instance);
            DependencyResolutionFactory resolutionFactory = new(_activator, new DependencyFactory(InspectMethod.Instance));
            DependencyScopeCompiler<IDependencyResolutionConfiguration> compiler = new(CreateBinders(_serviceCollection, resolutionFactory, dependencyFactory).Union(_additionalBinders));
            MicrosoftDependencyContainer dependencyContainer = new(_resolutionConfiguration, compiler);
            dependencyContainer.Build(dependencyInfos);
        }

        public void ConfigureServiceProvider(IServiceProvider serviceProvider)
        {
            _resolutionConfiguration.AddInstance(serviceProvider);
        }

        public IDependencyBuilder CreateDependencyBuilder()
        {
            DependencyBuilder builder = new();

            builder.BindInstance(InspectConstructor.Instance).Resolve<IInspectConstructor>();
            builder.BindInstance(InspectMethod.Instance).Resolve<IInspectMethod>();
            builder.BindInstance(Create.Instance).Resolve<ICreate>();
            builder.BindInstance(_resolutionConfiguration).Resolve<IDependencyResolutionConfiguration>();
            builder.BindInstance(_instanceResolution).Resolve<IDependencyInstanceResolution>();
            builder.BindInstance(_activator).Resolve<IDependencyTypeActivator>();
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

        private static IEnumerable<IDependencyScopeBinder<IDependencyResolutionConfiguration>> CreateBinders(IServiceCollection services, DependencyResolutionFactory resolutionFactory, DependencyFactory dependencyFactory)
        {
            yield return new MicrosoftTypedDependencyScopeCompiler(services, resolutionFactory);
            yield return new MicrosoftInstanceDependencyScopeCompiler(services, resolutionFactory);
            yield return new MicrosoftFactoryDependencyScopeCompiler(services, resolutionFactory, dependencyFactory);
            yield return new MicrosoftMethodDependencyScopeCompiler(services, resolutionFactory);
        }

        #endregion Methods
    }
}