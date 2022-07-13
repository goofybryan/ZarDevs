using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency container that is used to build bindings and apply them to the configuration.
    /// </summary>
    public class DependencyContainer : DependencyContainerBase<IDependencyResolutionConfiguration>
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the dependency container.
        /// </summary>
        /// <param name="configuration">
        /// The instance configuration that will contain the binding configuration.
        /// </param>
        /// <param name="scopeCompiler">Scope compiler</param>
        public DependencyContainer(IDependencyResolutionConfiguration configuration, IDependencyScopeCompiler<IDependencyResolutionConfiguration> scopeCompiler) : base(configuration, scopeCompiler)
        {
        }

        #endregion Constructors
    }
}