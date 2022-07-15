using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency scope provider that is used to register <see cref="IServiceCollection"/> dependencies for <see cref="IDependencyInstanceInfo"/>.
    /// </summary>
    public class MicrosoftInstanceDependencyScopeCompiler : InstanceDependencyScopeCompiler
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Create a new instance of the <see cref="MicrosoftFactoryDependencyScopeCompiler"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="resolutionFactory">An instance of the resolution factory.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftInstanceDependencyScopeCompiler(IServiceCollection services, IDependencyResolutionFactory resolutionFactory)
            : base(resolutionFactory)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc/>
        protected override void OnBind(IDependencyResolutionConfiguration container, IDependencyInstanceInfo definition)
        {
            foreach (var resolveType in definition.ResolvedTypes)
            {
                _services.AddSingleton(resolveType, definition.Instance);
            }

            base.OnBind(container, definition);
        }
    }
}