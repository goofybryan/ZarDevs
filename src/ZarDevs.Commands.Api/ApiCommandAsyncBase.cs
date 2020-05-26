using ZarDevs.Commands.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Commands.Api
{
    public abstract class ApiCommandAsyncBase<TRequest, TResponse> : IApiCommandAsync<TRequest, TResponse> where TRequest : ApiCommandRequest where TResponse : ApiCommandResponse
    {
        #region Fields

        private bool _disposedValue = false;

        #endregion Fields

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

        public void Dispose()
        {
            Dispose(true);
        }

        public async Task<TResponse> ExecuteAsync(TRequest request)
        {
            return await OnExecuteAsync(request);
        }

        protected abstract Task<TResponse> CreateResponse(TRequest originalRequest, HttpResponseMessage httpResponseMessage);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
            }
        }

        protected abstract Task<HttpResponseMessage> OnApiCall(TRequest request);

        protected virtual async Task<TResponse> OnExecuteAsync(TRequest request)
        {
            var responseMessage = await OnApiCall(request);
            return await CreateResponse(request, responseMessage);
        }

        #endregion Methods
    }
}