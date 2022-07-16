using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency scope provider that is used to register <see cref="IServiceCollection"/> dependencies for <see cref="IDependencyMethodInfo"/>.
    /// </summary>
    public class MicrosoftMethodDependencyScopeCompiler : MethodDependencyScopeCompiler
    {
        private readonly IServiceCollection _services;
        private readonly IDependencyResolutionFactory _resolutionFactory;

        /// <summary>
        /// Create a new instance of the <see cref="MicrosoftTypedDependencyScopeCompiler"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="resolutionFactory">An instance of the resolution factory.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftMethodDependencyScopeCompiler(IServiceCollection services, IDependencyResolutionFactory resolutionFactory)
            : base(resolutionFactory, DependyBuilderScopes.Transient | DependyBuilderScopes.Singleton)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _resolutionFactory = resolutionFactory ?? throw new ArgumentNullException(nameof(resolutionFactory));
        }

        /// <inheritdoc/>
        protected override void OnRegisterRequest(IDependencyResolutionConfiguration container, IDependencyMethodInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                // BUG, This will create new instance for each resolve. Needs to be factory mapper if more than one. For now known issue.
                _services.AddScoped(resolveType, p => info.Execute(info.CreateContext(Ioc.Container)));
            }

            base.OnRegisterRequest(container, info);
        }
        
        /// <inheritdoc/>
        protected override void OnRegisterSingleton(IDependencyResolutionConfiguration container, IDependencyMethodInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                // BUG, This will create new instance for each resolve. Needs to be factory mapper if more than one. For now known issue.
                _services.AddSingleton(resolveType, p => info.Execute(info.CreateContext(Ioc.Container)));
            }

            base.OnRegisterSingleton(container, info);
        }

        /// <inheritdoc/>
        protected override void OnRegisterTransient(IDependencyResolutionConfiguration container, IDependencyMethodInfo info)
        {
            foreach (var resolveType in info.ResolvedTypes)
            {
                _services.AddTransient(resolveType, p => info.Execute(info.CreateContext(Ioc.Container)));
            }

            base.OnRegisterTransient(container, info);
        }

        /// <inheritdoc/>
        protected override void OnRegisterThread(IDependencyResolutionConfiguration container, IDependencyMethodInfo info)
        {
            var tracked = new DependencyThreadResolution<IDependencyMethodInfo, IDependencyResolution<IDependencyMethodInfo>>(_resolutionFactory.ResolutionFor(info));
            
            foreach (var resolveType in info.ResolvedTypes)
            {
                _services.AddTransient(resolveType, p => tracked.Resolve());
            }

            base.OnRegisterThread(container, info);
        }
    }
}