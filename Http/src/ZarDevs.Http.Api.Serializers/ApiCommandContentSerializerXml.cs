using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Serialize a <see cref="IApiCommandRequest"/> content to HttpContent and deserialize the content of a <see cref="HttpResponseMessage"/> using the xml format.
    /// </summary>
    public class ApiCommandContentSerializerXml : IApiCommandContentSerializer, IApiCommandContentDeserializer
    {
        #region Fields

        private readonly Encoding _encoding;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the class.
        /// </summary>
        /// <param name="encoding">Optionally specify the content encoding, otherwise defaults <see cref="Encoding.Default"/></param>
        public ApiCommandContentSerializerXml(Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.Default;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the content type that can be added to the headers or compared to.
        /// </summary>
        public IReadOnlyList<string> MediaTypes => HttpContentType.Xml;

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
            using var stream = await content.ReadAsStreamAsync();

            TContent value = (TContent)new XmlSerializer(typeof(TContent)).Deserialize(stream);

            return value;
        }

        /// <summary>
        /// Check if the <paramref name="mediaType"/> is valid for this serializer.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public bool IsValidFor(string mediaType)
        {
            return MediaTypes.Any(type => StringComparer.OrdinalIgnoreCase.Equals(type, mediaType));
        }

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
            var serializer = new XmlSerializer(value.GetType());
            var xmlBuilder = new StringBuilder();
            using (var writer = new StringWriter(xmlBuilder))
            {
                serializer.Serialize(writer, value);
            }

            StringContent content = new(xmlBuilder.ToString(), _encoding, MediaTypes[0]);

            return content;
        }

        #endregion Methods
    }
}