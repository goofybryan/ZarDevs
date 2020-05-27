using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;

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

        public static void ConfigureIocProvider(this IServiceProvider serviceProvider)
        {
            IIocKernelServiceProvider iocKernelService = (IIocKernelServiceProvider)Ioc.Container;

            iocKernelService.ConfigureServiceProvider(serviceProvider);
        }

        #endregion Methods
    }

    internal class DependencyContainer : IDependencyContainer
    {
        #region Fields

        // TODO BM: Continue Microsoft Support
        private readonly INamedServiceConfiguration _namedConfiguration;

        private readonly IServiceCollection _services;

        #endregion Fields

        #region Constructors

        public DependencyContainer(IServiceCollection services, INamedServiceConfiguration namedConfiguration)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _namedConfiguration = namedConfiguration ?? throw new ArgumentNullException(nameof(namedConfiguration));
        }

        #endregion Constructors

        #region Methods

        public void Build(IList<IDependencyInfo> definitions)
        {
            foreach (var info in definitions)
            {
                if (!TryRegisterTypeTo(info as IDependencyTypeInfo) && !TryRegisterMethod(info as IDependencyMethodInfo))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", info.TypeFrom));
            }
        }

        // TODO BM: Continue Microsoft Support
        private static object FactoryMethod(IServiceProvider provider)
        {
            throw new NotImplementedException();
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

        private bool TryRegisterTypeTo(IDependencyTypeInfo info)
        {
            if (info == null)
                return false;

            return true;
        }

        #endregion Methods
    }
}