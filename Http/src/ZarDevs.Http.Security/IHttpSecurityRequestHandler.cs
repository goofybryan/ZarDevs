using ZarDevs.Http.Client;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Http Security request handler that will be used to validate current request and inject the appropriate http headers in the request.
    /// </summary>
    public interface IHttpSecurityRequestHandler : IApiHttpRequestHandler
    { 
        
    }
}
