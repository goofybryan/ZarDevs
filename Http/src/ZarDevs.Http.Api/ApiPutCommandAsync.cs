using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    public class ApiPutCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>, IApiPutCommandAsync
    {
        #region Fields

        private readonly IHttpResponseFactory _responseFactory;

        #endregion Fields

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
            if (request is not ApiCommandContentRequest contentRequest)
            {
                throw new NotSupportedException("Api command request that does not inherit from ApiCommandContentRequest is not supported.");
            }

            return await HttpClient.PutAsync(request.ApiUri, contentRequest.Content.WriteAsJson());
        }

        #endregion Methods
    }
}