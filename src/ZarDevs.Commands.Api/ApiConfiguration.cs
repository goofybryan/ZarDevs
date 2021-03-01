using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public static class ApiConfiguration
    {
        #region Methods

        public static IApiHttpRequestHandlerBinding ConfigureDeleteApi<THandler>(this IApiHttpFactory factory) where THandler : class, IApiHttpRequestHandler
        {
            return factory.AddRequestHandler<THandler>().Named(HttpRequestType.Delete.ToString());
        }

        public static IApiHttpRequestHandlerBinding ConfigureGetApi<THandler>(this IApiHttpFactory factory) where THandler : class, IApiHttpRequestHandler
        {
            return factory.AddRequestHandler<THandler>().Named(HttpRequestType.Get.ToString());
        }

        public static IApiHttpRequestHandlerBinding ConfigurePostApi<THandler>(this IApiHttpFactory factory) where THandler : class, IApiHttpRequestHandler
        {
            return factory.AddRequestHandler<THandler>().Named(HttpRequestType.Post.ToString());
        }

        public static IApiHttpRequestHandlerBinding ConfigurePutApi<THandler>(this IApiHttpFactory factory) where THandler : class, IApiHttpRequestHandler
        {
            return factory.AddRequestHandler<THandler>().Named(HttpRequestType.Put.ToString());
        }

        #endregion Methods
    }
}