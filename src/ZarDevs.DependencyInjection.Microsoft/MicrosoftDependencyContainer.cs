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

        public MicrosoftDependencyContainer(IServiceCollection services, IDependencyInstanceConfiguration configuration, IDependencyTypeActivator activator)
            : base(configuration, activator)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Constructors

        #region Methods

        protected override void OnRegisterInstance(IDependencyInstanceInfo info)
        {
            _services.AddSingleton(info.RequestType, info.Instance);

            base.OnRegisterInstance(info);
        }

        protected override void OnRegisterSingleton(IDependencyTypeInfo info)
        {
            _services.AddSingleton(info.ResolvedType);
            _services.AddSingleton(info.RequestType, info.ResolvedType);

            base.OnRegisterSingleton(info);
        }

        protected override void OnRegisterTransient(IDependencyTypeInfo info)
        {
            _services.AddTransient(info.ResolvedType);
            _services.AddTransient(info.RequestType, info.ResolvedType);

            base.OnRegisterTransient(info);
        }

        protected override void OnRegisterSingletonMethod(IDependencyMethodInfo info)
        {
            _services.AddSingleton(info.RequestType, p => info.Method(CreateDependencyInfoContext(p, info), info.Key));

            base.OnRegisterSingletonMethod(info);
        }

        protected override void OnRegisterTransientMethod(IDependencyMethodInfo info)
        {
            _services.AddTransient(info.RequestType, p => info.Method(CreateDependencyInfoContext(p, info), info.Key));

            base.OnRegisterTransientMethod(info);
        }

        private DepencyBuilderInfoContext CreateDependencyInfoContext(IServiceProvider provider, IDependencyMethodInfo info)
        {
            if(info.RequestType == typeof(IIocContainer)) 
                return null;

            return new DepencyBuilderInfoContext(provider.GetRequiredService<IIocContainer>());
        }

        #endregion Methods
    }
}