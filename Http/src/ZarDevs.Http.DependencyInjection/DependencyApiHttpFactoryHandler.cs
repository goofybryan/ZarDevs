using System;
using ZarDevs.DependencyInjection;

namespace ZarDevs.Http.Client
{
    internal class DependencyApiHttpFactoryHandler : ApiHttpHandlerFactoryBase
    {
        #region Properties

        private static IIocContainer Ioc => DependencyInjection.Ioc.Container;

        #endregion Properties

        #region Methods

        public override IApiHttpRequestHandler GetHandler<THandler>()
        {
            return Ioc.Resolve<THandler>();
        }

        public override IApiHttpRequestHandler GetHandler(Type handlerType)
        {
            return (IApiHttpRequestHandler) Ioc.TryResolve(handlerType) ?? throw new InvalidOperationException($"The type '{handlerType}' cannot be resolved.");
        }

        #endregion Methods
    }
}