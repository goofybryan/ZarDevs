using System;

namespace ZarDevs.DependencyInjection
{
    /// <inheritdoc/>
    public class DependencyResolutionFactory : IDependencyResolutionFactory
    {
        private readonly IDependencyTypeActivator _activator;
        private readonly IDependencyFactory _dependencyFactory;

        /// <summary>
        /// Create a new instance of the <see cref="DependencyResolutionFactory"/>
        /// </summary>
        /// <param name="activator">The dependency type activator</param>
        /// <param name="dependencyFactory">The defpendecy factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DependencyResolutionFactory(IDependencyTypeActivator activator, IDependencyFactory dependencyFactory)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        /// <inheritdoc/>
        public IDependencyResolution<IDependencyTypeInfo> ResolutionFor(IDependencyTypeInfo info) => new DependencyTypeResolution(info, _activator);

        /// <inheritdoc/>
        public IDependencyResolution<IDependencyMethodInfo> ResolutionFor(IDependencyMethodInfo info) => new DependencyMethodResolution(info);

        /// <inheritdoc/>
        public IDependencyResolution ResolutionFor(IDependencyInstanceInfo info) => new DependencySingletonInstance(info);

        /// <inheritdoc/>
        public IDependencyResolution<IDependencyFactoryInfo> ResolutionFor(IDependencyFactoryInfo info) => new DependencyFactoryResolution(info, _dependencyFactory);
    }
}