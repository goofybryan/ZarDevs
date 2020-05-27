using System;
using System.Collections.Generic;
using System.Net.Http;
using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Http
{
    public interface IApiHttpFactory
    {
        #region Methods

        IApiHttpRequestHandlerBinding AddRequestHandler<TFor, THandler>(string name = "") where THandler : IApiHttpRequestHandler;

        IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler;

        IApiHttpClient NewClientFor(Type type, string name = "");

        IApiHttpClient NewClientFor<T>(string name = "");

        #endregion Methods
    }

    public class ApiHttpFactory : IApiHttpFactory
    {
        #region Fields

        private readonly Dictionary<Type, ApiHttpRequestHandlerBindingMap> _handlerMappings;
        private readonly HttpClient _httpClient;
        private readonly IIocContainer _ioc;

        #endregion Fields

        #region Constructors

        public ApiHttpFactory(IIocContainer ioc, HttpClient httpClient)
        {
            _handlerMappings = new Dictionary<Type, ApiHttpRequestHandlerBindingMap>();
            _ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #endregion Constructors

        #region Properties

        [Obsolete("Here for legacy purposes")]
        public static IApiHttpFactory Instance => Ioc.Resolve<IApiHttpFactory>();

        #endregion Properties

        #region Methods

        public IApiHttpRequestHandlerBinding AddRequestHandler<TFor, THandler>(string name = "") where THandler : IApiHttpRequestHandler
        {
            var type = typeof(TFor);
            var binding = GetOrCreateBinding<THandler>(type, name);
            SetOrUpdateBinding(type, name, binding);
            return binding;
        }

        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : IApiHttpRequestHandler
        {
            return _ioc.Resolve<THandler>();
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
            return map?.TryGetBinding(name) ?? new ApiHttpRequestHandlerBinding<THandler>(this);
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
}