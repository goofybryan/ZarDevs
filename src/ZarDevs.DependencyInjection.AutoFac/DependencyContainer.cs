using Autofac;
using Autofac.Builder;
using ZarDevs.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    public interface IAutoFacDependencyContainer : IDependencyContainer
    {
        #region Properties

        IContainer Container { get; }

        #endregion Properties

        #region Methods

        bool HasModule(string name);

        #endregion Methods
    }

    public class DependencyContainer : IAutoFacDependencyContainer
    {
        #region Fields

        private readonly ContainerBuildOptions _buildOptions;

        #endregion Fields

        #region Constructors

        private DependencyContainer(ContainerBuildOptions buildOptions)
        {
            Builder = new ContainerBuilder();
            _buildOptions = buildOptions;
            Modules = new List<IDependencyModule>();
        }

        #endregion Constructors

        #region Properties

        public ContainerBuilder Builder { get; }
        public IContainer Container { get; private set; }
        public IList<IDependencyModule> Modules { get; }

        #endregion Properties

        #region Methods

        public static IAutoFacDependencyContainer Create(ContainerBuildOptions buildOptions) => new DependencyContainer(buildOptions);

        public void Build(IList<IDependencyInfo> definitions)
        {
            Check.IsNotNull(definitions, nameof(definitions));

#pragma warning disable CA1062 // Validate arguments of public methods
            foreach (IDependencyInfo info in definitions)
#pragma warning restore CA1062 // Validate arguments of public methods
            {
                if (!TryRegisterTypeTo(Builder, info as IDependencyTypeInfo) && !TryRegisterMethod(Builder, info as IDependencyMethodInfo))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", info.TypeFrom));
            }

            Container = Builder.Build(_buildOptions);
        }

        public bool HasModule(string name)
        {
            return Modules.Any(m => m.Name == name);
        }

        internal static void Build<TActivatorData, TRegistrationStyle>(IDependencyInfo info, IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> binding)
        {
            binding.As(info.TypeFrom);

            switch (info.Scope)
            {
                case DependyBuilderScope.Transient:
                    binding.InstancePerDependency();
                    break;

                case DependyBuilderScope.Request:
                    binding.InstancePerRequest();
                    break;

                case DependyBuilderScope.Singleton:
                    binding.SingleInstance();
                    break;
            }
        }

        private bool TryRegisterMethod(ContainerBuilder builder, IDependencyMethodInfo info)
        {
            if (info == null)
                return false;

            var module = new MethodModule(info);
            builder.RegisterModule(module);
            Modules.Add(module);

            return true;
        }

        private bool TryRegisterTypeTo(ContainerBuilder builder, IDependencyTypeInfo info)
        {
            if (info == null)
                return false;

            var binding = builder.RegisterType(info.TypeTo);

            if (!string.IsNullOrWhiteSpace(info.Name))
                binding.Named(info.Name, info.TypeTo);

            Build(info, binding);

            return true;
        }

        #endregion Methods
    }
}