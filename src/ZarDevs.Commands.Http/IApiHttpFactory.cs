using System;

namespace ZarDevs.Commands.Http
{
    public interface IApiHttpFactory
    {
        #region Methods

        IApiHttpRequestHandlerBinding AddRequestHandler<THandler>(string name = "") where THandler : IApiHttpRequestHandler;

        IApiHttpClient NewClient(string name = "");

        #endregion Methods
    }

    public interface IApiHttpHandlerFactory
    {
        IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler;
    }
}