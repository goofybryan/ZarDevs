using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ZarDevs.DependencyInjection
{    internal class DependencyContainer : IDependencyContainer
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

        public static IDependencyContainer Create(IServiceCollection services) => new DependencyContainer(services, new NamedServiceConfiguration());

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
                    _services.AddTransient(info.RequestType, provider => info.MethodTo(new DepencyBuilderInfoContext(provider.GetRequiredService<IIocContainer>(), info.RequestType), info.Key));
                    break;

                case DependyBuilderScope.Singleton:
                    _services.AddSingleton(info.RequestType, provider => info.MethodTo(new DepencyBuilderInfoContext(provider.GetRequiredService<IIocContainer>(), info.RequestType), info.Key));
                    break;

                case DependyBuilderScope.Request:
                    _services.AddScoped(info.RequestType, provider => info.MethodTo(new DepencyBuilderInfoContext(provider.GetRequiredService<IIocContainer>(), info.RequestType), info.Key));
                    break;
            }

            return true;
        }

        private bool TryRegisterTypeTo(IDependencyTypeInfo info)
        {
            if (info == null)
                return false;

            if (info.Key == null || info.Key.ToString() == string.Empty)
            {
                RegisterInstances(info.RequestType, info.ResolvedType, info.Scope);
                return true;
            }

            _namedConfiguration.Configure(info.RequestType, info.ResolvedType, info.Key);
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
