using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Http api client class that is used to wrap
    /// </summary>
    public interface IApiHttpClient : IDisposable
    {
        #region Methods

        /// <summary>
        /// Request a delete from the api.
        /// </summary>
        /// <param name="apiUrl">The api uri</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> DeleteAsync(string apiUrl);

        /// <summary>
        /// Request a delete from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> DeleteAsync(Uri apiUri);

        /// <summary>
        /// Request a get from the api.
        /// </summary>
        /// <param name="apiUrl">The api uri</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> GetAsync(string apiUrl);

        /// <summary>
        /// Request a get from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> GetAsync(Uri apiUri);

        /// <summary>
        /// Request a patch from the api.
        /// </summary>
        /// <param name="apiUrl">The api uri</param>
        /// <param name="httpContent">The content to patch.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PatchAsync(string apiUrl, HttpContent httpContent);

        /// <summary>
        /// Request a patch from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <param name="httpContent">The content to patch.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PatchAsync(Uri apiUri, HttpContent httpContent);

        /// <summary>
        /// Request a post from the api.
        /// </summary>
        /// <param name="apiUrl">The api uri</param>
        /// <param name="httpContent">The content to post.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PostAsync(string apiUrl, HttpContent httpContent);

        /// <summary>
        /// Request a post from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <param name="httpContent">The content to post.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PostAsync(Uri apiUri, HttpContent httpContent);

        /// <summary>
        /// Request a put from the api.
        /// </summary>
        /// <param name="apiUrl">The api url</param>
        /// <param name="httpContent">The content to put.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PutAsync(string apiUrl, HttpContent httpContent);

        /// <summary>
        /// Request a put from the api.
        /// </summary>
        /// <param name="apiUri">The api uri</param>
        /// <param name="httpContent">The content to put.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> PutAsync(Uri apiUri, HttpContent httpContent);

        /// <summary>
        /// Send a request message to the api.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> response.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        #endregion Methods
    }
}