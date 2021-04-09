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
    internal class ApiCommandContentSerializerXml : IApiCommandContentSerializer, IApiCommandContentDeserializer
    {
        #region Fields

        private const string _mediaType = "application/xml";
        private readonly Encoding _encoding;
        private readonly IList<string> _mediaTypes;

        #endregion Fields

        #region Constructors

        public ApiCommandContentSerializerXml(Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.Default;
            _mediaTypes = new List<string> { _mediaType, "text/xml" };
        }

        #endregion Constructors

        #region Properties

        public IList<string> MediaTypes => _mediaTypes;

        #endregion Properties

        #region Methods

        public async Task<TContent> DeserializeAsync<TContent>(HttpContent content)
        {
            using var stream = await content.ReadAsStreamAsync();

            TContent value = (TContent)new XmlSerializer(typeof(TContent)).Deserialize(stream);

            return value;
        }

        public bool IsValidFor(string mediaType)
        {
            return _mediaTypes.Any(type => StringComparer.OrdinalIgnoreCase.Equals(type, mediaType));
        }

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

            StringContent content = new(xmlBuilder.ToString(), _encoding, _mediaType);

            return content;
        }

        #endregion Methods
    }
}