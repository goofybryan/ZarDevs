using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeDependencyActivator : IDependencyTypeActivator
    {
        #region Fields

        private readonly ICreate _creation;
        private readonly IInspectConstructor _inspection;
        private readonly IDictionary<Type, RuntimeResolutionPlan> _resolutionPlan;

        #endregion Fields

        #region Constructors

        public RuntimeDependencyActivator(IInspectConstructor inspection, ICreate creation)
        {
            _inspection = inspection ?? throw new ArgumentNullException(nameof(inspection));
            _creation = creation ?? throw new ArgumentNullException(nameof(creation));
            _resolutionPlan = new Dictionary<Type, RuntimeResolutionPlan>();
        }

        #endregion Constructors

        #region Methods

        public object Resolve(IDependencyTypeInfo info, params object[] args)
        {
            return _creation.New(info.ResolvedType, args);
        }

        public object Resolve(IDependencyTypeInfo info, params (string, object)[] args)
        {
            return Resolve(info, _inspection.OrderParameters(info.ResolvedType, args));
        }

        public object Resolve(IDependencyTypeInfo info)
        {
            var type = info.ResolvedType;
            if (!_resolutionPlan.TryGetValue(type, out var resolution))
            {
                resolution = _resolutionPlan[type] = RuntimeResolutionPlan.FromType(_inspection, type);
            }

            return resolution.Resolve();
        }

        #endregion Methods
    }

    internal class RuntimeResolutionPlan
    {
        #region Constructors

        public RuntimeResolutionPlan(ConstructorInfo constructor, IList<Type> constructorArgs)
        {
            Constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
            ConstructorArgs = CreateParameterResolvers(constructorArgs ?? throw new ArgumentNullException(nameof(constructorArgs)));
        }

        #endregion Constructors

        #region Properties

        public ConstructorInfo Constructor { get; }
        public IList<IRuntimeResolutionPlanParameterResolver> ConstructorArgs { get; }

        #endregion Properties

        #region Methods

        public static RuntimeResolutionPlan FromType(IInspectConstructor inspect, Type instanceType)
        {
            var (constructor, args) = inspect.GetConstructorParameterMap(instanceType);
            return new RuntimeResolutionPlan(constructor, args);
        }

        public object Resolve() => Constructor.Invoke(ResolveParameters());

        public object[] ResolveParameters()
        {
            return (ConstructorArgs.Count == 0) ? new object[0] : ResolveArgs();
        }

        private IList<IRuntimeResolutionPlanParameterResolver> CreateParameterResolvers(IList<Type> constructorArgs)
        {
            var list = new List<IRuntimeResolutionPlanParameterResolver>();
            var resolver = Ioc.Container.Resolver();

            foreach (Type argType in constructorArgs)
            {
                if (argType.IsArray)
                    list.Add(new RuntimeResolutionPlanListParameter(resolver.TryGetAllResolutions(argType.GetElementType())));
                else if (typeof(IEnumerable).IsAssignableFrom(argType) && argType.GenericTypeArguments.Length > 0)
                    list.Add(new RuntimeResolutionPlanListParameter(resolver.TryGetAllResolutions(argType.GenericTypeArguments[0])));
                else
                    list.Add(new RuntimeResolutionPlanSingleParameter(resolver.TryGetResolution(argType)));
            }

            return list;
        }

        private object[] ResolveArgs()
        {
            var args = new object[ConstructorArgs.Count];
            for (int i = 0; i < ConstructorArgs.Count; i++)
            {
                args[i] = ConstructorArgs[i].Resolve();
            }

            return args;
        }

        #endregion Methods
    }

    /// <summary>
    /// <see cref="IIocContainer"/> extensions
    /// </summary>
    public static class IIocContainerExtensions
    {
        /// <summary>
        /// Cast the current container to <see cref="IDependencyResolver"/>.
        /// </summary>
        /// <param name="container">The container to cast</param>
        /// <returns></returns>
        public static IDependencyResolver Resolver(this IIocContainer container)
        {
            return (IDependencyResolver)container;
        }
    }
}