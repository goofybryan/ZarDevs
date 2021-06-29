using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    internal class ApiSendCommandAsync : ApiContentCommandAsync
    {
        private readonly HttpMethod _httpMethod;

        public ApiSendCommandAsync(IApiHttpClient httpClient, IApiCommandContentSerializer serializer, IHttpResponseFactory responseFactory, HttpMethod httpMethod) : base(httpClient, serializer, responseFactory)
        {
            _httpMethod = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));
        }

        public Task<IApiCommandResponse> ExecuteAsync(HttpContent content)
        {
            throw new NotImplementedException();
        }

        protected override Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content)
        {
            return HttpClient.SendAsync(HttpClient.CreateRequest(_httpMethod, apiUri, content));
        }
    }
}