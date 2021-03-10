using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency factory that is used to resolve a factory call.
    /// </summary>
    public interface IDependencyFactory
    {
        #region Properties

        /// <summary>
        /// Get a list of factory dependency info that this factory can resolve. This list gets
        /// updated when <see cref="MakeConcrete(IDependencyFactoryInfo)"/> or <see
        /// cref="Resolve(IDependencyContext)"/> is called.
        /// </summary>
        IReadOnlyList<IDependencyFactoryInfo> FactoryInfos { get; }

        /// <summary>
        /// Get an instance of the runtime method inspector.
        /// </summary>
        IInspectMethod InspectMethod { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generate a concrete factory from the info supplied.
        /// </summary>
        /// <param name="concreteInfo">The concrete type.</param>
        /// <returns></returns>
        IDependencyFactory MakeConcrete(IDependencyFactoryInfo concreteInfo);

        /// <summary>
        /// Resolve the instance of the requested type from the factory.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object Resolve(IDependencyContext context);

        #endregion Methods
    }

    /// <summary>
    /// Dependency factory that is used to resolve a factory call.
    /// </summary>
    public class DependencyFactory : IDependencyFactory
    {
        #region Fields

        private readonly List<IDependencyFactoryInfo> _factoryInfos;
        private readonly IInspectMethod _inspectMethod;
        private readonly ConcurrentDictionary<Type, IDependencyFactoryResolutionPlan> _resolutionPlans;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create new instance of dependnecy factory.
        /// </summary>
        /// <param name="inspectMethod">The runtime method inspector.</param>
        public DependencyFactory(IInspectMethod inspectMethod)
        {
            _resolutionPlans = new ConcurrentDictionary<Type, IDependencyFactoryResolutionPlan>();
            _inspectMethod = inspectMethod ?? throw new ArgumentNullException(nameof(inspectMethod));
            _factoryInfos = new List<IDependencyFactoryInfo>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get a list of factory dependency info that this factory can resolve. This list gets
        /// updated when <see cref="MakeConcrete(IDependencyFactoryInfo)"/> or <see
        /// cref="Resolve(IDependencyContext)"/> is called.
        /// </summary>
        public IReadOnlyList<IDependencyFactoryInfo> FactoryInfos => _factoryInfos;

        /// <summary>
        /// Get an instance of the runtime method inspector.
        /// </summary>
        public IInspectMethod InspectMethod => _inspectMethod;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generate a concrete factory from the info supplied.
        /// </summary>
        /// <param name="concreteInfo">The concrete type.</param>
        /// <returns></returns>
        public IDependencyFactory MakeConcrete(IDependencyFactoryInfo concreteInfo)
        {
            _resolutionPlans.GetOrAdd(concreteInfo.RequestType, type => CreatePlanForType(concreteInfo));

            return this;
        }

        /// <summary>
        /// Resolve the instance of the requested type from the factory.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public object Resolve(IDependencyContext context)
        {
            var info = (IDependencyFactoryInfo)context.Info;
            var factory = context.Ioc.TryResolve(info.FactoryType);

            if (factory == null) return null;

            IDependencyFactoryResolutionPlan plan = context.ArgumentCount == 0 ?
                  _resolutionPlans.GetOrAdd(info.RequestType, type => CreatePlanForType(info)) :
                  new DependencyFactoryArgumentsResolutionPlan(info.FactoryType, info.MethodName, InspectMethod);

            return plan.Resolve(factory, context);
        }

        private IDependencyFactoryResolutionPlan CreatePlanForType(IDependencyFactoryInfo info)
        {
            _factoryInfos.Add(info);
            var (method, parameters) = InspectMethod.GetMethodParameterMap(info.FactoryType, info.MethodName);
            return new DependencyFactoryResolveResolutionPlan(method, parameters);
        }

        #endregion Methods
    }
}