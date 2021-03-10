using ZarDevs.DependencyInjection;

namespace ZarDevs.Http
{
    internal class DependencyApiHttpFactoryHandler : IApiHttpHandlerFactory
    {
        #region Constructors

        public DependencyApiHttpFactoryHandler(IIocContainer ioc)
        {
            Ioc = ioc ?? throw new System.ArgumentNullException(nameof(ioc));
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Ioc { get; }

        #endregion Properties

        #region Methods

        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : class, IApiHttpRequestHandler
        {
            return Ioc.Resolve<THandler>();
        }

        #endregion Methods
    }
}