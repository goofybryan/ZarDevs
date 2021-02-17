using System;
using System.Collections.Generic;
using System.Text;
using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Http
{
    internal class DependencyApiHttpFactoryHandler : IApiHttpHandlerFactory
    {
        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler
        {
            return Ioc.Resolve<THandler>();
        }
    }
}
