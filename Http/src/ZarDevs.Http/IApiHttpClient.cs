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

        Task<HttpResponseMessage> DeleteAsync(Uri apiUri);

        Task<HttpResponseMessage> GetAsync(Uri apiUri);

        Task<HttpResponseMessage> PostAsync(Uri apiUri, HttpContent httpContent);

#if NET5_0_OR_GREATER
        Task<HttpResponseMessage> PatchAsync(Uri apiUri, HttpContent httpContent);
#endif

        Task<HttpResponseMessage> PutAsync(Uri apiUri, HttpContent httpContent);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        #endregion Methods
    }
}