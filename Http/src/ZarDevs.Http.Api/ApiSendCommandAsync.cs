using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// This command is used when a non-standard call allowing for some customization to the API call, this will call the <see cref="IApiHttpClient.SendAsync(HttpRequestMessage)"/>
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

        /// <summary>
        /// Call the specific send api call to the specified <paramref name="apiUri"/> with the <paramref name="content"/> content.
        /// </summary>
        /// <param name="apiUri">The api <see cref="Uri"/>.</param>
        /// <param name="content">The <see cref="HttpContent"/> to call.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the <see cref="IApiHttpClient"/> call.</returns>
        protected override Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content)
        {
            return HttpClient.SendAsync(HttpClient.CreateRequest(_httpMethod, apiUri, content));
        }
    }
}