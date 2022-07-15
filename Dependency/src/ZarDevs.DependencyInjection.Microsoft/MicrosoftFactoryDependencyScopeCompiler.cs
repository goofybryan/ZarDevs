using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency scope provider that is used to register <see cref="IServiceCollection"/> dependencies for <see cref="IDependencyFactoryInfo"/>.
    /// </summary>
    public class MicrosoftFactoryDependencyScopeCompiler : FactoryDependencyScopeCompiler
    {
        private readonly IServiceCollection _services;
        private readonly IDependencyFactory _dependencyFactory;

        /// <summary>
        /// Create a new instance of the <see cref="MicrosoftFactoryDependencyScopeCompiler"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="resolutionFactory">An instance of the resolution factory.</param>
        /// <param name="dependencyFactory">An instance of the dependency factory.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftFactoryDependencyScopeCompiler(IServiceCollection services, IDependencyResolutionFactory resolutionFactory, IDependencyFactory dependencyFactory)
            : base(resolutionFactory, DependyBuilderScopes.Transient | DependyBuilderScopes.Singleton | DependyBuilderScopes.Request)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        /// <inheritdoc/>
        protected override void OnRegisterRequest(IDependencyResolutionConfiguration container, IDependencyFactoryInfo info)
        {
            if (info.IsFactoryGeneric())
            {
            }
            else
            {
                foreach (var resolveType in info.ResolvedTypes)
                {
                    // BUG, This will create new instance for each resolve. Needs to be factory mapper if more than one. For now known issue.
                    _services.AddScoped(resolveType, p => _dependencyFactory.Resolve(info.CreateContext(Ioc.Container)));
                }
            }

            base.OnRegisterRequest(container, info);
        }

        /// <inheritdoc/>
        protected override void OnRegisterSingleton(IDependencyResolutionConfiguration container, IDependencyFactoryInfo info)
        {
            if (info.IsFactoryGeneric())
            {
            }
            else
            {
                foreach (var resolveType in info.ResolvedTypes)
                {
                    // BUG, This will create new instance for each resolve. Needs to be factory mapper if more than one. For now known issue.
                    _services.AddSingleton(resolveType, p => _dependencyFactory.Resolve(info.CreateContext(Ioc.Container)));
                }
            }

            base.OnRegisterSingleton(container, info);
        }

        /// <inheritdoc/>
        protected override void OnRegisterTransient(IDependencyResolutionConfiguration container, IDependencyFactoryInfo info)
        {

            if (info.IsFactoryGeneric())
            {
            }
            else
            {
                foreach (var resolveType in info.ResolvedTypes)
                {
                    _services.AddTransient(resolveType, p => _dependencyFactory.Resolve(info.CreateContext(Ioc.Container)));
                }
            }

            base.OnRegisterTransient(container, info);
        }
    }
}