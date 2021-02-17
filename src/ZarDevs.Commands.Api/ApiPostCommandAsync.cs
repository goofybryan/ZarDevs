using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiPostCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>, IApiPostCommandAsync
    {
        private readonly IHttpResponseFactory _responseFactory;
        #region Constructors

        public ApiPostCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient)
        {
            this._responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
        }

        #endregion Constructors

        #region Methods

        protected override async Task<ApiCommandResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            ApiCommandResponse response = await _responseFactory.CreateWithJsonContent(originalRequest, httpResponseMessage);
            return response;
        }

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandRequest request)
        {
            if (!(request is ApiCommandContentRequest contentRequest))
            {
                throw new NotSupportedException("Api command request that does not inherit from ApiCommandContentRequest is not supported.");
            }

            return await HttpClient.PostAsync(request.ApiUri, contentRequest.Content.WriteAsJson());
        }

        #endregion Methods
    }
}