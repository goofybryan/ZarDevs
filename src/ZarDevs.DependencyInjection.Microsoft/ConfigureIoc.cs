using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZarDevs.DependencyInjection
{
    public static class ConfigureIoc
    {
        #region Methods

        public static IServiceCollection ConfigureIocBindings(this IServiceCollection services, Action<IDependencyBuilder> builder)
        {
            var namedConfiguration = new NamedServiceConfiguration();
            var dependencyContainer = new DependencyContainer(services, namedConfiguration);
            var iocContainer = new IocKernelContainer(dependencyContainer);
            var dependencyBuilder = Ioc.InitializeWithBuilder(iocContainer);

            services.AddSingleton<INamedServiceConfiguration>(namedConfiguration);
            services.AddSingleton<INamedResolver, NamedResolver>();
            services.AddSingleton<IIocContainer>(iocContainer);

            builder(dependencyBuilder);

            dependencyBuilder.Build();

            return services;
        }

        public static IServiceProvider ConfigureIocProvider(this IServiceProvider serviceProvider)
        {
            IIocKernelServiceProvider iocKernelService = (IIocKernelServiceProvider)Ioc.GetIocKernel();

            iocKernelService.ConfigureServiceProvider(serviceProvider);

            return serviceProvider;
        }

        #endregion Methods
    }
}