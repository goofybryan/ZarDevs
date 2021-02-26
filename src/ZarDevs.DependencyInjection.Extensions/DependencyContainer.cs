using System;
using System.Globalization;

namespace ZarDevs.DependencyInjection
{
    public class DependencyContainer : DependencyContainerBase
    {
        #region Fields

        private readonly IDependencyTypeActivator _activator;
        private readonly IDependencyInstanceConfiguration _configuration;

        #endregion Fields

        #region Constructors

        public DependencyContainer(IDependencyInstanceConfiguration configuration, IDependencyTypeActivator activator)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        #endregion Constructors

        #region Methods

        protected override void OnBuild(IDependencyInfo definition)
        {
            if (!TryRegisterTypeTo(definition as IDependencyTypeInfo) && !TryRegisterMethod(definition as IDependencyMethodInfo) && !TryRegisterInstance(definition as IDependencyInstanceInfo))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", definition.RequestType));
        }

        protected virtual void OnRegisterInstance(IDependencyInstanceInfo info)
        {
            _configuration.Configure(info.RequestType, new DependencySingletonInstance(info));
        }

        protected virtual void OnRegisterSingleton(IDependencyTypeInfo info)
        {
            _configuration.Configure(info.RequestType, new DependencySingletionResolution<IDependencyTypeInfo, DependencyTypeResolution>(new DependencyTypeResolution(info, _activator)));
        }

        protected virtual void OnRegisterSingletonMethod(IDependencyMethodInfo info)
        {
            _configuration.Configure(info.RequestType, new DependencySingletionResolution<IDependencyMethodInfo, DependencyMethodResolution>(new DependencyMethodResolution(info)));
        }

        protected virtual void OnRegisterTransient(IDependencyTypeInfo info)
        {
            _configuration.Configure(info.RequestType, new DependencyTypeResolution(info, _activator));
        }

        protected virtual void OnRegisterTransientMethod(IDependencyMethodInfo info)
        {
            _configuration.Configure(info.RequestType, new DependencyMethodResolution(info));
        }

        private bool TryRegisterInstance(IDependencyInstanceInfo info)
        {
            if (info == null)
                return false;

            OnRegisterInstance(info);

            return true;
        }

        private bool TryRegisterMethod(IDependencyMethodInfo info)
        {
            if (info == null)
                return false;

            switch (info.Scope)
            {
                case DependyBuilderScope.Transient:
                    OnRegisterTransientMethod(info);
                    break;

                case DependyBuilderScope.Singleton:
                    OnRegisterSingletonMethod(info);
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
                case DependyBuilderScope.Transient:
                    OnRegisterTransient(info);
                    break;

                case DependyBuilderScope.Singleton:
                    OnRegisterSingleton(info);
                    break;

                default:
                    throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
            }

            return true;
        }

        #endregion Methods
    }
}