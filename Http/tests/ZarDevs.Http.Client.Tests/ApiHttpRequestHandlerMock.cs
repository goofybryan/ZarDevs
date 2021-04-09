using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client.Tests
{
    internal class ApiHttpRequestHandlerMock : ApiHttpRequestHandler
    {
        public HttpRequestMessage RequestMessage { get; private set; }

        protected override Task OnHandleRequestAsync(HttpRequestMessage request)
        {
            RequestMessage = request;
            return Task.CompletedTask;
        }
    }
}
