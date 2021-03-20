using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal interface IRuntimeDependencyActivator : IDependencyTypeActivator
    {
    }

    /// <summary>
    /// <see cref="IIocContainer"/> extensions
    /// </summary>
    public static class IIocContainerExtensions
    {
        #region Methods

        /// <summary>
        /// Cast the current container to <see cref="IDependencyResolver"/>.
        /// </summary>
        /// <param name="container">The container to cast</param>
        /// <returns></returns>
        public static IDependencyResolver Resolver(this IIocContainer container)
        {
            return (IDependencyResolver)container;
        }

        #endregion Methods
    }

    internal class RuntimeDependencyActivator : IDependencyTypeActivator
    {
        #region Fields

        private readonly ICreate _creation;
        private readonly IInspectConstructor _inspection;
        private readonly IRuntimeResolutionPlanCreator _planCreator;
        private readonly IDictionary<Type, IRuntimeResolutionPlan> _resolutionPlan;

        #endregion Fields

        #region Constructors

        public RuntimeDependencyActivator(IInspectConstructor inspection, ICreate creation, IRuntimeResolutionPlanCreator planCreator)
        {
            _inspection = inspection ?? throw new ArgumentNullException(nameof(inspection));
            _creation = creation ?? throw new ArgumentNullException(nameof(creation));
            _planCreator = planCreator ?? throw new ArgumentNullException(nameof(planCreator));
            _resolutionPlan = new Dictionary<Type, IRuntimeResolutionPlan>();
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
                var plan = _planCreator.FromInfo(info);
                resolution = _resolutionPlan[type] = _planCreator.UseExpressions ? _planCreator.ToExpressionPlan(plan) : plan;
            }

            return resolution.Resolve();
        }

        #endregion Methods
    }

    internal class RuntimeResolutionPlanType : IRuntimeResolutionPlan
    {
        #region Fields

        private readonly IRuntimeResolutionPlanCreator _planCreator;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanType(IRuntimeResolutionPlanCreator planCreator, ConstructorInfo constructor, IList<Type> constructorArgs)
        {
            _planCreator = planCreator ?? throw new ArgumentNullException(nameof(planCreator));
            Constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
            ConstructorArgs = CreateParameterResolvers(constructorArgs ?? throw new ArgumentNullException(nameof(constructorArgs)));
        }

        #endregion Constructors

        #region Properties

        public ConstructorInfo Constructor { get; }
        public IList<IRuntimeResolutionPlanParameterResolver> ConstructorArgs { get; }

        #endregion Properties

        #region Methods

        public Expression CreateExpression()
        {
            if (ConstructorArgs.Count > 0)
            {
                return Expression.New(Constructor, ConstructorArgs.Select(c => c.GetExpression()));
            }

            return Expression.New(Constructor);
        }

        public object Resolve() => Constructor.Invoke(ResolveParameters());

        public object[] ResolveParameters()
        {
            return (ConstructorArgs.Count == 0) ? Array.Empty<object>() : ResolveArgs();
        }

        private IList<IRuntimeResolutionPlanParameterResolver> CreateParameterResolvers(IList<Type> constructorArgs)
        {
            var list = new List<IRuntimeResolutionPlanParameterResolver>();
            var resolver = Ioc.Container.Resolver();

            foreach (Type argType in constructorArgs)
            {
                if (argType.IsArray)
                    list.Add(new RuntimeResolutionPlanListParameter(_planCreator, resolver.TryGetAllResolutions(argType.GetElementType())));
                else if (typeof(IEnumerable).IsAssignableFrom(argType) && argType.GenericTypeArguments.Length > 0)
                    list.Add(new RuntimeResolutionPlanListParameter(_planCreator, resolver.TryGetAllResolutions(argType.GenericTypeArguments[0])));
                else
                    list.Add(new RuntimeResolutionPlanSingleParameter(_planCreator, resolver.TryGetResolution(argType)));
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
}