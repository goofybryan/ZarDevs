using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.Http.Security
{
    internal class HttpSecurityJwtFactory : IHttpSecurityFactory
    {
        public IHttpSecurityRequestHandler CreateHandler()
        {
            return new HttpSecurityRequestHandler();
        }
    }

    internal class HttpSecurityRequestHandler : IHttpSecurityRequestHandler
    { }
}
