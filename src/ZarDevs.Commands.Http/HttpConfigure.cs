using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Http
{
    public static class HttpConfigure
    {
        #region Methods

        public static IDependencyBuilder ConfigureHttp(this IDependencyBuilder builder)
        {
            builder.Bind<IApiHttpFactory>().To((ctx, name) => ApiHttpFactory.Instance).InSingletonScope();

            return builder;
        }

        #endregion Methods
    }
}