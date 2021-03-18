using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeResolutionPlanListParameter : IRuntimeResolutionPlanParameterResolver
    {
        #region Fields

        private readonly IDependencyResolutions _resolutions;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanListParameter(IDependencyResolutions resolutions)
        {
            _resolutions = resolutions ?? throw new ArgumentNullException(nameof(resolutions));
        }

        #endregion Constructors

        #region Methods

        public object Resolve() => _resolutions.Resolve();

        #endregion Methods
    }
}