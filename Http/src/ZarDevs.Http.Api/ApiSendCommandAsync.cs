using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// This command is used when a non-standard call allowing for some customization to the API call, this will call the <see cref="IApiHttpClient.SendAsync(HttpRequestMessage, CancellationToken)"/>
    /// </summary>
    public class ApiSendCommandAsync : ApiContentCommandAsync
    {
        private readonly HttpMethod _httpMethod;

        /// <summary>
        /// Create a new instance of the <see cref="ApiSendCommandAsync"/>
        /// </summary>
        /// <param name="httpClient">The <see cref="IApiHttpClient"/> client used for sending the delete request.</param>
        /// <param name="serializer">The serializer that is used to serialize the <see cref="IApiCommandRequest.Content"/> to <see cref="HttpContent"/>.</param>
        /// <param name="responseFactory">The <see cref="IHttpResponseFactory"/> factory used for creating the response.</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/> that will be used for the API call.</param>
        public ApiSendCommandAsync(IApiHttpClient httpClient, IApiCommandContentSerializer serializer, IHttpResponseFactory responseFactory, HttpMethod httpMethod) : base(httpClient, serializer, responseFactory)
        {
            _httpMethod = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));
        }


        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await HttpClient.SendAsync(HttpClient.CreateRequest(_httpMethod, apiUri, content), cancellationToken).ConfigureAwait(false);
        }
    }
}