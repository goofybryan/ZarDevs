using System;

namespace ZarDevs.Commands.Http
{
    public interface IApiHttpFactory
    {
        #region Methods

        IApiHttpRequestHandlerBinding AddRequestHandler<TFor, THandler>(string name = "") where THandler : IApiHttpRequestHandler;

        IApiHttpClient NewClientFor(Type type, string name = "");

        IApiHttpClient NewClientFor<T>(string name = "");

        #endregion Methods
    }

    public interface IApiHttpHandlerFactory
    {
        IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler;
    }
}