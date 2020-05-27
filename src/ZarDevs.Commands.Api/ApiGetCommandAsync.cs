using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiGetCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandJsonResponse>, IApiGetCommandAsync
    {
        #region Constructors

        public ApiGetCommandAsync(IApiHttpClient httpClient) : base(httpClient)
        {
        }

        #endregion Constructors

        #region Methods

        protected override async Task<ApiCommandJsonResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return await HttpResponseFactory.Instance.CreateWithJsonContent(originalRequest, httpResponseMessage);
        }

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandRequest request)
        {
            return await HttpClient.GetAsync(request.ApiUri);
        }

        #endregion Methods
    }
}