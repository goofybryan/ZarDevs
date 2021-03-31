using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    public interface IHttpResponseFactory
    {
        #region Methods

        TResponse Create<TResponse>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage) where TResponse : ApiCommandResponse;

        ApiCommandResponse CreateDefault(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage);

        Task<ApiCommandResponse> CreateWithContent<TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage, Func<HttpContent, Task<TContent>> GetContentAsync);

        Task<TResponse> CreateWithContent<TResponse, TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage) where TResponse : ApiCommandContentResponse<TContent>;

        Task<TResponse> CreateWithContent<TResponse, TContent>(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage, Func<HttpContent, Task<TContent>> GetContentAsync) where TResponse : ApiCommandContentResponse<TContent>;

        Task<ApiCommandJsonResponse> CreateWithContent(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage);

        #endregion Methods
    }
}