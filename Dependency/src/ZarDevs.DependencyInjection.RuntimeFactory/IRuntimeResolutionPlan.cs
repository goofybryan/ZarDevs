using System.Linq.Expressions;

namespace ZarDevs.DependencyInjection
{
    internal interface IRuntimeResolutionPlan
    {
        #region Methods

        Expression CreateExpression();

        object Resolve();

        #endregion Methods
    }
}