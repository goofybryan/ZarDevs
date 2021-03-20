using System;
using System.Linq.Expressions;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeResolutionPlanSingleParameter : IRuntimeResolutionPlanParameterResolver
    {
        #region Fields

        private readonly IRuntimeResolutionPlanCreator _planCreator;
        private readonly IDependencyResolution _resolution;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanSingleParameter(IRuntimeResolutionPlanCreator planCreator, IDependencyResolution resolution)
        {
            _planCreator = planCreator ?? throw new ArgumentNullException(nameof(planCreator));
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
        }

        #endregion Constructors

        #region Methods

        public Expression GetExpression()
        {
            var _plan = _planCreator.FromResolution(_resolution);
            return _plan.CreateExpression();
        }

        public object Resolve() => _resolution.Resolve();

        #endregion Methods
    }
}