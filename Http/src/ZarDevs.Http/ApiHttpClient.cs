using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client
{
    internal class ApiHttpClient : IApiHttpClient
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly IApiHttpRequestHandler _requestHandler;

        private bool _isDisposed = false;

        #endregion Fields

        #region Constructors

        public ApiHttpClient(IApiHttpRequestHandler requestHandler, HttpClient httpClient)
        {
            _requestHandler = requestHandler;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #endregion Constructors

        #region Methods

        public async Task<HttpResponseMessage> DeleteAsync(Uri apiUri)
        {
            var request = CreateRequest(HttpMethod.Delete, apiUri);
            return await SendAsync(request);
        }

        public Task<HttpResponseMessage> DeleteAsync(string apiUrl)
        {
            return DeleteAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute));
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri apiUri)
        {
            var request = CreateRequest(HttpMethod.Get, apiUri);
            return await SendAsync(request);
        }

        public Task<HttpResponseMessage> GetAsync(string apiUrl)
        {
            return GetAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute));
        }

        public async Task<HttpResponseMessage> PatchAsync(Uri apiUri, HttpContent httpContent)
        {
#if NET5_0_OR_GREATER
            HttpMethod method = HttpMethod.Patch;
#else
            HttpMethod method = new("PATCH");
#endif
            var request = CreateRequest(method, apiUri, httpContent);
            return await SendAsync(request);
        }

        public Task<HttpResponseMessage> PatchAsync(string apiUrl, HttpContent httpContent)
        {
            return PatchAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute), httpContent);
        }

        public async Task<HttpResponseMessage> PostAsync(Uri apiUri, HttpContent httpContent)
        {
            var request = CreateRequest(HttpMethod.Post, apiUri, httpContent);
            return await SendAsync(request);
        }

        public Task<HttpResponseMessage> PostAsync(string apiUrl, HttpContent httpContent)
        {
            return PostAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute), httpContent);
        }

        public async Task<HttpResponseMessage> PutAsync(Uri apiUri, HttpContent httpContent)
        {
            var request = CreateRequest(HttpMethod.Put, apiUri, httpContent);
            return await SendAsync(request);
        }

        public Task<HttpResponseMessage> PutAsync(string apiUrl, HttpContent httpContent)
        {
            return PutAsync(new Uri(apiUrl, UriKind.RelativeOrAbsolute), httpContent);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            await _requestHandler?.HandleAsync(request);
            return await _httpClient.SendAsync(request);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }
                _isDisposed = true;
            }
        }

        private static HttpRequestMessage CreateRequest(HttpMethod method, Uri apiUri, HttpContent httpContent = null)
        {
            HttpRequestMessage message = new(method, apiUri)
            {
                Content = httpContent
            };

            return message;
        }

        #endregion Methods
    }
}