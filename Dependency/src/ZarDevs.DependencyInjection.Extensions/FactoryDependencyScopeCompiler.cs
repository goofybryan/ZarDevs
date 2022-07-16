using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Compiler for <see cref="IDependencyFactoryInfo"/>
    /// </summary>
    public class FactoryDependencyScopeCompiler : DependencyScopeBinderBase<IDependencyResolutionConfiguration, IDependencyFactoryInfo>
    {
        private readonly IDependencyResolutionFactory _resolutionFactory;

        /// <summary>
        /// Create a new instance of the <see cref="FactoryDependencyScopeCompiler"/>
        /// </summary>
        /// <param name="resolutionFactory">An instance of the resolution factory.</param>
        /// <param name="scopes">Specify the scopes that this is valid for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolutionFactory"/> is null.</exception>
        public FactoryDependencyScopeCompiler(IDependencyResolutionFactory resolutionFactory, DependyBuilderScopes scopes) : base(scopes)
        {
            _resolutionFactory = resolutionFactory ?? throw new ArgumentNullException(nameof(resolutionFactory));
        }

        /// <inheritdoc/>
        protected override void OnRegisterTransient(IDependencyResolutionConfiguration container, IDependencyFactoryInfo info)
        {
            container.Add(info.ResolvedTypes, _resolutionFactory.ResolutionFor(info));
        }

        /// <inheritdoc/>
        protected override void OnRegisterSingleton(IDependencyResolutionConfiguration container, IDependencyFactoryInfo info)
        {
            container.Add(info.ResolvedTypes, new DependencySingletionResolution<IDependencyFactoryInfo, IDependencyResolution<IDependencyFactoryInfo>>(_resolutionFactory.ResolutionFor(info)));
        }

        /// <inheritdoc/>
        protected override void OnRegisterThread(IDependencyResolutionConfiguration container, IDependencyFactoryInfo info)
        {
            container.Add(info.ResolvedTypes, new DependencyThreadResolution<IDependencyFactoryInfo, IDependencyResolution<IDependencyFactoryInfo>>(_resolutionFactory.ResolutionFor(info)));
        }
    }
}