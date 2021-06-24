using System;
using System.Collections.Generic;
using System.Net.Http;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api command factory used to create commands
    /// </summary>
    public class ApiCommandFactory : IApiCommandFactory
    {
        private readonly IApiHttpFactory _httpFactory;
        #region Fields

        private readonly IHttpResponseFactory _responseFactory;
        private readonly IDictionary<string, IApiCommandContentSerializer> _serializers;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///  Create a new instance of the command factory.
        /// </summary>
        /// <param name="httpFactory">An instance of the api http factory used to get <see cref="IApiHttpClient"/> instances.</param>
        /// <param name="responseFactory">An instance of the response factory to interpret the responses from the server.</param>
        /// <param name="serializers">A list of serializers that is used deserialize the serialize the content that is sent to the server.</param>
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

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for deleting.
        /// </summary>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public IApiCommandAsync CreateDeleteCommand(object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);

            return new ApiDeleteCommandAsync(client, _responseFactory);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for getting.
        /// </summary>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public IApiCommandAsync CreateGetCommand(object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);

            return new ApiGetCommandAsync(client, _responseFactory);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for patching.
        /// </summary>
        /// <param name="mediaType">Specify the media type that will be used to send any content. This will be used to serialize any content to the api call.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public IApiCommandAsync CreatePatchCommand(string mediaType, object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);
            var serializer = _serializers[mediaType];

            return new ApiPatchCommandAsync(client, serializer, _responseFactory);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for posting.
        /// </summary>
        /// <param name="mediaType">Specify the media type that will be used to send any content. This will be used to serialize any content to the api call.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public IApiCommandAsync CreatePostCommand(string mediaType, object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);
            var serializer = _serializers[mediaType];

            return new ApiPostCommandAsync(client, serializer, _responseFactory);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for putting.
        /// </summary>
        /// <param name="mediaType">Specify the media type that will be used to send any content. This will be used to serialize any content to the api call.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public IApiCommandAsync CreatePutCommand(string mediaType, object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);
            var serializer = _serializers[mediaType];

            return new ApiPutCommandAsync(client, serializer, _responseFactory);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandRequest"/> that is used during the request.
        /// </summary>
        /// <param name="apiUrl">Specify the api url</param>
        /// <param name="content">Specify any content that is needed to be sent.</param>
        public IApiCommandRequest CreateRequest(Uri apiUrl, object content = null)
        {
            return new ApiCommandRequest(apiUrl, content);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/>. This command is meant to be used as a custom command when the normal pattern does not fit.
        /// </summary>
        /// <param name="mediaType">Specify the media type that will be used to send any content. This will be used to serialize any content to the api call.</param>
        /// <param name="sendMethod">Specify the <see cref="HttpMethod"/> that the command with use.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public IApiCommandAsync CreateSendCommand(string mediaType, HttpMethod sendMethod, object apiClientKey = null)
        {
            var client = _httpFactory.NewClient(apiClientKey);
            var serializer = _serializers[mediaType];

            return new ApiSendCommandAsync(client, serializer, _responseFactory, sendMethod);
        }

        #endregion Methods
    }
}