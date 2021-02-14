using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Api
{
    public static class ApiConfiguration
    {
        #region Methods

        public static IDependencyBuilder ConfigureApi(this IDependencyBuilder builder)
        {
            builder.Bind<IHttpResponseFactory>().To<HttpResponseFactory>().InSingletonScope();

            return builder;
        }

        #endregion Methods
    }
}