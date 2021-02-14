﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using ZarDevs.Runtime;

namespace ZarDevs.Commands.Http
{
    public class ApiHttpFactory : IApiHttpFactory
    {
        #region Fields

        private readonly IDictionary<Type, ApiHttpRequestHandlerBindingMap> _handlerMappings;
        private readonly IApiHttpHandlerFactory _handlerFactory;
        private readonly HttpClient _httpClient;

        #endregion Fields

        #region Constructors

        public ApiHttpFactory(HttpClient httpClient, IApiHttpHandlerFactory handlerFactory)
        {
            _handlerMappings = new Dictionary<Type, ApiHttpRequestHandlerBindingMap>();
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _handlerFactory = handlerFactory ?? new DefaultHttpHandlerFactor();
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        public IApiHttpRequestHandlerBinding AddRequestHandler<TFor, THandler>(string name = "") where THandler : IApiHttpRequestHandler
        {
            var type = typeof(TFor);
            var binding = GetOrCreateBinding<THandler>(type, name);
            SetOrUpdateBinding(type, name, binding);
            return binding;
        }

        public IApiHttpClient NewClientFor<T>(string name = "")
        {
            return NewClientFor(typeof(T), name);
        }

        public IApiHttpClient NewClientFor(Type type, string name = "")
        {
            var binding = GetOrCreateBinding<ApiHttpRequestHandler>(type, name);
            return NewClient(binding?.Build());
        }

        private IApiHttpRequestHandlerBinding GetOrCreateBinding<THandler>(Type type, string name) where THandler : IApiHttpRequestHandler
        {
            _handlerMappings.TryGetValue(type, out ApiHttpRequestHandlerBindingMap map);
            return map?.TryGetBinding(name) ?? new ApiHttpRequestHandlerBinding<THandler>(_handlerFactory);
        }

        private IApiHttpClient NewClient(IApiHttpRequestHandler handler = null)
        {
            return new ApiHttpClient(handler ?? new ApiHttpRequestHandler(), _httpClient);
        }

        private void SetOrUpdateBinding(Type type, string name, IApiHttpRequestHandlerBinding binding)
        {
            if (!_handlerMappings.TryGetValue(type, out ApiHttpRequestHandlerBindingMap map))
            {
                map = new ApiHttpRequestHandlerBindingMap();
                _handlerMappings[type] = map;
            }

            map[name] = binding;
        }

        #endregion Methods
    }

    public class DefaultHttpHandlerFactor : IApiHttpHandlerFactory
    {
        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler
        {
            return Create.Instance.New<THandler>();
        }
    }
}