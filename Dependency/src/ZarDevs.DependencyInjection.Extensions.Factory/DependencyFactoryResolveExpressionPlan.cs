using System;
using System.Linq.Expressions;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyFactoryResolveExpressionPlan : IDependencyFactoryResolutionPlan
    {
        #region Fields

        private readonly Expression<Func<object, IDependencyContext, object>> _expression;
        private readonly Func<object, IDependencyContext, object> _func;

        #endregion Fields

        #region Constructors

        public DependencyFactoryResolveExpressionPlan(Expression<Func<object, IDependencyContext, object>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _func = Compile(expression);
        }

        #endregion Constructors

        #region Methods

        public Expression<Func<object, IDependencyContext, object>> GetExpression()
        {
            return _expression;
        }

        public object Resolve(object factory, IDependencyContext context)
        {
            return _func(factory, context);
        }

        private static Func<object, IDependencyContext, object> Compile(Expression<Func<object, IDependencyContext, object>> expression)
        {
            while (expression.CanReduce)
            {
                expression = (Expression<Func<object, IDependencyContext, object>>)expression.ReduceAndCheck();
            }

            return expression.Compile();
        }

        #endregion Methods
    }
}