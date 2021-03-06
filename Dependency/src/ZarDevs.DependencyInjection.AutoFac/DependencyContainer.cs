using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// AutoFac dependency container
    /// </summary>
    public interface IAutoFacDependencyContainer : IDependencyContainer
    {
        #region Properties

        /// <summary>
        /// AutoFac
        /// </summary>
        IContainer Container { get; }

        #endregion Properties
    }

    internal class DependencyContainer : DependencyContainerBase, IAutoFacDependencyContainer
    {
        #region Fields

        private readonly ContainerBuildOptions _buildOptions;
        private readonly IDependencyFactory _dependencyFactory;

        #endregion Fields

        #region Constructors

        private DependencyContainer(ContainerBuildOptions buildOptions, IDependencyFactory dependencyFactory)
        {
            Builder = new ContainerBuilder();
            _buildOptions = buildOptions;
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        #endregion Constructors

        #region Properties

        public ContainerBuilder Builder { get; }
        public IContainer Container { get; private set; }

        #endregion Properties

        #region Methods

        public static IAutoFacDependencyContainer Create(ContainerBuildOptions buildOptions, IDependencyFactory dependencyFactory) => new DependencyContainer(buildOptions, dependencyFactory);

        protected override void OnBuild(IDependencyInfo info)
        {
            if (!TryRegisterTypeTo(Builder, info as IDependencyTypeInfo) && !TryRegisterMethod(Builder, info as IDependencyMethodInfo) && !TryRegisterInstance(Builder, info as IDependencyInstanceInfo) && !TryRegisterFactory(Builder, info))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", info));
        }

        protected override void OnBuildEnd()
        {
            Builder.RegisterGeneric(typeof(MultipleResolver<>)).As(typeof(IMultipleResolver<>));
            Container = Builder.Build(_buildOptions);
        }

        protected override void OnBuildStart()
        {
        }

        private static void Build<TActivatorData, TRegistrationStyle>(IDependencyInfo info, IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> binding)
        {
            binding.As(info.ResolvedTypes.ToArray());

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

        private object ExecuteFactory(IDependencyFactoryInfo info, IList<Parameter> parameters)
        {
            if (parameters == null || parameters.Count == 0) return _dependencyFactory.Resolve(info.CreateContext(Ioc.Container));
            if (TryGetNamedParameters(parameters, out (string, object)[] namedParameters)) return _dependencyFactory.Resolve(info.CreateContext(Ioc.Container).SetArguments(namedParameters));
            if (TryGetPositionalParameters(parameters, out object[] positionalParameters)) return _dependencyFactory.Resolve(info.CreateContext(Ioc.Container).SetArguments(positionalParameters));

            throw new NotSupportedException("Only AutoFac NamedParameter or PositionalParameter is supported.");
        }

        private static object ExecuteMethod(IDependencyMethodInfo info, IList<Parameter> parameters)
        {
            if (parameters == null || parameters.Count == 0) return info.Execute(info.CreateContext(Ioc.Container));
            if (TryGetNamedParameters(parameters, out (string, object)[] namedParameters)) return info.Execute(info.CreateContext(Ioc.Container).SetArguments(namedParameters));
            if (TryGetPositionalParameters(parameters, out object[] positionalParameters)) return info.Execute(info.CreateContext(Ioc.Container).SetArguments(positionalParameters));

            throw new NotSupportedException("Only AutoFac NamedParameter or PositionalParameter is supported.");
        }

        private static void RegisterNamedDependency<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> binding, IDependencyInfo info)
        {
            if (!TryRegisterNamedTypeTo(binding, info) && info.Key is not string && info.Key != null)
            {
                foreach (var type in info.ResolvedTypes)
                {
                    binding.Keyed(info.Key, type);
                }
            }
        }

        private static bool TryGetNamedParameters(IList<Parameter> parameters, out (string, object)[] namedParameters)
        {
            IList<(string, object)> args = new List<(string, object)>();
            foreach (var parameter in parameters.OfType<NamedParameter>())
            {
                args.Add(ValueTuple.Create(parameter.Name, parameter.Value));
            }

            namedParameters = args.ToArray();

            return namedParameters.Length > 0;
        }

        private static bool TryGetPositionalParameters(IList<Parameter> parameters, out object[] positionalParameters)
        {
            IList<object> args = new List<object>();

            foreach (var parameter in parameters.OfType<PositionalParameter>())
            {
                args.Add(parameter.Value);
            }

            positionalParameters = args.ToArray();

            return args.Count > 0;
        }

        private bool TryRegisterFactory(ContainerBuilder builder, IDependencyInfo info)
        {
            if (info is not IDependencyFactoryInfo factoryInfo)
                return false;

            if (factoryInfo.IsFactoryGeneric())
            {
                var binding = builder.RegisterGeneric((ctx, genericTypes, parameters) =>
                {
                    var concreteInfo = factoryInfo.As(genericTypes);
                    return ExecuteFactory(concreteInfo, parameters?.ToArray());
                });

                RegisterNamedDependency(binding, info);

                Build(info, binding);
            }
            else
            {
                var binding = builder.Register((ctx, p) =>
                {
                    return ExecuteFactory(factoryInfo, p?.ToArray());
                });

                RegisterNamedDependency(binding, info);

                Build(info, binding);
            }

            return true;
        }

        private static bool TryRegisterInstance(ContainerBuilder builder, IDependencyInstanceInfo info)
        {
            if (info == null)
                return false;

            var binding = builder.RegisterInstance(info.Instance);

            RegisterNamedDependency(binding, info);

            Build(info, binding);

            return true;
        }

        private static bool TryRegisterMethod(ContainerBuilder builder, IDependencyMethodInfo info)
        {
            if (info == null)
                return false;

            var binding = builder.Register((c, p) => ExecuteMethod(info, p?.ToArray()));

            RegisterNamedDependency(binding, info);

            Build(info, binding);

            return true;
        }

        private static bool TryRegisterNamedTypeTo<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> binding, IDependencyInfo info)
        {
            if (info.Key is not string name || string.IsNullOrWhiteSpace(name)) return false;

            foreach (var type in info.ResolvedTypes)
            {
                binding.Named(name, type);
            }

            return true;
        }

        private static bool TryRegisterTypeGeneric(ContainerBuilder builder, IDependencyTypeInfo info)
        {
            if (!info.ResolutionType.IsGenericType)
                return false;

            var binding = builder.RegisterGeneric(info.ResolutionType);

            RegisterNamedDependency(binding, info);
            Build(info, binding);

            return true;
        }

        private static bool TryRegisterTypeTo(ContainerBuilder builder, IDependencyTypeInfo info)
        {
            if (info == null)
                return false;

            if (TryRegisterTypeGeneric(builder, info))
                return true;

            var binding = builder.RegisterType(info.ResolutionType);

            RegisterNamedDependency(binding, info);
            Build(info, binding);

            return true;
        }

        #endregion Methods
    }
}