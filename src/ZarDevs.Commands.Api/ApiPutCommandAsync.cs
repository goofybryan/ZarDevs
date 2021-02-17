using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiPutCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>, IApiPutCommandAsync
    {
        private readonly IHttpResponseFactory _responseFactory;

        #region Constructors

        public ApiPutCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory factory) : base(httpClient)
        {
            _responseFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        #endregion Constructors

        #region Methods

        protected override async Task<ApiCommandResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            var response = await _responseFactory.CreateWithJsonContent(originalRequest, httpResponseMessage);
            return response;
        }

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandRequest request)
        {
            if (!(request is ApiCommandContentRequest contentRequest))
            {
                throw new NotSupportedException("Api command request that does not inherit from ApiCommandContentRequest is not supported.");
            }

            return await HttpClient.PutAsync(request.ApiUri, contentRequest.Content.WriteAsJson());
        }

        #endregion Methods
    }
}