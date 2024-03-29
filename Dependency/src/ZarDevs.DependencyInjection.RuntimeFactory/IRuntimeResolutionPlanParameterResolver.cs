﻿using System.Linq.Expressions;

namespace ZarDevs.DependencyInjection
{
    internal interface IRuntimeResolutionPlanParameterResolver
    {
        #region Methods

        Expression GetExpression();

        object Resolve();

        #endregion Methods
    }
}