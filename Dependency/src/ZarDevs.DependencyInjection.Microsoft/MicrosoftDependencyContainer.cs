using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZarDevs.DependencyInjection
{
    internal class MicrosoftDependencyContainer : DependencyContainer
    {
        #region Fields

        private readonly IServiceCollection _services;

        #endregion Fields

        #region Constructors

        public MicrosoftDependencyContainer(IServiceCollection services, IDependencyResolutionConfiguration configuration, IDependencyTypeActivator activator, IDependencyFactory dependencyFactory)
            : base(configuration, activator, dependencyFactory)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Constructors

        #region Methods

        protected override void OnRegisterFactory(IDependencyFactoryInfo info, IDependencyFactory factory)
        {
            if (info.IsFactoryGeneric())
            {
            }
            else
            {
                foreach (var resolveType in info.ResolvedTypes)
                {
                    _services.AddTransient(resolveType, p => factory.Resolve(info.CreateContext(Ioc.Container)));
                }
            }

            base.OnRegisterFactory(info, factory);
        }

        protected override void OnRegisterFactorySingleton(IDependencyFactoryInfo info, IDependencyFactory factory)
        {
            if (info.IsFactoryGeneric())
            {
            }
            else
            {
                foreach (var resolveType in info.ResolvedTypes)
                {
                    _services.AddSingleton(resolveType, p => factory.Resolve(info.CreateContext(Ioc.Container)));
                }
            }

            base.OnRegisterFactorySingleton(info, factory);
        }

        protected override void OnRegisterInstance(IDependencyInstanceInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                _services.AddSingleton(resolveType, info.Instance);
            }

            base.OnRegisterInstance(info);
        }

        protected override void OnRegisterSingleton(IDependencyTypeInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                _services.AddSingleton(resolveType, info.ResolutionType);
            }

            base.OnRegisterSingleton(info);
        }

        protected override void OnRegisterSingletonMethod(IDependencyMethodInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                _services.AddSingleton(resolveType, p => info.Execute(info.CreateContext(Ioc.Container)));
            }

            base.OnRegisterSingletonMethod(info);
        }

        protected override void OnRegisterTransient(IDependencyTypeInfo info)
        {

            foreach (var resolveType in info.ResolvedTypes)
            {
                _services.AddTransient(resolveType, info.ResolutionType);
            }

            base.OnRegisterTransient(info);
        }

        protected override void OnRegisterTransientMethod(IDependencyMethodInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                _services.AddTransient(resolveType, p => info.Execute(info.CreateContext(Ioc.Container)));
            }

            base.OnRegisterTransientMethod(info);
        }

        #endregion Methods
    }
}