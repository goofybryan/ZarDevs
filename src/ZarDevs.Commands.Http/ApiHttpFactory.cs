using System;
using System.Net.Http;
using ZarDevs.Runtime;

namespace ZarDevs.Commands.Http
{
    public class ApiHttpFactory : IApiHttpFactory
    {
        #region Fields

        private readonly ApiHttpRequestHandlerBindingMap _handlerMappings;
        private readonly IApiHttpHandlerFactory _handlerFactory;
        private readonly HttpClient _httpClient;

        #endregion Fields

        #region Constructors90

        public ApiHttpFactory(HttpClient httpClient, IApiHttpHandlerFactory handlerFactory)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _handlerMappings = new ApiHttpRequestHandlerBindingMap();
        }

        #endregion Constructors

        #region Properties

        public static IApiHttpFactory Instance { get; set; }

        #endregion Properties

        #region Methods

        public IApiHttpRequestHandlerBinding AddRequestHandler<THandler>(object key= null) where THandler : class, IApiHttpRequestHandler
        {
            var binding = GetOrCreateBinding<THandler>(key);
            return binding;
        }

        public IApiHttpClient NewClient(object key = null)
        {
            var binding = GetOrCreateBinding<ApiHttpRequestHandler>(key);
            return NewClient(binding?.Build());
        }

        private IApiHttpRequestHandlerBinding GetOrCreateBinding<THandler>(object key) where THandler : class, IApiHttpRequestHandler
        {
            return _handlerMappings?.TryGetBinding(key) ?? new ApiHttpRequestHandlerBinding<THandler>(_handlerFactory);
        }

        private IApiHttpClient NewClient(IApiHttpRequestHandler handler = null)
        {
            return new ApiHttpClient(handler ?? new ApiHttpRequestHandler(), _httpClient);
        }

        #endregion Methods
    }

    public class DefaultHttpHandlerFactory : IApiHttpHandlerFactory
    {
        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : class, IApiHttpRequestHandler
        {
            return Create.Instance.New<THandler>();
        }
    }
}