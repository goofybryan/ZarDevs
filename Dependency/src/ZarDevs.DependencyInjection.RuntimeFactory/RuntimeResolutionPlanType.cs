using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ZarDevs.DependencyInjection
{
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