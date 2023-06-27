using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Command for putting, this will call the <see cref="IApiHttpClient.PatchAsync(Uri, HttpContent, CancellationToken)"/>
    /// </summary>
    public class ApiPutCommandAsync : ApiContentCommandAsync
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="ApiPutCommandAsync"/>
        /// </summary>
        /// <param name="httpClient">The <see cref="IApiHttpClient"/> client used for sending the delete request.</param>
        /// <param name="serializer">The serializer that is used to serialize the <see cref="IApiCommandRequest.Content"/> to <see cref="HttpContent"/>.</param>
        /// <param name="responseFactory">The <see cref="IHttpResponseFactory"/> factory used for creating the response.</param>
        public ApiPutCommandAsync(IApiHttpClient httpClient, IApiCommandContentSerializer serializer, IHttpResponseFactory responseFactory) : base(httpClient, serializer, responseFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await HttpClient.PutAsync(apiUri, content, cancellationToken).ConfigureAwait(false);
        }

        #endregion Methods
    }
}