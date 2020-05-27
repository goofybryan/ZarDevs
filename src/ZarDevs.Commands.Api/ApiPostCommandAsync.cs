using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiPostCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>, IApiPostCommandAsync
    {
        #region Constructors

        public ApiPostCommandAsync(IApiHttpClient httpClient) : base(httpClient)
        {
        }

        #endregion Constructors

        #region Methods

        protected override async Task<ApiCommandResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            ApiCommandResponse response = await HttpResponseFactory.Instance.CreateWithJsonContent(originalRequest, httpResponseMessage);
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