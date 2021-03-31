using System;
using System.Net.Http;
using ZarDevs.Runtime;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Http Api factory that creates new instance of the <see cref="IApiHttpClient"/>
    /// </summary>
    public class ApiHttpFactory : IApiHttpFactory
    {
        #region Fields

        private readonly IApiHttpHandlerFactory _handlerFactory;
        private readonly ApiHttpRequestHandlerBindingMap _handlerMappings;
        private readonly HttpClient _httpClient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the factory.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> object that all <see cref="IApiHttpClient"/> will use.</param>
        /// <param name="handlerFactory">The handler factory that will be used to create the instances of <see cref="IApiHttpRequestHandler"/> from the <see cref="IApiHttpRequestHandlerBinding"/> bindings.</param>
        public ApiHttpFactory(HttpClient httpClient, IApiHttpHandlerFactory handlerFactory)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _handlerMappings = new ApiHttpRequestHandlerBindingMap();
        }

        /// <summary>
        /// A static instance of the <see cref="IApiHttpFactory"/>.
        /// </summary>
        /// <remarks>
        ///  This needs to be initialized and set before use.
        /// </remarks>
        public static IApiHttpFactory Instance { get; set; }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add a request handler to the <see cref="ApiHttpFactory"/>. This will append the handler to any new <see cref="IApiHttpClient"/> created.
        /// </summary>
        /// <typeparam name="THandler">Add the <typeparamref name="THandler"/> handler for the specified key.</typeparam>
        /// <param name="key">Specify an optional key for the handler. If a key is specified, it will only be added to new <see cref="IApiHttpClient"/> with the same key. Default is null.</param>
        /// <returns>A <see cref="IApiHttpRequestHandlerBinding"/> where you can link multiple additional handlers.</returns>
        public IApiHttpRequestHandlerBinding AddRequestHandler<THandler>(object key = null) where THandler : class, IApiHttpRequestHandler
        {
            var binding = _handlerMappings.TryGet(key);

            if (binding == null)
            {
                binding = _handlerFactory.CreateHandlerBinding<THandler>();
                _handlerMappings.TrySet(key, binding);
            }

            return binding;
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiHttpClient"/>. This will append any registered <see cref="IApiHttpRequestHandler"/> for the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Specify an optional key. This will append any registered <see cref="IApiHttpRequestHandler"/> for the specified <paramref name="key"/> otherwise use the default handlers(those registered with null)</param>
        /// <returns>An new instance of the <see cref="IApiHttpClient"/></returns>
        public IApiHttpClient NewClient(object key = null)
        {
            var binding = _handlerMappings.TryGet(key);
            var handler = binding?.Build();

            return new ApiHttpClient(handler, _httpClient);
        }

        #endregion Methods
    }
}