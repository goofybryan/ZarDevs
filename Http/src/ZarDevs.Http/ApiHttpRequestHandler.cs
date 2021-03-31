using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client
{
    public class ApiHttpRequestHandler : IApiHttpRequestHandler
    {
        #region Fields

        private readonly List<IApiHttpRequestHandler> _handlers;
        private IApiHttpRequestHandler _innerHandler;

        #endregion Fields

        #region Constructors

        public ApiHttpRequestHandler()
        {
            _handlers = new List<IApiHttpRequestHandler>();
        }

        #endregion Constructors

        #region Methods

        public void AppendHandler(IApiHttpRequestHandler handler)
        {
            _handlers.Add(handler);
        }

        public async Task HandleAsync(HttpRequestMessage request)
        {
            await OnHandleAsync(request);
        }

        public void SetInnerHandler(IApiHttpRequestHandler handler)
        {
            _innerHandler = handler;
        }

        protected virtual async Task OnHandleAsync(HttpRequestMessage request)
        {
            if (_innerHandler != null)
            {
                await _innerHandler.HandleAsync(request);
            }

            _handlers.ForEach(handler => handler.HandleAsync(request));
        }

        #endregion Methods
    }
}