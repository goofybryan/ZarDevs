using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client
{
    internal class ApiHttpClient : IApiHttpClient
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly IApiHttpRequestHandler _requestHandler;

        #endregion Fields

        #region Constructors

        public ApiHttpClient(HttpClient httpClient, IApiHttpRequestHandler requestHandler = null)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _requestHandler = requestHandler;
        }

        #endregion Constructors

        #region Methods

        public HttpRequestMessage CreateRequest(HttpMethod method, Uri apiUri, HttpContent httpContent = null)
        {
            HttpRequestMessage message = new(method, apiUri)
            {
                Content = httpContent
            };

             return message;
        }

       public async Task<HttpResponseMessage> DeleteAsync(Uri apiUri, CancellationToken cancellation = default)
        {
            var request = CreateRequest(HttpMethod.Delete, apiUri);
             return await SendAsync(request, cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> DeleteAsync(string apiUrl, CancellationToken cancellation = default)
        {
             return await DeleteAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute), cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> GetAsync(Uri apiUri, CancellationToken cancellation = default)
        {
            var request = CreateRequest(HttpMethod.Get, apiUri);
             return await SendAsync(request, cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> GetAsync(string apiUrl, CancellationToken cancellation = default)
        {
             return await GetAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute), cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> PatchAsync(Uri apiUri, HttpContent httpContent, CancellationToken cancellation = default)
        {

            HttpMethod method = new ("PATCH");
            var request = CreateRequest(method, apiUri, httpContent);
             return await SendAsync(request, cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> PatchAsync(string apiUrl, HttpContent httpContent, CancellationToken cancellation = default)
        {
             return await PatchAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute), httpContent, cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> PostAsync(Uri apiUri, HttpContent httpContent, CancellationToken cancellation = default)
        {
            var request = CreateRequest(HttpMethod.Post, apiUri, httpContent);
             return await SendAsync(request, cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> PostAsync(string apiUrl, HttpContent httpContent, CancellationToken cancellation = default)
        {
             return await PostAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute), httpContent, cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> PutAsync(Uri apiUri, HttpContent httpContent, CancellationToken cancellation = default)
        {
            var request = CreateRequest(HttpMethod.Put, apiUri, httpContent);
             return await SendAsync(request, cancellation).ConfigureAwait(false);
        }

       public async Task<HttpResponseMessage> PutAsync(string apiUrl, HttpContent httpContent, CancellationToken cancellation = default)
        {
             return await PutAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute), httpContent, cancellation).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellation = default)
        {
            if (_requestHandler != null)
            {
                await _requestHandler.HandleAsync(request).ConfigureAwait(false);
            }

             return await _httpClient.SendAsync(request, cancellation).ConfigureAwait(false);
        }

        #endregion Methods
    }
}