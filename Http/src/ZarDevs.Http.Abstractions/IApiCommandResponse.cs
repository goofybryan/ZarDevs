using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Interface defining the API command response. This is returned after an api call.
    /// </summary>
    public interface IApiCommandResponse
    {
        #region Properties

        /// <summary>
        /// Get an indicator that the response returned a success status code.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Get the reason given by the server for the failure.
        /// </summary>
        string Reason { get; }

        /// <summary>
        /// Get the http response message.
        /// </summary>
        HttpResponseMessage Response { get; }

        /// <summary>
        /// Get the http resposne status code.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Ensure that the response returned a success code. Throws this exception when <see cref="HttpResponseMessage.IsSuccessStatusCode"/> returns false. The <see cref="HttpResponseMessage.ReasonPhrase"/> will be set as the exception message.
        /// </summary>
        /// <exception cref="ApiCommandException">Throws this exception when <see cref="HttpResponseMessage"/>.<see cref="HttpResponseMessage.IsSuccessStatusCode"/> returns false.</exception>
        void EnsureSuccess();

        /// <summary>
        /// Try and deserialize the <see cref="Response"/> content (<see cref="HttpResponseMessage.Content"/>) if any to the requested type of <typeparamref name="TContent"/>. Returns a value if there is content deserialize, otherwise default(<typeparamref name="TContent"/>).
        /// </summary>
        /// <param name="cancellation">Optional cancellation token.</param>
        /// <typeparam name="TContent">The expected type of content.</typeparam>
        /// <exception cref="ApiCommandException">Calls <see cref="EnsureSuccess"/> before trying to get content.</exception>
        Task<TContent> TryGetContentAsync<TContent>(CancellationToken cancellation = default);

        #endregion Methods
    }
}