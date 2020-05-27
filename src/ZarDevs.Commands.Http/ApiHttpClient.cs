using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Commands.Http
{
    internal class ApiHttpClient : IApiHttpClient
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly IApiHttpRequestHandler _requestHandler;

        private bool disposedValue = false;

        #endregion Fields

        #region Constructors

        public ApiHttpClient(IApiHttpRequestHandler requestHandler, HttpClient httpClient)
        {
            _requestHandler = requestHandler ?? throw new ArgumentNullException(nameof(requestHandler));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #endregion Constructors

        #region Methods

        public async Task<HttpResponseMessage> DeleteAsync(Uri apiUri)
        {
            var request = CreateRequest(HttpMethod.Delete, apiUri);
            return await SendAsync(request);
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

        public async Task<HttpResponseMessage> PostAsync(Uri apiUri, HttpContent httpContent)
        {
            var request = CreateRequest(HttpMethod.Post, apiUri, httpContent);
            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync(Uri apiUri, HttpContent httpContent)
        {
            var request = CreateRequest(HttpMethod.Put, apiUri, httpContent);
            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            await _requestHandler.HandleAsync(request);
            return await _httpClient.SendAsync(request);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }
                disposedValue = true;
            }
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, Uri apiUri, HttpContent httpContent = null)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, apiUri)
            {
                Content = httpContent
            };

            return message;
        }

        #endregion Methods

        // To detect redundant calls
    }
}