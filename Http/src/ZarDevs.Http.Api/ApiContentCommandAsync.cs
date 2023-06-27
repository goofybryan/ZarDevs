using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api abstract command for content that implements the base construct of the API Command infrastructure.
    /// </summary>
    public abstract class ApiContentCommandAsync : ApiCommandAsync
    {
        private readonly IApiCommandContentSerializer _serializer;

        /// <summary>
        /// Protected constructor that enforces the required variables needed for this implementation.
        /// </summary>
        /// <param name="httpClient">The <see cref="IApiHttpClient"/> client used for sending the delete request.</param>
        /// <param name="serializer">The serializer that is used to serialize the <see cref="IApiCommandRequest.Content"/> to <see cref="HttpContent"/>.</param>
        /// <param name="responseFactory">The <see cref="IHttpResponseFactory"/> factory used for creating the response.</param>
        protected ApiContentCommandAsync(IApiHttpClient httpClient, IApiCommandContentSerializer serializer, IHttpResponseFactory responseFactory) : base(httpClient, responseFactory)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <summary>
        /// Call the specific api call for the specified <paramref name="request"/>
        /// </summary>
        /// <param name="request">The request message that contains the content need for the server call.</param>
        /// <param name="cancellation">Cancelation Token</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the <see cref="IApiHttpClient"/> call.</returns>
        protected override Task<HttpResponseMessage> OnApiCallAsync(IApiCommandRequest request, CancellationToken cancellation)
        {
            return OnApiCall(request.ApiUri, _serializer.Serialize(request), cancellation);
        }

        /// <summary>
        /// Call the specific api call to the specified <paramref name="apiUri"/> with the <paramref name="content"/> content.
        /// </summary>
        /// <param name="apiUri">The api <see cref="Uri"/>.</param>
        /// <param name="content">The <see cref="HttpContent"/> to call.</param>
        /// <param name="cancellation">Cancelation token</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the <see cref="IApiHttpClient"/> call.</returns>
        protected abstract Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content, CancellationToken cancellation);
    }
}