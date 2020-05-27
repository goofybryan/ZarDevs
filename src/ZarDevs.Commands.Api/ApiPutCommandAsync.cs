using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiPutCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>, IApiPutCommandAsync
    {
        #region Constructors

        public ApiPutCommandAsync(IApiHttpClient httpClient) : base(httpClient)
        {
        }

        #endregion Constructors

        #region Methods

        protected override async Task<ApiCommandResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            var response = await HttpResponseFactory.Instance.CreateWithJsonContent(originalRequest, httpResponseMessage);
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