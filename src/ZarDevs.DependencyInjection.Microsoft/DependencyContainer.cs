using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyContainer : DependencyContainerBase
    {
        #region Fields

        // TODO BM: Continue Microsoft Support
        private readonly IDependencyInstanceConfiguration _configuration;

        private readonly IServiceCollection _services;

        #endregion Fields

        #region Constructors

        public DependencyContainer(IServiceCollection services, IDependencyInstanceConfiguration configuration)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion Constructors

        #region Methods

        public static IDependencyContainer Create(IServiceCollection services) => new DependencyContainer(services, new DependencyResolutionConfiguration());

        protected override void OnBuild(IDependencyInfo definition)
        {
            if (!TryRegisterTypeTo(definition as IDependencyTypeInfo) && !TryRegisterMethod(definition as IDependencyMethodInfo))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", definition.RequestType));
        }

        private bool TryRegisterMethod(IDependencyMethodInfo info)
        {
            if (info == null)
                return false;

            switch (info.Scope)
            {
                case DependyBuilderScope.Request:
                case DependyBuilderScope.Transient:
                    _configuration.Configure(info.RequestType, new DependencyMethodResolution(info));
                    break;

                case DependyBuilderScope.Singleton:
                    _configuration.Configure(info.RequestType, new DependencyMethodSingletonResolution(info));
                    break;

                default:
                    throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
            }

            return true;
        }

        private bool TryRegisterTypeTo(IDependencyTypeInfo info)
        {
            if (info == null)
                return false;

            switch (info.Scope)
            {
                case DependyBuilderScope.Request:
                case DependyBuilderScope.Transient:
                    _services.AddTransient(info.ResolvedType);
                    _services.AddTransient(info.RequestType, info.ResolvedType);
                    _configuration.Configure(info.RequestType, new DependencyTypeResolution(info));
                    break;

                case DependyBuilderScope.Singleton:
                    _services.AddSingleton(info.ResolvedType);
                    _services.AddSingleton(info.RequestType, info.ResolvedType);
                    _configuration.Configure(info.RequestType, new DependencyTypeSingletonResolution(info));
                    break;

                default:
                    throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
            }

            return true;
        }

        #endregion Methods
    }
}