using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeDependencyActivator : IDependencyTypeActivator
    {
        #region Fields

        private readonly ICreate _creation;
        private readonly IInspectConstructor _inspection;
        private readonly ConcurrentDictionary<Type, RuntimeResolutionPlan> _resolutionPlan;

        #endregion Fields

        #region Constructors

        public RuntimeDependencyActivator(IInspectConstructor inspection, ICreate creation)
        {
            _inspection = inspection ?? throw new ArgumentNullException(nameof(inspection));
            _creation = creation ?? throw new ArgumentNullException(nameof(creation));
            _resolutionPlan = new ConcurrentDictionary<Type, RuntimeResolutionPlan>();
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
            var resolution = _resolutionPlan.GetOrAdd(info.ResolvedType, type => RuntimeResolutionPlan.FromType(_inspection, type));
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
            ConstructorArgs = constructorArgs ?? throw new ArgumentNullException(nameof(constructorArgs));
        }

        #endregion Constructors

        #region Properties

        public ConstructorInfo Constructor { get; }
        public IList<Type> ConstructorArgs { get; }

        #endregion Properties

        #region Methods

        public static RuntimeResolutionPlan FromType(IInspectConstructor inspect, Type instanceType)
        {
            var (constructor, args) = inspect.GetConstructorParameterMap(instanceType);
            return new RuntimeResolutionPlan(constructor, args);
        }

        public object Resolve() => Constructor.Invoke(ResolveParameters().ToArray());

        public IEnumerable<object> ResolveParameters()
        {
            return (ConstructorArgs.Count == 0) ? Enumerable.Empty<object>() : YieldResolve();
        }

        private IEnumerable<object> YieldResolve()
        {
            foreach (Type argType in ConstructorArgs)
            {
                if (argType.IsArray)
                    yield return Ioc.Container.ResolveAll(argType.GetElementType());
                else if (typeof(IEnumerable).IsAssignableFrom(argType) && argType.GenericTypeArguments.Length > 0)
                    yield return Ioc.Container.ResolveAll(argType.GenericTypeArguments[0]);
                else
                    yield return Ioc.Container.TryResolve(argType);
            }
        }

        #endregion Methods
    }
}