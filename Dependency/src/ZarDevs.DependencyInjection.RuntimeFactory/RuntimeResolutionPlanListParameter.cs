using System;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeResolutionPlanListParameter : IRuntimeResolutionPlanParameterResolver
    {
        #region Fields

        private readonly IIocContainer _ioc;
        private readonly Type _resolveType;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanListParameter(Type resolveType, IIocContainer ioc)
        {
            _resolveType = resolveType ?? throw new ArgumentNullException(nameof(resolveType));
            _ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
        }

        #endregion Constructors

        #region Methods

        public object Resolve() => _ioc.ResolveAll(_resolveType);

        #endregion Methods
    }
}