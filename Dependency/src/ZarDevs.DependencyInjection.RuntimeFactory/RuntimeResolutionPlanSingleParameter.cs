using System;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeResolutionPlanSingleParameter : IRuntimeResolutionPlanParameterResolver
    {
        #region Fields

        private readonly IDependencyResolution _resolution;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanSingleParameter(IDependencyResolution resolution)
        {
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
        }

        #endregion Constructors

        #region Methods

        public object Resolve() => _resolution.Resolve();

        #endregion Methods
    }
}