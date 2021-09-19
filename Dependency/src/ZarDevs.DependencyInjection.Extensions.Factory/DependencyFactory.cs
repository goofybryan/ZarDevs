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
        /// Get an instance of the runtime method inspector.
        /// </summary>
        IInspectMethod InspectMethod { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get the factory for the resolve type and key
        /// </summary>
        /// <param name="forResolveType">Specify the resolve type to find.</param>
        /// <param name="key">Specify the key, null is an empty key, so will be returned.</param>
        /// <returns></returns>
        IDependencyFactoryInfo FindFactory(Type forResolveType, object key);

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

        private readonly bool _enableExpressions;
        private readonly IDictionary<(Type, object), IDependencyFactoryInfo> _map;
        private readonly IInspectMethod _inspectMethod;
        private readonly ConcurrentDictionary<IDependencyFactoryInfo, IDependencyFactoryResolutionPlan> _resolutionPlans;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create new instance of dependnecy factory.
        /// </summary>
        /// <param name="inspectMethod">The runtime method inspector.</param>
        /// <param name="enableExpressions">
        /// Enable the resolving to make use of <see cref="System.Linq.Expressions.Expression"/> to
        /// execute the factory. Default is true/&gt;
        /// </param>
        public DependencyFactory(IInspectMethod inspectMethod, bool enableExpressions = true)
        {
            _resolutionPlans = new();
            _inspectMethod = inspectMethod ?? throw new ArgumentNullException(nameof(inspectMethod));
            _enableExpressions = enableExpressions;
            _map = new Dictionary<(Type, object), IDependencyFactoryInfo>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get an instance of the runtime method inspector.
        /// </summary>
        public IInspectMethod InspectMethod => _inspectMethod;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get the factory for the resolve type and key
        /// </summary>
        /// <param name="forResolveType">Specify the resolve type to find.</param>
        /// <param name="key">Specify the key, null is an empty key, so will be returned.</param>
        /// <returns>The factory or null if not found.</returns>
        public IDependencyFactoryInfo FindFactory(Type forResolveType, object key)
        {
            (Type, object) mapKey = new(forResolveType, key);

            return _map.TryGetValue(mapKey, out var factoryInfo) ? factoryInfo : null;
        }

        /// <summary>
        /// Generate a concrete factory from the info supplied.
        /// </summary>
        /// <param name="concreteInfo">The concrete type.</param>
        /// <returns>The current instance</returns>
        public IDependencyFactory MakeConcrete(IDependencyFactoryInfo concreteInfo)
        {            
            _resolutionPlans.GetOrAdd(concreteInfo, type => CreatePlanForType(concreteInfo));

            return this;
        }

        /// <summary>
        /// Resolve the instance of the requested type from the factory.
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <returns>The resolved object for the context.</returns>
        public object Resolve(IDependencyContext context)
        {
            var info = (IDependencyFactoryInfo)context.Info;
            var factory = context.Ioc.TryResolve(info.FactoryType);

            if (factory == null) return null;

            IDependencyFactoryResolutionPlan plan = context.ArgumentCount == 0 ?
                  _resolutionPlans.GetOrAdd(info, type => CreatePlanForType(info)) :
                  new DependencyFactoryArgumentsResolutionPlan(info.FactoryType, info.MethodName, InspectMethod);

            return plan.Resolve(factory, context);
        }

        private IDependencyFactoryResolutionPlan CreatePlanForType(IDependencyFactoryInfo info)
        {
            foreach(var type in info.ResolvedTypes)
            {
                _map[new(type, info.Key)] = info;
            }
            var (method, parameters) = InspectMethod.GetMethodParameterMap(info.FactoryType, info.MethodName);
            var plan = new DependencyFactoryResolveResolutionPlan(method, parameters);

            return _enableExpressions ? new DependencyFactoryResolveExpressionPlan(plan.GetExpression()) : plan;
        }

        #endregion Methods
    }
}