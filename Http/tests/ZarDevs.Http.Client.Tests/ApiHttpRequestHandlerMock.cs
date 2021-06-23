using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client.Tests
{
    internal class ApiHttpRequestHandlerMock : ApiHttpRequestHandler
    {
        #region Constructors

        public ApiHttpRequestHandlerMock() : base()
        {
        }

        #endregion Constructors

        #region Properties

        public IList<IApiHttpRequestHandler> Handlers { get; private set; }
        public IApiHttpRequestHandler Next { get; private set; }
        public HttpRequestMessage RequestMessage { get; private set; }

        #endregion Properties

        #region Methods

        protected override Task OnHandleHandlersAsync(IList<IApiHttpRequestHandler> handlers, HttpRequestMessage request)
        {
            Handlers = handlers;

            return base.OnHandleHandlersAsync(handlers, request);
        }

        protected override Task OnHandleNextAsync(IApiHttpRequestHandler next, HttpRequestMessage request)
        {
            Next = next;

            return base.OnHandleNextAsync(next, request);
        }

        protected override Task OnHandleRequestAsync(HttpRequestMessage request)
        {
            RequestMessage = request; ;
            return Task.CompletedTask;
        }

        #endregion Methods

        public class ApiHttpRequestHandlerDifferentType1Mock : ApiHttpRequestHandlerMock { }
        public class ApiHttpRequestHandlerDifferentType2Mock : ApiHttpRequestHandlerMock { }
        public class ApiHttpRequestHandlerDifferentType3Mock : ApiHttpRequestHandlerMock { }
        public class ApiHttpRequestHandlerDifferentType4Mock : ApiHttpRequestHandlerMock { }
        public class ApiHttpRequestHandlerDifferentType5Mock : ApiHttpRequestHandlerMock { }
    }
}