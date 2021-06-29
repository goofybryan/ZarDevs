using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    internal abstract class ApiCommandAsyncBase : IApiCommandAsync
    { 
        #region Constructors

        protected ApiCommandAsyncBase(IApiHttpClient httpClient)
        {
            HttpClient = httpClient ?? throw new System.ArgumentNullException(nameof(httpClient));
        }

        #endregion Constructors

        #region Properties

        protected IApiHttpClient HttpClient { get; }

        #endregion Properties

        #region Methods

        public async Task<IApiCommandResponse> ExecuteAsync(IApiCommandRequest request)
        {
            return await OnExecuteAsync(request);
        }

        protected abstract IApiCommandResponse CreateResponse(HttpResponseMessage httpResponseMessage);

        protected abstract Task<HttpResponseMessage> OnApiCall(IApiCommandRequest request);

        protected virtual async Task<IApiCommandResponse> OnExecuteAsync(IApiCommandRequest request)
        {
            var responseMessage = await OnApiCall(request);
            return CreateResponse(responseMessage);
        }

        #endregion Methods
    }
}