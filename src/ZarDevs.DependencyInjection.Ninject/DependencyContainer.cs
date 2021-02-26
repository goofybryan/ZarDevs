using Ninject;
using Ninject.Activation;
using Ninject.Syntax;
using System;
using System.Globalization;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    public class DependencyContainer : DependencyContainerBase
    {
        #region Constructors

        private DependencyContainer(IKernel kernel)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public static IDependencyContainer Create(IKernel kernel) => new DependencyContainer(kernel);

        protected override void OnBuild(IDependencyInfo info)
        {
            IBindingToSyntax<object> initial = Kernel.Bind(info.RequestType);

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

                case DependyBuilderScope.Request:
                    binding.InThreadScope();
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
                return initial.ToMethod(ctx => methodInfo.MethodTo(CreateContext(ctx), info.Key));
            }

            if (info is IDependencyTypeInfo typeInfo)
            {
                return initial.To(typeInfo.ResolvedType);
            }

            if(info is IDependencyInstanceInfo instanceInfo)
            {
                return initial.ToConstant(instanceInfo.Instance);
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", info.RequestType));
        }

        private DepencyBuilderInfoContext CreateContext(IContext ctx)
        {
            if (ctx.Request.Service == typeof(IIocContainer)) return null;

            return new DepencyBuilderInfoContext(ctx.Kernel.Get<IIocContainer>(),
                      ctx.Parameters.Select(s => ValueTuple.Create(s.Name, s.GetValue(ctx, ctx.Request.Target))).ToArray());
        }

        #endregion Methods
    }
}