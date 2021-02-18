﻿using System;

namespace ZarDevs.Commands.Http
{
    public interface IApiHttpFactory
    {
        #region Methods

        IApiHttpRequestHandlerBinding AddRequestHandler<THandler>(object key = null) where THandler : IApiHttpRequestHandler;

        IApiHttpClient NewClient(object key = null);

        #endregion Methods
    }

    public interface IApiHttpHandlerFactory
    {
        IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler;
    }
}