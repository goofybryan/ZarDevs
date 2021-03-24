using System;
using System.Linq.Expressions;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeResolutionPlanExpression : IRuntimeResolutionPlan
    {
        #region Fields

        private readonly Expression<Func<object>> _expression;
        private readonly Func<object> _function;
        private readonly IRuntimeResolutionPlan _originalPlan;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanExpression(IRuntimeResolutionPlan originalPlan)
        {
            _originalPlan = originalPlan ?? throw new ArgumentNullException(nameof(originalPlan));
            _expression = CreateFunction(_originalPlan.CreateExpression());
            _function = _expression.Compile();
        }

        #endregion Constructors

        #region Methods

        public Expression CreateExpression()
        {
            return _expression;
        }

        public object Resolve()
        {
            return _function();
        }

        private static Expression<Func<object>> CreateFunction(Expression expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            while (expression.CanReduce)
            {
                expression = expression.ReduceAndCheck();
            }

            return Expression.Lambda<Func<object>>(expression);
        }

        #endregion Methods
    }
}