using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Api
{
    public static class ApiConfiguration
    {
        #region Methods

        public static IDependencyBuilder ConfigureApi(this IDependencyBuilder builder)
        {
            builder.Bind<IHttpResponseFactory>().To<HttpResponseFactory>().InSingletonScope();

            builder.Bind<ICommandAsync<ApiCommandRequest, ApiCommandJsonResponse>>().To<ApiGetCommandAsync>().Named(HttpRequestType.Get);
            builder.Bind<ICommandAsync<ApiCommandRequest, ApiCommandResponse>>().To<ApiDeleteCommandAsync>().Named(HttpRequestType.Delete);
            builder.Bind<ICommandAsync<ApiCommandRequest, ApiCommandResponse>>().To<ApiPutCommandAsync>().Named(HttpRequestType.Put);
            builder.Bind<ICommandAsync<ApiCommandRequest, ApiCommandResponse>>().To<ApiPostCommandAsync>().Named(HttpRequestType.Post);
            builder.Bind<IApiGetCommandAsync>().To<ApiGetCommandAsync>().Named(HttpRequestType.Delete);
            builder.Bind<IApiPutCommandAsync>().To<ApiPutCommandAsync>().Named(HttpRequestType.Put);
            builder.Bind<IApiPostCommandAsync>().To<ApiPostCommandAsync>().Named(HttpRequestType.Post);
            //builder.Bind<IHttpMessageBuilder>().To<HttpMessageBuilder>();

            return builder;
        }

        #endregion Methods
    }
}