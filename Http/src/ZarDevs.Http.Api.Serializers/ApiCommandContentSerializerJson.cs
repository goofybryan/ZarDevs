using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Serialize a <see cref="IApiCommandRequest"/> content to HttpContent and deserialize the content of a <see cref="HttpResponseMessage"/> using the JSON format.
    /// </summary>
    public class ApiCommandContentSerializerJson : IApiCommandContentSerializer, IApiCommandContentDeserializer
    {
        #region Fields

        private readonly Encoding _encoding;
        private readonly Formatting _formatting;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="encoding">Optionally specify the content encoding, otherwise defaults <see cref="Encoding.Default"/></param>
        /// <param name="formatting">Optionally specift the formatting, otherwise defaults to <see cref="Formatting.None"/></param>
        public ApiCommandContentSerializerJson(Encoding encoding = null, Formatting formatting = Formatting.None)
        {
            _encoding = encoding ?? Encoding.Default;
            _formatting = formatting;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the content type that can be added to the headers or compared to.
        /// </summary>
        public IList<string> MediaTypes => HttpContentType.Json;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Deserialize the content to the expected type <typeparamref name="TContent"/>
        /// </summary>
        /// <param name="content">The Http content to deserialize.</param>
        /// <typeparam name="TContent">The expected content type</typeparam>
        /// <returns>The deserialized content of type <typeparamref name="TContent"/></returns>
        public async Task<TContent> DeserializeAsync<TContent>(HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            TContent value = JsonConvert.DeserializeObject<TContent>(json);
            return value;
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

            var value = request.Content;
            var jsonString = JsonConvert.SerializeObject(value, _formatting);
            var content = new StringContent(jsonString, _encoding, MediaTypes[0]);

            return content;
        }

        #endregion Methods
    }
}