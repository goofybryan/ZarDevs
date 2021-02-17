using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Commands.Http;

namespace ZarDevs.Commands.Api
{
    public class ApiDeleteCommandAsync : ApiCommandAsyncBase<ApiCommandRequest, ApiCommandResponse>, IApiDeleteCommandAsync
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

        protected override Task<ApiCommandResponse> CreateResponse(ApiCommandRequest originalRequest, HttpResponseMessage httpResponseMessage)
        {
            return Task.FromResult(_responseFactory.CreateDefault(originalRequest, httpResponseMessage));
        }

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandRequest request)
        {
            return await HttpClient.DeleteAsync(request.ApiUri);
        }

        #endregion Methods
    }
}