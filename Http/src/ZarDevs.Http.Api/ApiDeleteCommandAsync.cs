﻿using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    internal class ApiDeleteCommandAsync : ApiCommandAsyncBase
    {
        #region Fields

        private readonly IHttpResponseFactory _responseFactory;

        #endregion Fields

        #region Constructors

        public ApiDeleteCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient)
        {
            _responseFactory = responseFactory ?? throw new System.ArgumentNullException(nameof(responseFactory));
        }

        #endregion Constructors

        #region Methods

        protected override IApiCommandResponse CreateResponse(HttpResponseMessage httpResponseMessage)
        {
            return _responseFactory.CreateResponse(httpResponseMessage);
        }

        protected override async Task<HttpResponseMessage> OnApiCall(IApiCommandRequest request)
        {
            return await HttpClient.DeleteAsync(request.ApiUri);
        }

        #endregion Methods
    }
}