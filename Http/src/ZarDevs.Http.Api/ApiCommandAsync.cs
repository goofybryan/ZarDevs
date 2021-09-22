using System.Net.Http;
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

        /// <summary>
        /// Execute the command asyncronously. The command requires a request, that will be sent and processed and return a response.
        /// </summary>
        /// <param name="request">The request that will be sent to the server.</param>
        /// <returns>The response from the server.</returns>
        public async Task<IApiCommandResponse> ExecuteAsync(IApiCommandRequest request)
        {
            var responseMessage = await OnApiCall(request);
            return CreateResponse(responseMessage);
        }

        /// <summary>
        /// Create the <see cref="IApiCommandResponse"/> response from the <paramref name="httpResponseMessage"/>
        /// </summary>
        /// <param name="httpResponseMessage">The <see cref="HttpRequestMessage"/> from the server.</param>
        protected virtual IApiCommandResponse CreateResponse(HttpResponseMessage httpResponseMessage)
        {
            return _responseFactory.CreateResponse(httpResponseMessage);
        }

        /// <summary>
        /// Call the specific api call for the specified <paramref name="request"/>
        /// </summary>
        /// <param name="request">The request message that contains the content need for the server call.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the <see cref="IApiHttpClient"/> call.</returns>
        protected abstract Task<HttpResponseMessage> OnApiCall(IApiCommandRequest request);

        #endregion Methods
    }
}