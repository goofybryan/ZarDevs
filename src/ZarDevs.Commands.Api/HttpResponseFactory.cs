using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Api
{
    public class HttpResponseFactory : IHttpResponseFactory
    {
        [Obsolete("Legacy")]
        public static IHttpResponseFactory Instance => Ioc.Resolve<IHttpResponseFactory>();

        #region Methods

        public TResponse Create<TResponse>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage) where TResponse : ApiCommandResponse
        {
            var response = Runtime.Create.Instance.New<TResponse>(httpResponseMessage);
            response.RequestId = originalRequest?.Id;
            return response;
        }

        public ApiCommandResponse CreateDefault(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return Create<ApiCommandResponse>(originalRequest, httpResponseMessage);
        }

        public Task<TResponse> CreateWithContent<TResponse, TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage) where TResponse : ApiCommandContentResponse<TContent>
        {
            return CreateWithContent<TResponse, TContent>(originalRequest, httpResponseMessage, content => content.ReadAsJsonAsync<TContent>());
        }

        public async Task<TResponse> CreateWithContent<TResponse, TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage, Func<HttpContent, Task<TContent>> GetContentAsync) where TResponse : ApiCommandContentResponse<TContent>
        {
            var response = Create<TResponse>(originalRequest, httpResponseMessage);
            if (response.IsSuccess)
            {
                response.Content = await GetContentAsync(httpResponseMessage.Content);
            }
            return response;
        }

        public async Task<ApiCommandResponse> CreateWithContent<TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage, Func<HttpContent, Task<TContent>> GetContentAsync)
        {
            return await CreateWithContent<ApiCommandContentResponse<TContent>, TContent>(originalRequest, httpResponseMessage, GetContentAsync);
        }

        public async Task<ApiCommandJsonResponse> CreateWithJsonContent(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return await CreateWithContent<ApiCommandJsonResponse, string>(originalRequest, httpResponseMessage, (content) => content.ReadAsStringAsync());
        }

        #endregion Methods
    }
}