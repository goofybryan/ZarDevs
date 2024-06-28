using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Command for posting, this will call the <see cref="IApiHttpClient.PostAsync(Uri, HttpContent, CancellationToken)"/>
    /// </summary>
    public class ApiPostCommandAsync : ApiContentCommandAsync
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="ApiPatchCommandAsync"/>
        /// </summary>
        /// <param name="httpClient">The <see cref="IApiHttpClient"/> client used for sending the delete request.</param>
        /// <param name="serializer">The serializer that is used to serialize the <see cref="IApiCommandRequest.Content"/> to <see cref="HttpContent"/>.</param>
        /// <param name="responseFactory">The <see cref="IHttpResponseFactory"/> factory used for creating the response.</param>
        public ApiPostCommandAsync(IApiHttpClient httpClient, IApiCommandContentSerializer serializer, IHttpResponseFactory responseFactory) : base(httpClient, serializer, responseFactory)
        {
        }

        #endregion Constructors

        #region Methods


        /// <summary>
        /// Call the specific api call to the specified <paramref name="apiUri"/> with the <paramref name="content"/> content.
        /// </summary>
        /// <param name="apiUri">The api <see cref="Uri"/>.</param>
        /// <param name="content">The <see cref="HttpContent"/> to call.</param>
        /// <param name="cancellation">Cancelation token</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the <see cref="IApiHttpClient"/> call.</returns>
        protected override Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content, CancellationToken cancellation) => HttpClient.PostAsync(apiUri, content, cancellation);

        #endregion Methods
    }
}