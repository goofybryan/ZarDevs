using Ninject;
using Ninject.Activation;
using Ninject.Syntax;
using System;
using System.Globalization;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ninject's scope binder. It is valid for <see cref="DependyBuilderScopes.Singleton"/>, <see
    /// cref="DependyBuilderScopes.Transient"/> and <see cref="DependyBuilderScopes.Thread"/>
    /// </summary>
    public class NinjectDependencyScopeBinder : NinjectDependencyScopeBinderBase
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="NinjectDependencyScopeBinder"/>
        /// </summary>
        /// <param name="dependencyFactory">The dependency factory</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="dependencyFactory"/> is null
        /// </exception>
        public NinjectDependencyScopeBinder(IDependencyFactory dependencyFactory) : base(dependencyFactory, DependyBuilderScopes.Singleton | DependyBuilderScopes.Transient | DependyBuilderScopes.Thread)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        protected override void BindScope(IDependencyInfo info, IBindingWhenInNamedWithOrOnSyntax<object> binding)
        {
            switch (info.Scope)
            {
                case DependyBuilderScopes.Transient:
                    binding.InTransientScope();
                    break;

                case DependyBuilderScopes.Singleton:
                    binding.InSingletonScope();
                    break;

                case DependyBuilderScopes.Thread:
                    binding.InThreadScope();
                    break;
            }
        }

        #endregion Methods
    }
}