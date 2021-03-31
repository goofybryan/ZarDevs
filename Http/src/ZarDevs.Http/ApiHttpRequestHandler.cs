using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Api request handler interface that is execute before an <see
    /// cref="HttpClient.SendAsync(HttpRequestMessage)"/> call. This can be used to manipulate the
    /// <see cref="HttpRequestMessage"/>.
    /// </summary>
    public abstract class ApiHttpRequestHandler : IApiHttpRequestHandler
    {
        #region Fields

        private readonly List<IApiHttpRequestHandler> _handlers;
        private IApiHttpRequestHandler _next;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance
        /// </summary>
        protected ApiHttpRequestHandler()
        {
            _handlers = new List<IApiHttpRequestHandler>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Append a handler to this handler. You can add multiple and they will be executed after
        /// the current handler has been executed.
        /// </summary>
        /// <param name="handler">The handler to append.</param>
        public void AppendHandler(IApiHttpRequestHandler handler)
        {
            _handlers.Add(handler);
        }

        /// <summary>
        /// Handle the current <paramref name="request"/>.
        /// </summary>
        /// <param name="request">The request message</param>
        /// <returns>Returns a <see cref="Task"/></returns>
        public async Task HandleAsync(HttpRequestMessage request)
        {
            await OnHandleRequestAsync(request);
            await OnHandleHandlersAsync(_handlers, request);
            await OnHandleNextAsync(_next, request);
        }

        /// <summary>
        /// Set the next handler that will be execute after all other handlers have been executed.
        /// </summary>
        /// <param name="handler">The handler that will be execute next.</param>
        public void SetNextHandler(IApiHttpRequestHandler handler)
        {
            _next = handler;
        }

        /// <summary>
        /// Handle the list of handlers, these will be executed concurrently and awaited until all have completed. Override for custom behaviour.
        /// </summary>
        /// <param name="handlers">The list of handlers to execute.</param>
        /// <param name="request">The request message.</param>
        protected virtual async Task OnHandleHandlersAsync(IList<IApiHttpRequestHandler> handlers, HttpRequestMessage request)
        {
            if (_handlers.Count == 0)
                return;

            Task[] tasks = handlers.Select(handler => handler.HandleAsync(request)).ToArray();

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Handle the next handler if it has been specified. Override for custom behaviour.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual async Task OnHandleNextAsync(IApiHttpRequestHandler next, HttpRequestMessage request)
        {
            if (_next != null)
            {
                await _next.HandleAsync(request);
            }
        }

        /// <summary>
        /// Handle the request message.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <returns></returns>
        protected abstract Task OnHandleRequestAsync(HttpRequestMessage request);

        #endregion Methods
    }
}