﻿using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public interface IIocKernelServiceProvider
    {
        #region Methods

        void ConfigureServiceProvider(IServiceProvider serviceProvider);

        #endregion Methods
    }

    public sealed class IocKernelBuilder : IIocKernelBuilder, IIocKernelServiceProvider
    {
        #region Fields

        private readonly IDependencyInstanceConfiguration _resolutionConfiguration;
        private readonly IServiceCollection _serviceCollection;

        #endregion Fields

        #region Constructors

        public IocKernelBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _resolutionConfiguration = new DependencyResolutionConfiguration();
        }

        #endregion Constructors

        #region Methods

        public void ConfigureServiceProvider(IServiceProvider serviceProvider)
        {
            _resolutionConfiguration.AddInstanceResolution(serviceProvider, null);
        }

        public IDependencyBuilder CreateDependencyBuilder()
        {
            var activator = new MicrosoftUtilitiesActivator(InspectConstructor.Instance);
            var dependencyContainer = new MicrosoftDependencyContainer(_serviceCollection, _resolutionConfiguration, activator);
            var builder = new DependencyBuilder(dependencyContainer);

            builder.Bind<IInspectConstructor>().To(InspectConstructor.Instance);
            builder.Bind<ICreate>().To(Create.Instance);
            builder.Bind<IDependencyInstanceResolution>().To(_resolutionConfiguration);
            builder.Bind<IDependencyTypeActivator>().To(activator);
            builder.Bind<IDependencyResolver>().To<DependencyResolver>();

            return builder;
        }

        public IIocContainer CreateIocContainer()
        {
            var resolution = (IDependencyInstanceResolution)_resolutionConfiguration;
            var serviceProvider = (IServiceProvider)resolution.GetResolution(typeof(IServiceProvider)).Resolve();
            var activator = serviceProvider.GetRequiredService<IDependencyResolver>();
            return new IocContainer(activator, serviceProvider);
        }

        #endregion Methods
    }
}