using System;
using System.Linq.Expressions;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeResolutionPlanResolution : IRuntimeResolutionPlan
    {
        #region Fields

        private readonly IDependencyResolution _resolution;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanResolution(IDependencyResolution resolution)
        {
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
        }

        #endregion Constructors

        #region Methods

        public Expression CreateExpression()
        {
            return Expression.Constant(_resolution.Resolve());
        }

        public object Resolve()
        {
            return _resolution.Resolve();
        }

        #endregion Methods
    }
}