using ZarDevs.Core;
using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ZarDevs.DependencyInjection
{
    public class DependencyContainer : IDependencyContainer
    {
        #region Constructors

        private DependencyContainer(IKernel kernel)
        {
            Kernel = Check.IsNotNull(kernel, nameof(kernel));
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public static IDependencyContainer Create(IKernel kernel) => new DependencyContainer(kernel);

        public void Build(IList<IDependencyInfo> definitions)
        {
            if (definitions is null)
            {
                throw new ArgumentNullException(nameof(definitions));
            }

            Kernel.Bind<IIocContainer>().ToConstant(Ioc.Container);

#pragma warning disable CA1062 // Validate arguments of public methods
            foreach (var info in definitions)
#pragma warning restore CA1062 // Validate arguments of public methods
            {
                IBindingToSyntax<object> initial = Kernel.Bind(info.TypeFrom);

                IBindingWhenInNamedWithOrOnSyntax<object> binding = BuildTo(info, initial);

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

                if (!string.IsNullOrEmpty(info.Name))
                {
                    binding.Named(info.Name);
                }
            }
        }

        private IBindingWhenInNamedWithOrOnSyntax<object> BuildTo(IDependencyInfo info, IBindingToSyntax<object> initial)
        {
            if (info is IDependencyMethodInfo methodInfo)
            {
                return initial.ToMethod(ctx => methodInfo.MethodTo(new DepencyBuilderInfoContext(ctx.Request.Target.Member.DeclaringType, ctx.Request.Target.Type), info.Name));
            }

            if (info is IDependencyTypeInfo typeInfo)
            {
                return initial.To(typeInfo.TypeTo);
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", info.TypeFrom));
        }

        #endregion Methods
    }
}