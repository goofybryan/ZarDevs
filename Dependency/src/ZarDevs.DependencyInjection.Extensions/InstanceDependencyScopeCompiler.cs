using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Compiler for <see cref="IDependencyTypeInfo"/>
    /// </summary>
    public class InstanceDependencyScopeCompiler : DependencyScopeBinderBase<IDependencyResolutionConfiguration>
    {
        private readonly IDependencyResolutionFactory _resolutionFactory;

        /// <summary>
        /// Create a new instance of the <see cref="InstanceDependencyScopeCompiler"/>
        /// </summary>
        /// <param name="resolutionFactory">An instance of the resolution factory.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolutionFactory"/> is null.</exception>
        public InstanceDependencyScopeCompiler(IDependencyResolutionFactory resolutionFactory) : base(DependyBuilderScopes.Singleton)
        {
            _resolutionFactory = resolutionFactory ?? throw new ArgumentNullException(nameof(resolutionFactory));
        }

        /// <inheritdoc/>
        public override bool CanBind(IDependencyInfo definition)
        {
            return definition is IDependencyInstanceInfo;
        }

        /// <inheritdoc/>
        protected override void OnBind(IDependencyResolutionConfiguration container, IDependencyInfo definition)
        {
            IDependencyInstanceInfo instanceInfo = (IDependencyInstanceInfo)definition;

            OnBind(container, instanceInfo);
        }

        /// <summary>
        /// Bind the definition to the container.
        /// </summary>
        /// <param name="container">The dependency container</param>
        /// <param name="definition">The dependency info</param>
        protected virtual void OnBind(IDependencyResolutionConfiguration container, IDependencyInstanceInfo definition)
        {
            foreach (var resolvedType in definition.ResolvedTypes)
            {
                container.Add(resolvedType, _resolutionFactory.ResolutionFor(definition));
            }
        }
    }
}