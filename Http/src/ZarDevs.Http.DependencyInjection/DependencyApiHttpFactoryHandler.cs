using ZarDevs.DependencyInjection;
using ZarDevs.Http.Client;

namespace ZarDevs.Http
{
    internal class DependencyApiHttpFactoryHandler : IApiHttpHandlerFactory
    {
        #region Properties

        private static IIocContainer Ioc => DependencyInjection.Ioc.Container;

        #endregion Properties

        #region Methods

        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : class, IApiHttpRequestHandler
        {
            return Ioc.Resolve<THandler>();
        }

        #endregion Methods
    }
}