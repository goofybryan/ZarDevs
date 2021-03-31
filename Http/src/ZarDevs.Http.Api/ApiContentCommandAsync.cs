using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    public abstract class ApiContentCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>
    {
        private readonly IHttpResponseFactory _responseFactory;

        protected ApiContentCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient)
        {
            _responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
        }

        protected override async Task<ApiCommandResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            ApiCommandResponse response = await _responseFactory.CreateWithContent(originalRequest, httpResponseMessage);
            return response;
        }

        protected override Task<HttpResponseMessage> OnApiCall(ApiCommandRequest request)
        {
            if (request is not ApiCommandContentRequest contentRequest)
            {
                throw new NotSupportedException("Api command request that does not inherit from ApiCommandContentRequest is not supported.");
            }

            return OnApiCall(contentRequest);
        }

        protected abstract Task<HttpResponseMessage> OnApiCall(ApiCommandContentRequest contentRequest);
    }
}