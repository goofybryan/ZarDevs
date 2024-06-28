using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Command for getting, this will call the <see cref="IApiHttpClient.GetAsync(System.Uri, CancellationToken)"/>
    /// </summary>
    public class ApiGetCommandAsync : ApiCommandAsync
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="ApiGetCommandAsync"/>
        /// </summary>
        /// <param name="httpClient">The <see cref="IApiHttpClient"/> client used for sending the delete request.</param>
        /// <param name="responseFactory">The <see cref="IHttpResponseFactory"/> factory used for creating the response.</param>
        public ApiGetCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient, responseFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> OnApiCallAsync(IApiCommandRequest request, CancellationToken cancellationToken)
        {
            return await HttpClient.GetAsync(request.ApiUri, cancellationToken).ConfigureAwait(false);
        }

        #endregion Methods
    }
}