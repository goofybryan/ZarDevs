using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ZarDevs.Commands.Http
{
    public static class ApiHttpFactory
    {
        #region Constructors

        static ApiHttpFactory()
        {
            HandlerMappings = new Dictionary<Type, ApiHttpRequestHandlerBindingMap>();
            HttpClient = new HttpClient();
        }

        #endregion Constructors

        #region Properties

        internal static Dictionary<Type, ApiHttpRequestHandlerBindingMap> HandlerMappings { get; }
        internal static HttpClient HttpClient { get; }

        #endregion Properties

        #region Methods

        public static IApiHttpRequestHandlerBinding AddRequestHandler<TFor, THandler>(string name = "") where THandler : IApiHttpRequestHandler
        {
            var type = typeof(TFor);
            var binding = GetOrCreateBinding<THandler>(type, name);
            SetOrUpdateBinding(type, name, binding);
            return binding;
        }

        public static IApiHttpClient NewClientFor<T>(string name = "")
        {
            return NewClientFor(typeof(T), name);
        }

        public static IApiHttpClient NewClientFor(Type type, string name = "")
        {
            var binding = GetOrCreateBinding<ApiHttpRequestHandler>(type, name);
            return NewClient(binding?.Build());
        }

        private static IApiHttpRequestHandlerBinding GetOrCreateBinding<THandler>(Type type, string name) where THandler : IApiHttpRequestHandler
        {
            HandlerMappings.TryGetValue(type, out ApiHttpRequestHandlerBindingMap map);
            return map?.TryGetBinding(name) ?? new ApiHttpRequestHandlerBinding<THandler>();
        }

        private static IApiHttpClient NewClient(IApiHttpRequestHandler handler = null)
        {
            return new ApiHttpClient(handler ?? new ApiHttpRequestHandler(), HttpClient);
        }

        private static void SetOrUpdateBinding(Type type, string name, IApiHttpRequestHandlerBinding binding)
        {
            if (!HandlerMappings.TryGetValue(type, out ApiHttpRequestHandlerBindingMap map))
            {
                map = new ApiHttpRequestHandlerBindingMap();
                HandlerMappings[type] = map;
            }

            map[name] = binding;
        }

        #endregion Methods
    }
}