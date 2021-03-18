using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Http factory interface that will be used to create the core security handlers.
    /// </summary>
    public interface IHttpSecurityFactory
    {
        IHttpSecurityRequestHandler CreateHandler();
    }
}
