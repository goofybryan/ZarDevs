using Ninject.Syntax;
using Ninject.Web.Common;
using System;

namespace ZarDevs.DependencyInjection;

/// <summary>
/// Ninject's scope binder. It is valid for <see cref="DependyBuilderScopes.Request"/>
/// </summary>
public class NinjectDependencyWebScopeBinder : NinjectDependencyScopeBinderBase
{
    #region Constructors

    /// <summary>
    /// Create a new instance of the <see cref="NinjectDependencyScopeBinder"/>
    /// </summary>
    /// <param name="dependencyFactory">The dependency factory</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="dependencyFactory"/> is null
    /// </exception>
    public NinjectDependencyWebScopeBinder(IDependencyFactory dependencyFactory) : base(dependencyFactory, DependyBuilderScopes.Request)
    {
    }

    #endregion Constructors

    #region Methods

    protected override void BindScope(IDependencyInfo info, IBindingWhenInNamedWithOrOnSyntax<object> binding)
    {
        binding.InRequestScope();
    }

    #endregion Methods
}