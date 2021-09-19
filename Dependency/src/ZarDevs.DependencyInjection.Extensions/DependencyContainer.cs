using System;
using System.Globalization;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency container that is used to build bindings and apply them to the configuration.
    /// </summary>
    public class DependencyContainer : DependencyContainerBase
    {
        #region Fields

        private readonly IDependencyTypeActivator _activator;
        private readonly IDependencyResolutionConfiguration _configuration;
        private readonly IDependencyFactory _dependencyFactory;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the dependency container.
        /// </summary>
        /// <param name="configuration">
        /// The instance configuration that will contain the binding configuration.
        /// </param>
        /// <param name="activator">The type activator that is used to resolve types.</param>
        /// <param name="dependencyFactory">The dependency factory.</param>
        public DependencyContainer(IDependencyResolutionConfiguration configuration, IDependencyTypeActivator activator, IDependencyFactory dependencyFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Implement the on build method that will be called for each definition added.
        /// </summary>
        /// <param name="definition">The dependency info that describes what is required.</param>
        protected override void OnBuild(IDependencyInfo definition)
        {
            if (!TryRegisterTypeTo(definition as IDependencyTypeInfo) && !TryRegisterMethod(definition as IDependencyMethodInfo) && !TryRegisterInstance(definition as IDependencyInstanceInfo) && !TryRegisterFactory(definition as IDependencyFactoryInfo))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", definition.ResolvedTypes.ToArray()));
        }

        /// <summary>
        /// Register a <see cref="IDependencyInstanceInfo"/> instance with the configuration. Can be overridden.
        /// </summary>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        /// <param name="factory"></param>
        protected virtual void OnRegisterFactory(IDependencyFactoryInfo info, IDependencyFactory factory)
        {
            _configuration.Add(info.ResolvedTypes, new DependencyFactoryResolution(info, factory));
        }

        /// <summary>
        /// Register a singleton <see cref="IDependencyTypeInfo"/> instance with the configuration.
        /// Can be overridden.
        /// </summary>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        /// <param name="factory">The dependency factory that will resolve the factory.</param>
        protected virtual void OnRegisterFactorySingleton(IDependencyFactoryInfo info, IDependencyFactory factory)
        {
            _configuration.Add(info.ResolvedTypes, new DependencySingletionResolution<IDependencyFactoryInfo, DependencyFactoryResolution>(new DependencyFactoryResolution(info, factory)));
        }

        /// <summary>
        /// Register a <see cref="IDependencyInstanceInfo"/> instance with the configuration. Can be overridden.
        /// </summary>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterInstance(IDependencyInstanceInfo info)
        {
            foreach (var resolvedType in info.ResolvedTypes)
            {
                _configuration.Add(resolvedType, new DependencySingletonInstance(info));
            }
        }

        /// <summary>
        /// Register a singleton <see cref="IDependencyTypeInfo"/> instance with the configuration.
        /// Can be overridden.
        /// </summary>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterSingleton(IDependencyTypeInfo info)
        {
            _configuration.Add(info.ResolvedTypes, new DependencySingletionResolution<IDependencyTypeInfo, DependencyTypeResolution>(new DependencyTypeResolution(info, _activator)));
        }

        /// <summary>
        /// Register a singleton <see cref="IDependencyMethodInfo"/> instance with the
        /// configuration. Can be overridden.
        /// </summary>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterSingletonMethod(IDependencyMethodInfo info)
        {
            _configuration.Add(info.ResolvedTypes, new DependencySingletionResolution<IDependencyMethodInfo, DependencyMethodResolution>(new DependencyMethodResolution(info)));
        }

        /// <summary>
        /// Register a transient <see cref="IDependencyTypeInfo"/> instance with the configuration.
        /// Can be overridden.
        /// </summary>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterTransient(IDependencyTypeInfo info)
        {
            _configuration.Add(info.ResolvedTypes, new DependencyTypeResolution(info, _activator));
        }

        /// <summary>
        /// Register a transient <see cref="IDependencyMethodInfo"/> instance with the
        /// configuration. Can be overridden.
        /// </summary>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterTransientMethod(IDependencyMethodInfo info)
        {
            _configuration.Add(info.ResolvedTypes, new DependencyMethodResolution(info));
        }

        private bool TryRegisterFactory(IDependencyFactoryInfo info)
        {
            if (info == null)
                return false;

            switch (info.Scope)
            {
                case DependyBuilderScope.Transient:
                    OnRegisterFactory(info, _dependencyFactory);
                    break;

                case DependyBuilderScope.Singleton:
                    OnRegisterFactorySingleton(info, _dependencyFactory);
                    break;

                default:
                    throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
            }

            return true;
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