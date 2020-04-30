using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ZarDevs.DependencyInjection
{
    public static class ConfigureIoc
    {
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

        public static void ConfigureIocProvider(this IServiceProvider serviceProvider)
        {
            IIocKernelServiceProvider iocKernelService = (IIocKernelServiceProvider)Ioc.Container;

            iocKernelService.ConfigureServiceProvider(serviceProvider);
        }
    }

    internal class DependencyContainer : IDependencyContainer
    {
        private readonly IServiceCollection _services;
        private readonly INamedServiceConfiguration _namedConfiguration;

        public DependencyContainer(IServiceCollection services, INamedServiceConfiguration namedConfiguration)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _namedConfiguration = namedConfiguration ?? throw new ArgumentNullException(nameof(namedConfiguration));
        }

        public void Build(IList<IDependencyInfo> definitions)
        {
            foreach(var info in definitions)
            {
                if (!TryRegisterTypeTo(info as IDependencyTypeInfo) && !TryRegisterMethod(info as IDependencyMethodInfo))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", info.TypeFrom));
            }
        }
        private bool TryRegisterMethod(IDependencyMethodInfo info)
        {
            if (info == null)
                return false;

            switch (info.Scope)
            {
                case DependyBuilderScope.Transient:
                    break;
                case DependyBuilderScope.Singleton:
                    break;
                case DependyBuilderScope.Request:
                    _services.AddScoped(info.TypeFrom, provider => info.MethodTo(new DepencyBuilderInfoContext(), info.Name));
                    break;
            }

            return true;
        }

        private static object FactoryMethod(IServiceProvider provider)
        {
            throw new NotImplementedException();
        }

        private bool TryRegisterTypeTo(IDependencyTypeInfo info)
        {
            if (info == null)
                return false;

            return true;
        }
    }
}
