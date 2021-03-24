using System;
using System.Linq.Expressions;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency resolution factory plan.
    /// </summary>
    public interface IDependencyFactoryResolutionPlan
    {
        #region Methods

        /// <summary>
        /// Get an expression of the what needs to happen/
        /// </summary>
        /// <returns></returns>
        Expression<Func<object, IDependencyContext, object>> GetExpression();

        /// <summary>
        /// Resolve the factory method.
        /// </summary>
        /// <param name="factory">The factory object that needs to be resolved.</param>
        /// <param name="context">The dependency context.</param>
        /// <returns></returns>
        object Resolve(object factory, IDependencyContext context);

        #endregion Methods
    }
}