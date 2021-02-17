﻿using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiGetCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandJsonResponse>, IApiGetCommandAsync
    {
        private readonly IHttpResponseFactory _responseFactory;
        #region Constructors

        public ApiGetCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient)
        {
            _responseFactory = responseFactory ?? throw new System.ArgumentNullException(nameof(responseFactory));
        }

        #endregion Constructors

        #region Methods

        protected override async Task<ApiCommandJsonResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return await _responseFactory.CreateWithJsonContent(originalRequest, httpResponseMessage);
        }

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandRequest request)
        {
            return await HttpClient.GetAsync(request.ApiUri);
        }

        #endregion Methods
    }
}