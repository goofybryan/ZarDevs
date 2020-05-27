using System;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public static class ConfigureApi
    {
        #region Methods

        [Obsolete("Need to refactor to ioc binding.")]
        public static IApiHttpRequestHandlerBinding ConfigureDeleteApi<THandler>() where THandler : IApiHttpRequestHandler
        {
            return ApiHttpFactory.Instance.AddRequestHandler<ApiDeleteCommandAsync, THandler>().Named(HttpRequestType.Delete.ToString());
        }

        [Obsolete("Need to refactor to ioc binding.")]
        public static IApiHttpRequestHandlerBinding ConfigureGetApi<THandler>() where THandler : IApiHttpRequestHandler
        {
            return ApiHttpFactory.Instance.AddRequestHandler<ApiGetCommandAsync, THandler>().Named(HttpRequestType.Get.ToString());
        }

        [Obsolete("Need to refactor to ioc binding.")]
        public static IApiHttpRequestHandlerBinding ConfigurePostApi<THandler>() where THandler : IApiHttpRequestHandler
        {
            return ApiHttpFactory.Instance.AddRequestHandler<ApiPostCommandAsync, THandler>().Named(HttpRequestType.Post.ToString());
        }

        [Obsolete("Need to refactor to ioc binding.")]
        public static IApiHttpRequestHandlerBinding ConfigurePutApi<THandler>() where THandler : IApiHttpRequestHandler
        {
            return ApiHttpFactory.Instance.AddRequestHandler<ApiPutCommandAsync, THandler>().Named(HttpRequestType.Put.ToString());
        }

        #endregion Methods
    }
}