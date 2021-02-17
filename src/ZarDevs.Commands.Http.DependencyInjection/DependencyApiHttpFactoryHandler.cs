using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Http
{
    internal class DependencyApiHttpFactoryHandler : IApiHttpHandlerFactory
    {
        #region Methods

        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler
        {
            return Ioc.Resolve<THandler>();
        }

        #endregion Methods
    }
}