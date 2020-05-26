using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Commands.Http
{
    public interface IApiHttpRequestHandler
    {
        #region Methods

        void AppendHandler(IApiHttpRequestHandler handler);

        Task HandleAsync(HttpRequestMessage request);

        void SetInnerHandler(IApiHttpRequestHandler handler);

        #endregion Methods
    }
}