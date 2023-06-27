using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api abstract command that implements the base construct of the API Command infrastructure.
    /// </summary>
    public abstract class ApiCommandAsync : IApiCommandAsync
    {
        private readonly IHttpResponseFactory _responseFactory;

        #region Constructors

        /// <summary>
        /// Protected constructor that enforces the required variables needed for this implementation
        /// </summary>
        /// <param name="httpClient">The http client needed to call</param>
        /// <param name="responseFactory">The response factory that is need to generate a <see cref="IApiCommandResponse"/></param>
        protected ApiCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory)
        {
            HttpClient = httpClient ?? throw new System.ArgumentNullException(nameof(httpClient));
            _responseFactory = responseFactory ?? throw new System.ArgumentNullException(nameof(responseFactory));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The http client
        /// </summary>
        protected IApiHttpClient HttpClient { get; }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public async Task<IApiCommandResponse> ExecuteAsync(IApiCommandRequest request, CancellationToken cancellation = default)
        {
            var responseMessage = await OnApiCallAsync(request, cancellation).ConfigureAwait(false);
            return CreateResponse(responseMessage);
        }

        /// <inheritdoc/>
        protected virtual IApiCommandResponse CreateResponse(HttpResponseMessage httpResponseMessage)
        {
            return _responseFactory.CreateResponse(httpResponseMessage);
        }

        /// <inheritdoc/>
        protected abstract Task<HttpResponseMessage> OnApiCallAsync(IApiCommandRequest request, CancellationToken cancellation);

        #endregion Methods
    }
}