using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Http api client class that is used to wrap
    /// </summary>
    public interface IApiHttpClient
    {
        #region Methods

        /// <summary>
        /// Create a request message for the <paramref name="method"/>
        /// </summary>
        /// <param name="method">The <see cref="HttpMethod"/> the request is for.</param>
        /// <param name="apiUri">The api url.</param>
        /// <param name="httpContent">Optional http content.</param>
        /// <returns></returns>
        HttpRequestMessage CreateRequest(HttpMethod method, Uri apiUri, HttpContent httpContent = null);

        /// <summary>
        /// Request a delete from the api.
        /// </summary>
        /// <param name="apiUrl">The api uri</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> DeleteAsync(string apiUrl, CancellationToken cancellation = default);

        /// <summary>
        /// Request a delete from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> DeleteAsync(Uri apiUri, CancellationToken cancellation = default);

        /// <summary>
        /// Request a get from the api.
        /// </summary>
        /// <param name="apiUrl">The api uri</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> GetAsync(string apiUrl, CancellationToken cancellation = default);

        /// <summary>
        /// Request a get from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> GetAsync(Uri apiUri, CancellationToken cancellation = default);

        /// <summary>
        /// Request a patch from the api.
        /// </summary>
        /// <param name="apiUrl">The api uri</param>
        /// <param name="httpContent">The content to patch.</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PatchAsync(string apiUrl, HttpContent httpContent, CancellationToken cancellation = default);

        /// <summary>
        /// Request a patch from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <param name="httpContent">The content to patch.</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PatchAsync(Uri apiUri, HttpContent httpContent, CancellationToken cancellation = default);

        /// <summary>
        /// Request a post from the api.
        /// </summary>
        /// <param name="apiUrl">The api uri</param>
        /// <param name="httpContent">The content to post.</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PostAsync(string apiUrl, HttpContent httpContent, CancellationToken cancellation = default);

        /// <summary>
        /// Request a post from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <param name="httpContent">The content to post.</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PostAsync(Uri apiUri, HttpContent httpContent, CancellationToken cancellation = default);

        /// <summary>
        /// Request a put from the api.
        /// </summary>
        /// <param name="apiUrl">The api url</param>
        /// <param name="httpContent">The content to put.</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PutAsync(string apiUrl, HttpContent httpContent, CancellationToken cancellation = default);

        /// <summary>
        /// Request a put from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <param name="httpContent">The content to put.</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PutAsync(Uri apiUri, HttpContent httpContent, CancellationToken cancellation = default);

        /// <summary>
        /// Send a request message to the api.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="cancellation">Optional cancellation token</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellation = default);

        #endregion Methods
    }
}