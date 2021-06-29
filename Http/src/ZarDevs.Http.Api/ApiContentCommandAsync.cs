using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    internal abstract class ApiContentCommandAsync : ApiCommandAsyncBase
    {
        private readonly IApiCommandContentSerializer _serializer;
        private readonly IHttpResponseFactory _responseFactory;

        protected ApiContentCommandAsync(IApiHttpClient httpClient, IApiCommandContentSerializer serializer, IHttpResponseFactory responseFactory) : base(httpClient)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
        }

        protected override  IApiCommandResponse CreateResponse(HttpResponseMessage httpResponseMessage)
        {
            IApiCommandResponse response = _responseFactory.CreateResponse(httpResponseMessage);
            return response;
        }

        protected override Task<HttpResponseMessage> OnApiCall(IApiCommandRequest request)
        {
            return OnApiCall(request.ApiUri, _serializer.Serialize(request));
        }

        protected abstract Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content);
    }
}