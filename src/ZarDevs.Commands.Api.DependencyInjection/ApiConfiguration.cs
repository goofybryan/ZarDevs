using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Api
{
    public static class ApiConfiguration
    {
        #region Methods

        public static IDependencyBuilder ConfigureApi(this IDependencyBuilder builder)
        {
            builder.Bind<IHttpResponseFactory>().To<HttpResponseFactory>().InSingletonScope();

            builder.Bind<ICommandAsync<ApiCommandRequest, ApiCommandJsonResponse>>().To<ApiGetCommandAsync>().WithKey(HttpRequestType.Get);
            builder.Bind<ICommandAsync<ApiCommandRequest, ApiCommandResponse>>().To<ApiDeleteCommandAsync>().WithKey(HttpRequestType.Delete);
            builder.Bind<ICommandAsync<ApiCommandRequest, ApiCommandResponse>>().To<ApiPutCommandAsync>().WithKey(HttpRequestType.Put);
            builder.Bind<ICommandAsync<ApiCommandRequest, ApiCommandResponse>>().To<ApiPostCommandAsync>().WithKey(HttpRequestType.Post);
            builder.Bind<IApiGetCommandAsync>().To<ApiGetCommandAsync>().WithKey(HttpRequestType.Delete);
            builder.Bind<IApiPutCommandAsync>().To<ApiPutCommandAsync>().WithKey(HttpRequestType.Put);
            builder.Bind<IApiPostCommandAsync>().To<ApiPostCommandAsync>().WithKey(HttpRequestType.Post);
            //builder.Bind<IHttpMessageBuilder>().To<HttpMessageBuilder>();

            return builder;
        }

        #endregion Methods
    }
}