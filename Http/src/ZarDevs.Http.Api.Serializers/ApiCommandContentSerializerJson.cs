using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Serialize a <see cref="IApiCommandRequest"/> content to HttpContent and deserialize the content of a <see cref="HttpResponseMessage"/> using the JSON format.
    /// </summary>
    public class ApiCommandContentSerializerJson : IApiCommandContentSerializer, IApiCommandContentDeserializer
    {
        #region Fields

        private readonly JsonSerializerOptions _serializerOptions;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="serializerOptions">Optionally specify the json serializer options, otherwise set to null for defaults.</param>
        public ApiCommandContentSerializerJson(JsonSerializerOptions serializerOptions = null)
        {
            _serializerOptions = serializerOptions;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the content type that can be added to the headers or compared to.
        /// </summary>
        public IReadOnlyList<string> MediaTypes => HttpContentType.Json;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Deserialize the content to the expected type <typeparamref name="TContent"/>
        /// </summary>
        /// <param name="content">The Http content to deserialize.</param>
        /// <param name="cancellationToken">Optionally add a cancellation token to the deserializer.</param>
        /// <typeparam name="TContent">The expected content type</typeparam>
        /// <returns>The deserialized content of type <typeparamref name="TContent"/></returns>
        public Task<TContent> DeserializeAsync<TContent>(HttpContent content, CancellationToken cancellationToken = default)
        {
            return content.ReadFromJsonAsync<TContent>(_serializerOptions, cancellationToken);
        }

        /// <summary>
        /// Check if the <paramref name="mediaType"/> is valid for this serializer.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public bool IsValidFor(string mediaType) => MediaTypes.Any(type => StringComparer.OrdinalIgnoreCase.Equals(mediaType, type));

        /// <summary>
        /// Serialize the <see cref="IApiCommandRequest.Content"/> to a <see cref="HttpContent"/>
        /// </summary>
        /// <param name="request">The request to serialize.</param>
        /// <returns>The <see cref="HttpContent"/> abstract object.</returns>
        public HttpContent Serialize(IApiCommandRequest request)
        {
            if (!request.HasContent)
                return null;

            var value = request.Content;            ;
            var content = JsonContent.Create(value, new MediaTypeHeaderValue(MediaTypes[0]), _serializerOptions);

            return content;
        }

        #endregion Methods
    }
}