using Ninject;
using Ninject.Activation;
using Ninject.Syntax;
using System;
using System.Globalization;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyContainer : DependencyContainerBase
    {
        #region Fields

        private readonly IDependencyFactory _dependencyFactory;

        #endregion Fields

        #region Constructors

        private DependencyContainer(IKernel kernel, IDependencyFactory dependencyFactory)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public static IDependencyContainer Create(IKernel kernel, IDependencyFactory dependencyFactory) => new DependencyContainer(kernel, dependencyFactory);

        public override IDependencyInfo TryGetBinding(Type requestType, object key)
        {
            return base.TryGetBinding(requestType, key) ?? _dependencyFactory.FindFactory(requestType, key);
        }

        protected override void OnBuild(IDependencyInfo info)
        {
            IBindingToSyntax<object> initial = Kernel.Bind(info.ResolvedTypes.ToArray());

            IBindingWhenInNamedWithOrOnSyntax<object> binding = BuildTo(info, initial);
            BindScope(info, binding);
            BindNamedIfConfigured(info, binding);

            Definitions.Add(info);
        }

        protected override void OnBuildStart()
        {
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

        private static void BindScope(IDependencyInfo info, IBindingWhenInNamedWithOrOnSyntax<object> binding)
        {
            switch (info.Scope)
            {
                case DependyBuilderScope.Transient:
                    binding.InTransientScope();
                    break;

                case DependyBuilderScope.Singleton:
                    binding.InSingletonScope();
                    break;
            }
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

        private static object ExecuteMethod(IDependencyMethodInfo info, IContext ctx)
        {
            var context = info.CreateContext(Ioc.Container);
            if (ctx.Parameters.Count == 0) return info.Execute(context);

            var args = ctx.Parameters.Select(s => ValueTuple.Create(s.Name, s.GetValue(ctx, ctx.Request.Target))).ToArray();

            return info.Execute(context.SetArguments(args));
        }

        #endregion Methods
    }
}