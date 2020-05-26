using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiDeleteCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>, IApiDeleteCommandAsync
    {
        public ApiDeleteCommandAsync(IApiHttpClient httpClient) : base(httpClient)
        {
        }

        #region Methods

        protected override Task<ApiCommandResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return Task.FromResult(HttpResponseFactory.CreateDefault(originalRequest, httpResponseMessage));
        }

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandRequest request)
        {
            return await HttpClient.DeleteAsync(request.ApiUri);
        }

        #endregion Methods
    }
}