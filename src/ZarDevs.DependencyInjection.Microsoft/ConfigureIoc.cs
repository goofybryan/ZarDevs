using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZarDevs.DependencyInjection
{
    public static class ConfigureIoc
    {
        #region Methods

        public static IServiceCollection ConfigureIocBindings(this IServiceCollection services, Action<IDependencyBuilder> builder)
        {
            var resolutionConfiguration = new DependencyResolutionConfiguration();
            var dependencyContainer = new DependencyContainer(services, resolutionConfiguration);
            var iocContainer = new IocKernelContainer(dependencyContainer);
            var dependencyBuilder = Ioc.Instance.InitializeWithBuilder(iocContainer);

            services.AddSingleton<IDependencyInstanceResolution>(resolutionConfiguration);
            services.AddSingleton<IDependencyResolver, DependencyResolver>();
            services.AddSingleton<IIocContainer>(iocContainer);

            builder(dependencyBuilder);

            dependencyBuilder.Build();

            return services;
        }

        public static IServiceProvider ConfigureIocProvider(this IServiceProvider serviceProvider)
        {
            IIocKernelServiceProvider iocKernelService = (IIocKernelServiceProvider)Ioc.Container;

            iocKernelService.ConfigureServiceProvider(serviceProvider);

            return serviceProvider;
        }

        #endregion Methods
    }
}