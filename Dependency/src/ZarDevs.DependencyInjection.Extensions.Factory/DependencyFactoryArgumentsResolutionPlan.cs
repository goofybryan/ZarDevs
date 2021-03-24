using System;
using System.Linq.Expressions;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyFactoryArgumentsResolutionPlan : IDependencyFactoryResolutionPlan
    {
        #region Fields

        private readonly Type _factoryType;
        private readonly IInspectMethod _inspectMethod;
        private readonly string _methodName;

        #endregion Fields

        #region Constructors

        public DependencyFactoryArgumentsResolutionPlan(Type factoryType, string methodName, IInspectMethod inspectMethod)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException($"'{nameof(methodName)}' cannot be null or whitespace.", nameof(methodName));
            }

            _factoryType = factoryType ?? throw new ArgumentNullException(nameof(factoryType));
            _methodName = methodName;
            _inspectMethod = inspectMethod ?? throw new ArgumentNullException(nameof(inspectMethod));
        }

        #endregion Constructors

        #region Methods

        public Expression<Func<object, IDependencyContext, object>> GetExpression()
        {
            var factoryObjectExpression = Expression.Parameter(typeof(object));
            var contextExpression = Expression.Parameter(typeof(IDependencyContext));

            var call = Expression.Call(Expression.Constant(this), nameof(Resolve), null, factoryObjectExpression, contextExpression);
            return Expression.Lambda<Func<object, IDependencyContext, object>>(call, factoryObjectExpression, contextExpression);
        }

        public object Resolve(object factory, IDependencyContext context)
        {
            var args = context.GetArguments();
            return ResolveMethod(factory, args);
        }

        private object ResolveMethod(object factory, object[] args)
        {
            var method = _inspectMethod.FindMethodForArguments(_factoryType, _methodName, args);
            return method.Invoke(factory, args);
        }

        #endregion Methods
    }
}