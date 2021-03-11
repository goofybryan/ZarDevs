using System;

namespace ZarDevs.DependencyInjection
{
    internal class RuntimeResolutionPlanSingleParameter : IRuntimeResolutionPlanParameterResolver
    {
        #region Fields

        private readonly IIocContainer _ioc;
        private readonly Type _resolveType;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanSingleParameter(Type resolveType, IIocContainer ioc)
        {
            _resolveType = resolveType ?? throw new ArgumentNullException(nameof(resolveType));
            _ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
        }

        #endregion Constructors

        #region Methods

        public object Resolve() => _ioc.TryResolve(_resolveType);

        #endregion Methods
    }
}