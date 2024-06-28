using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Command for deleting, this will call the <see cref="IApiHttpClient.DeleteAsync(System.Uri, CancellationToken)"/>
    /// </summary>
    public class ApiDeleteCommandAsync : ApiCommandAsync
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="ApiDeleteCommandAsync"/>
        /// </summary>
        /// <param name="httpClient">The <see cref="IApiHttpClient"/> client used for sending the delete request.</param>
        /// <param name="responseFactory">The <see cref="IHttpResponseFactory"/> factory used for creating the response.</param>
        public ApiDeleteCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient, responseFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> OnApiCallAsync(IApiCommandRequest request, CancellationToken cancellation)
        {
            return await HttpClient.DeleteAsync(request.ApiUri, cancellation);
        }

        #endregion Methods
    }
}