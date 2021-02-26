using Autofac;
using Autofac.Builder;
using Autofac.Core;
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
    }

    public class DependencyContainer : DependencyContainerBase, IAutoFacDependencyContainer
    {
        #region Fields

        private readonly ContainerBuildOptions _buildOptions;

        #endregion Fields

        #region Constructors

        private DependencyContainer(ContainerBuildOptions buildOptions)
        {
            Builder = new ContainerBuilder();
            _buildOptions = buildOptions;
        }

        #endregion Constructors

        #region Properties

        public ContainerBuilder Builder { get; }
        public IContainer Container { get; private set; }

        #endregion Properties

        #region Methods

        public static IAutoFacDependencyContainer Create(ContainerBuildOptions buildOptions) => new DependencyContainer(buildOptions);

        protected override void OnBuild(IDependencyInfo info)
        {
            if (!TryRegisterTypeTo(Builder, info as IDependencyTypeInfo) && !TryRegisterMethod(Builder, info as IDependencyMethodInfo))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", info.RequestType));
        }

        protected override void OnBuildEnd()
        {
            Container = Builder.Build(_buildOptions);
        }

        protected override void OnBuildStart()
        {
        }

        private static void Build<TActivatorData, TRegistrationStyle>(IDependencyInfo info, IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> binding)
        {
            binding.As(info.RequestType);

            switch (info.Scope)
            {
                case DependyBuilderScope.Transient:
                    binding.InstancePerDependency();
                    break;

                case DependyBuilderScope.Singleton:
                    binding.SingleInstance();
                    break;
            }
        }

        private DepencyBuilderInfoContext CreateBuilderContext(IComponentContext componentContext, IList<Parameter> parameters, Type requestType)
        {
            if (requestType == typeof(IIocContainer)) return null;

            IIocContainer container = componentContext.Resolve<IIocContainer>();
            if (parameters == null || parameters.Count == 0) return new DepencyBuilderInfoContext(container);
            if (TryGetNamedParameters(parameters, out (string, object)[] namedParameters)) return new DepencyBuilderInfoContext(container, namedParameters);
            if (TryGetPositionalParameters(parameters, out object[] positionalParameters)) return new DepencyBuilderInfoContext(container, positionalParameters);

            throw new NotSupportedException("Only AutoFac NamedParameter or PositionalParameter is supported.");
        }

        private void RegisterNamedDependency<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> binding, IDependencyInfo info)
        {
            if (!TryRegisterNamedTypeTo(binding, info) && info.Key is not string && info.Key != null)
                binding.Keyed(info.Key, info.RequestType);
        }

        private bool TryGetNamedParameters(IList<Parameter> parameters, out (string, object)[] namedParameters)
        {
            IList<(string, object)> args = new List<(string, object)>();
            foreach (var parameter in parameters.OfType<NamedParameter>())
            {
                args.Add(ValueTuple.Create(parameter.Name, parameter.Value));
            }

            namedParameters = args.ToArray();

            return namedParameters.Length > 0;
        }

        private bool TryGetPositionalParameters(IList<Parameter> parameters, out object[] positionalParameters)
        {
            IList<object> args = new List<object>();

            foreach (var parameter in parameters.OfType<PositionalParameter>())
            {
                args.Add(parameter.Value);
            }

            positionalParameters = args.ToArray();

            return args.Count > 0;
        }

        private bool TryRegisterMethod(ContainerBuilder builder, IDependencyMethodInfo info)
        {
            if (info == null)
                return false;

            var binding = builder.Register((c, p) => info.Method(CreateBuilderContext(c, p?.ToList(), info.RequestType), info.Key));

            RegisterNamedDependency(binding, info);

            Build(info, binding);

            return true;
        }

        private bool TryRegisterNamedTypeTo<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> binding, IDependencyInfo info)
        {
            if (info.Key is not string name || string.IsNullOrWhiteSpace(name)) return false;

            binding.Named(name, info.RequestType);

            return true;
        }

        private bool TryRegisterTypeTo(ContainerBuilder builder, IDependencyTypeInfo info)
        {
            if (info == null)
                return false;

            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> binding = builder.RegisterType(info.ResolvedType);

            RegisterNamedDependency(binding, info);

            Build(info, binding);

            return true;
        }

        #endregion Methods
    }
}