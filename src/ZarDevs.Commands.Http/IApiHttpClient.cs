using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Commands.Http
{
    public interface IApiHttpClient : IDisposable
    {
        #region Methods

        Task<HttpResponseMessage> DeleteAsync(Uri apiUri);

        Task<HttpResponseMessage> GetAsync(Uri apiUri);

        Task<HttpResponseMessage> PostAsync(Uri apiUri, HttpContent httpContent);

        Task<HttpResponseMessage> PutAsync(Uri apiUri, HttpContent httpContent);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        #endregion Methods
    }
}