using Ninject;
using Ninject.Activation;
using Ninject.Syntax;
using System;
using System.Globalization;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ninject's scope binder base. It is valid for <see cref="DependyBuilderScopes.Singleton"/>,
    /// <see cref="DependyBuilderScopes.Transient"/> and <see cref="DependyBuilderScopes.Thread"/>
    /// </summary>
    public abstract class NinjectDependencyScopeBinderBase : DependencyScopeBinderBase<IKernel>
    {
        #region Fields

        private readonly IDependencyFactory _dependencyFactory;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="NinjectDependencyScopeBinder"/>
        /// </summary>
        /// <param name="dependencyFactory">The dependency factory</param>
        /// <param name="scopes">The scopes that this is valid for.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="dependencyFactory"/> is null
        /// </exception>
        protected NinjectDependencyScopeBinderBase(IDependencyFactory dependencyFactory, DependyBuilderScopes scopes) : base(scopes)
        {
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Bind the <paramref name="info"/> to a scope
        /// </summary>
        /// <param name="info">The <see cref="IDependencyInfo"/> describing what is being bound.</param>
        /// <param name="binding">The current binding.</param>
        protected abstract void BindScope(IDependencyInfo info, IBindingWhenInNamedWithOrOnSyntax<object> binding);

        /// <inheritdoc/>
        protected override void OnBind(IKernel container, IDependencyInfo definition)
        {
            IBindingToSyntax<object> initial = container.Bind(definition.ResolvedTypes.ToArray());

            IBindingWhenInNamedWithOrOnSyntax<object> binding = BuildTo(definition, initial);
            BindScope(definition, binding);
            BindNamedIfConfigured(definition, binding);
        }

        private static void BindNamedIfConfigured(IDependencyInfo info, IBindingWhenInNamedWithOrOnSyntax<object> binding)
        {
            string named;

            if (info.Key is Enum enumKey)
            {
                named = enumKey.GetBindingName();
            }
            else if (info.Key is string name)
            {
                named = name;
            }
            else
            {
                named = info.Key?.ToString();
            }

            if (!string.IsNullOrWhiteSpace(named))
            {
                binding.Named(named);
            }
        }

        private static object ExecuteMethod(IDependencyMethodInfo info, IContext ctx)
        {
            var context = info.CreateContext(Ioc.Container);
            if (ctx.Parameters.Count == 0) return info.Execute(context);

            var args = ctx.Parameters.Select(s => ValueTuple.Create(s.Name, s.GetValue(ctx, ctx.Request.Target))).ToArray();

            return info.Execute(context.SetArguments(args));
        }

        private IBindingWhenInNamedWithOrOnSyntax<object> BuildTo(IDependencyInfo info, IBindingToSyntax<object> initial)
        {
            if (info is IDependencyMethodInfo methodInfo)
            {
                return initial.ToMethod(ctx => ExecuteMethod(methodInfo, ctx));
            }

            if (info is IDependencyTypeInfo typeInfo)
            {
                return initial.To(typeInfo.ResolutionType);
            }

            if (info is IDependencyInstanceInfo instanceInfo)
            {
                return initial.ToConstant(instanceInfo.Instance);
            }

            if (info is IDependencyFactoryInfo factoryInfo)
            {
                return initial.ToMethod(ctx => ExecuteFactory(factoryInfo, ctx));
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the types '{0}' is invalid. The binding has not been configured correctly", info));
        }

        private object ExecuteFactory(IDependencyFactoryInfo info, IContext ctx)
        {
            var executionInfo = info;

            if (info.IsFactoryGeneric())
            {
                executionInfo = info.As(ctx.GenericArguments);
            }

            var args = ctx.Parameters.Count == 0 ? null : ctx.Parameters.Select(s => ValueTuple.Create(s.Name, s.GetValue(ctx, ctx.Request.Target))).ToArray();
            return _dependencyFactory.Resolve(executionInfo.CreateContext(Ioc.Container).SetArguments(args));
        }

        #endregion Methods
    }
}