using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Http
{
    internal class DependencyApiHttpFactoryHandler : IApiHttpHandlerFactory
    {
        public DependencyApiHttpFactoryHandler(IIocContainer ioc)
        {
            Ioc = ioc ?? throw new System.ArgumentNullException(nameof(ioc));
        }

        public IIocContainer Ioc { get; }

        #region Methods

        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler
        {
            return Ioc.Resolve<THandler>();
        }

        #endregion Methods
    }
}