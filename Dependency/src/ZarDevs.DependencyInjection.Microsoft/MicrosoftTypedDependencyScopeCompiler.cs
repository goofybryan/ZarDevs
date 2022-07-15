using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency scope provider that is used to register <see cref="IServiceCollection"/> dependencies for <see cref="IDependencyTypeInfo"/>.
    /// </summary>
    public class MicrosoftTypedDependencyScopeCompiler : TypedDependencyScopeCompiler
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Create a new instance of the <see cref="MicrosoftTypedDependencyScopeCompiler"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="resolutionFactory">An instance of the resolution factory.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftTypedDependencyScopeCompiler(IServiceCollection services, IDependencyResolutionFactory resolutionFactory) 
            : base(resolutionFactory, DependyBuilderScopes.Transient | DependyBuilderScopes.Singleton | DependyBuilderScopes.Request)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc/>
        protected override void OnRegisterRequest(IDependencyResolutionConfiguration container, IDependencyTypeInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                // BUG, This will create new instance for each resolve. Needs to be factory mapper if more than one. For now known issue.
                _services.AddScoped(resolveType, info.ResolutionType);
            }

            base.OnRegisterRequest(container, info);
        }

        /// <inheritdoc/>
        protected override void OnRegisterSingleton(IDependencyResolutionConfiguration container, IDependencyTypeInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                // BUG, This will create new instance for each resolve. Needs to be factory mapper if more than one. For now known issue.
                _services.AddSingleton(resolveType, info.ResolutionType);
            }

            base.OnRegisterSingleton(container, info);
        }

        /// <inheritdoc/>
        protected override void OnRegisterTransient(IDependencyResolutionConfiguration container, IDependencyTypeInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                _services.AddTransient(resolveType, info.ResolutionType);
            }

            base.OnRegisterTransient(container, info);
        }
    }
}