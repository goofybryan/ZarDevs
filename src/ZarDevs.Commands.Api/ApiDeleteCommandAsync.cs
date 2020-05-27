using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiDeleteCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>, IApiDeleteCommandAsync
    {
        #region Constructors

        public ApiDeleteCommandAsync(IApiHttpClient httpClient) : base(httpClient)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Task<ApiCommandResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return Task.FromResult(HttpResponseFactory.Instance.CreateDefault(originalRequest, httpResponseMessage));
        }

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandRequest request)
        {
            return await HttpClient.DeleteAsync(request.ApiUri);
        }

        #endregion Methods
    }
}