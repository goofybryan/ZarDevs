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

        #endregion Methods
    }
}