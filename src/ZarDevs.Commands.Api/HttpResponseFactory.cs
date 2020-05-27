using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Commands.Api
{
    public static class HttpResponseFactory
    {
        #region Methods

        public static TResponse Create<TResponse>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage) where TResponse : ApiCommandResponse
        {
            var response = Core.Runtime.Create.Instance.New<TResponse>(httpResponseMessage);
            response.RequestId = originalRequest?.Id;
            return response;
        }

        public static ApiCommandResponse CreateDefault(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return Create<ApiCommandResponse>(originalRequest, httpResponseMessage);
        }

        public static Task<TResponse> CreateWithContent<TResponse, TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage) where TResponse : ApiCommandContentResponse<TContent>
        {
            return CreateWithContent<TResponse, TContent>(originalRequest, httpResponseMessage, content => content.ReadAsJsonAsync<TContent>());
        }

        public static async Task<TResponse> CreateWithContent<TResponse, TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage, Func<HttpContent, Task<TContent>> GetContentAsync) where TResponse : ApiCommandContentResponse<TContent>
        {
            var response = Create<TResponse>(originalRequest, httpResponseMessage);
            if (response.IsSuccess)
            {
                response.Content = await GetContentAsync(httpResponseMessage.Content);
            }
            return response;
        }

        public static async Task<ApiCommandResponse> CreateWithContent<TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage, Func<HttpContent, Task<TContent>> GetContentAsync)
        {
            return await CreateWithContent<ApiCommandContentResponse<TContent>, TContent>(originalRequest, httpResponseMessage, GetContentAsync);
        }

        public static async Task<ApiCommandJsonResponse> CreateWithJsonContent(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return await CreateWithContent<ApiCommandJsonResponse, string>(originalRequest, httpResponseMessage, (content) => content.ReadAsStringAsync());
        }

        public static TValue GetResponseAs<TValue>(this ApiCommandResponse response)
        {
            var value = default(TValue);

            if (response is ApiCommandJsonResponse jsonResponse)
            {
                value = jsonResponse.GetResponseAs<TValue>();
            }

            return value;
        }

        #endregion Methods
    }
}