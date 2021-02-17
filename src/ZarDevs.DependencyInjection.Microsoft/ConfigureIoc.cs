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
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", info.RequestType));
            }
        }

        private bool TryRegisterMethod(IDependencyMethodInfo info)
        {
            if (info == null)
                return false;

            switch (info.Scope)
            {
                case DependyBuilderScope.Transient:
                    _services.AddTransient(info.RequestType, provider => info.MethodTo(new DepencyBuilderInfoContext(provider.GetRequiredService<IIocContainer>(), info.RequestType), info.Name));
                    break;

                case DependyBuilderScope.Singleton:
                    _services.AddSingleton(info.RequestType, provider => info.MethodTo(new DepencyBuilderInfoContext(provider.GetRequiredService<IIocContainer>(), info.RequestType), info.Name));
                    break;

                case DependyBuilderScope.Request:
                    _services.AddScoped(info.RequestType, provider => info.MethodTo(new DepencyBuilderInfoContext(provider.GetRequiredService<IIocContainer>(), info.RequestType), info.Name));
                    break;
            }

            return true;
        }

        private bool TryRegisterTypeTo(IDependencyTypeInfo info)
        {
            if (info == null)
                return false;

            if (string.IsNullOrEmpty(info.Name))
            {
                RegisterInstances(info.RequestType, info.ResolvedType, info.Scope);
                return true;
            }

            _namedConfiguration.Configure(info.RequestType, info.ResolvedType, info.Name);
            RegisterInstances(info.ResolvedType, info.ResolvedType, info.Scope);

            return true;
        }

        private void RegisterInstances(Type requestType, Type resolvedType, DependyBuilderScope scope)
        {
            switch (scope)
            {
                case DependyBuilderScope.Transient:
                    _services.AddTransient(requestType, resolvedType);
                    break;
                case DependyBuilderScope.Singleton:
                    _services.AddSingleton(requestType, resolvedType);
                    break;
                case DependyBuilderScope.Request:
                    _services.AddScoped(requestType, resolvedType);
                    break;
            }
        }

        #endregion Methods
    }
}