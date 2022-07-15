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
    /// AutoFac dependency scope binder. This will convert bind the <see cref="IDependencyInfo"/> and map to the <see cref="ContainerBuilder"/>
    /// </summary>
    public class AutoFacDependencyScopeBinder : DependencyScopeBinderBase<ContainerBuilder>
    {
        private readonly IDependencyFactory _dependencyFactory;

        /// <summary>
        /// Create new instance of the <see cref="AutoFacDependencyScopeBinder"/>
        /// </summary>
        /// <param name="dependencyFactory">An instance of the dependency factory.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="dependencyFactory"/> is null.</exception>
        public AutoFacDependencyScopeBinder(IDependencyFactory dependencyFactory) : base(DependyBuilderScopes.Singleton | DependyBuilderScopes.Transient | DependyBuilderScopes.Request | DependyBuilderScopes.Thread)
        {
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        /// <inheritdoc/>
        protected override void OnBind(ContainerBuilder container, IDependencyInfo definition)
        {
            if (!TryRegisterTypeTo(container, definition as IDependencyTypeInfo)
                && !TryRegisterMethod(container, definition as IDependencyMethodInfo)
                && !TryRegisterInstance(container, definition as IDependencyInstanceInfo)
                && !TryRegisterFactory(container, definition))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", definition));
        }

        private static void Build<TActivatorData, TRegistrationStyle>(IDependencyInfo info, IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> binding)
        {
            binding.As(info.ResolvedTypes.ToArray());

            switch (info.Scope)
            {
                case DependyBuilderScopes.Transient:
                    binding.InstancePerDependency();
                    break;

                case DependyBuilderScopes.Singleton:
                    binding.SingleInstance();
                    break;
                case DependyBuilderScopes.Request:
                    binding.InstancePerRequest();
                    break;
                case DependyBuilderScopes.Thread:
                    binding.InstancePerLifetimeScope();
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
    }
}