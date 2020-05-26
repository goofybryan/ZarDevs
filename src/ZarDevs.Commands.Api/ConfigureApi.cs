using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public static class ConfigureApi
    {
        #region Methods

        public static IApiHttpRequestHandlerBinding ConfigureDeleteApi<THandler>() where THandler : IApiHttpRequestHandler
        {
            return ApiHttpFactory.AddRequestHandler<ApiDeleteCommandAsync, THandler>().Named(HttpRequestType.Delete.GetBindingName());
        }

        public static IApiHttpRequestHandlerBinding ConfigureGetApi<THandler>() where THandler : IApiHttpRequestHandler
        {
            return ApiHttpFactory.AddRequestHandler<ApiGetCommandAsync, THandler>().Named(HttpRequestType.Get.GetBindingName());
        }

        public static IApiHttpRequestHandlerBinding ConfigurePostApi<THandler>() where THandler : IApiHttpRequestHandler
        {
            return ApiHttpFactory.AddRequestHandler<ApiPostCommandAsync, THandler>().Named(HttpRequestType.Post.GetBindingName());
        }

        public static IApiHttpRequestHandlerBinding ConfigurePutApi<THandler>() where THandler : IApiHttpRequestHandler
        {
            return ApiHttpFactory.AddRequestHandler<ApiPutCommandAsync, THandler>().Named(HttpRequestType.Put.GetBindingName());
        }

        #endregion Methods
    }
}