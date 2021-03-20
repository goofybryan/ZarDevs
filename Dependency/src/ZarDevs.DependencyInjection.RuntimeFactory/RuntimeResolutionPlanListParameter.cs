using System;
using System.Linq;
using System.Linq.Expressions;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeResolutionPlanListParameter : IRuntimeResolutionPlanParameterResolver
    {
        #region Fields

        private readonly IRuntimeResolutionPlanCreator _planCreator;

        private readonly IDependencyResolutions _resolutions;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanListParameter(IRuntimeResolutionPlanCreator planCreator, IDependencyResolutions resolutions)
        {
            _planCreator = planCreator ?? throw new ArgumentNullException(nameof(planCreator));
            _resolutions = resolutions ?? throw new ArgumentNullException(nameof(resolutions));
        }

        #endregion Constructors

        #region Methods

        public Expression GetExpression()
        {
            return Expression.NewArrayInit(_resolutions.RequestType, _resolutions.Select(r => _planCreator.FromResolution(r).CreateExpression()));
        }

        public object Resolve() => _resolutions.Resolve();

        #endregion Methods
    }
}