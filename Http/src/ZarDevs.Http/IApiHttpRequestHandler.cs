using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Api request handler interface that is execute before an <see cref="HttpClient.SendAsync(HttpRequestMessage)"/> call. This can be used to manipulate the <see cref="HttpRequestMessage"/>.
    /// </summary>
    public interface IApiHttpRequestHandler
    {
        #region Methods

        /// <summary>
        /// Append a handler to this handler. You can add multiple and they will be executed after the current handler has been executed.
        /// </summary>
        /// <param name="handler">The handler to append.</param>
        void AppendHandler(IApiHttpRequestHandler handler);

        /// <summary>
        /// Handle the current <paramref name="request"/>.
        /// </summary>
        /// <param name="request">The request message</param>
        /// <returns>Returns a <see cref="Task"/></returns>
        Task HandleAsync(HttpRequestMessage request);

        /// <summary>
        /// Set the next handler that will be execute after all other handlers have been executed.
        /// </summary>
        /// <param name="handler">The handler that will be execute next.</param>
        void SetNextHandler(IApiHttpRequestHandler handler);

        #endregion Methods
    }
}