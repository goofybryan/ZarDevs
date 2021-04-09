using System;
using System.Collections.Generic;
using System.Net.Http;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    internal class ApiCommandFactory : IApiCommandFactory
    {
        private readonly IApiHttpFactory _httpFactory;
        #region Fields

        private readonly IHttpResponseFactory _responseFactory;
        private readonly IDictionary<string, IApiCommandContentSerializer> _serializers;

        #endregion Fields

        #region Constructors

        public ApiCommandFactory(IApiHttpFactory httpFactory, IHttpResponseFactory responseFactory, IList<IApiCommandContentSerializer> serializers)
        {
            if (serializers is null)
            {
                throw new ArgumentNullException(nameof(serializers));
            }

            if (serializers.Count == 0)
            {
                throw new ArgumentException($"{nameof(serializers)} cannot be an empty collection.", nameof(serializers));
            }
            _httpFactory = httpFactory ?? throw new ArgumentNullException(nameof(httpFactory));
            _responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
            _serializers = new Dictionary<string, IApiCommandContentSerializer>();
            foreach (var serializer in serializers)
            {
                foreach (var mediaType in serializer.MediaTypes)
                {
                    _serializers[mediaType] = serializer;
                }
            }
        }

        #endregion Constructors

        #region Methods

        public IApiCommandAsync CreateDeleteCommand(object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);

            return new ApiDeleteCommandAsync(client, _responseFactory);
        }

        public IApiCommandAsync CreateGetCommand(object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);

            return new ApiGetCommandAsync(client, _responseFactory);
        }

        public IApiCommandAsync CreatePatchCommand(string mediaType, object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);
            var serializer = _serializers[mediaType];

            return new ApiPatchCommandAsync(client, serializer, _responseFactory);
        }

        public IApiCommandAsync CreatePostCommand(string mediaType, object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);
            var serializer = _serializers[mediaType];

            return new ApiPostCommandAsync(client, serializer, _responseFactory);
        }

        public IApiCommandAsync CreatePutCommand(string mediaType, object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);
            var serializer = _serializers[mediaType];

            return new ApiPutCommandAsync(client, serializer, _responseFactory);
        }

        public IApiCommandRequest CreateRequest(Uri apiUrl, object content = null)
        {
            return new ApiCommandRequest(apiUrl, content);
        }

        public IApiCommandAsync CreateSendCommand(string mediaType, HttpMethod sendMethod, object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);
            var serializer = _serializers[mediaType];

            return new ApiSendCommandAsync(client, serializer, _responseFactory, sendMethod);
        }

        #endregion Methods
    }
}