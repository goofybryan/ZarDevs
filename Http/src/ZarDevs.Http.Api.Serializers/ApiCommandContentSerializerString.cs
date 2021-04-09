using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    internal class ApiCommandContentSerializerString : IApiCommandContentSerializer, IApiCommandContentDeserializer
    {
        #region Fields

        private readonly Encoding _encoding;
        private readonly IList<string> _mediaTypes;

        #endregion Fields

        #region Constructors

        public ApiCommandContentSerializerString(Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.Default;
            _mediaTypes = new List<string> { "text/plain" };
        }

        #endregion Constructors

        #region Properties

        public IList<string> MediaTypes => _mediaTypes;

        #endregion Properties

        #region Methods

        public async Task<TContent> DeserializeAsync<TContent>(HttpContent content)
        {
            object value = await content.ReadAsStringAsync();
            Type contentType = typeof(TContent);

            if (contentType == typeof(string))
                return (TContent)value;

            var converter = TypeDescriptor.GetConverter(typeof(TContent));

            return converter != null && converter.CanConvertFrom(typeof(string)) ? (TContent)converter.ConvertFrom(value) : default;
        }

        public bool IsValidFor(string mediaType)
        {
            return _mediaTypes.Any(type => StringComparer.OrdinalIgnoreCase.Equals(type, mediaType));
        }

        public HttpContent Serialize(IApiCommandRequest request)
        {
            if (!request.HasContent)
                return null;

            var value = request.Content.ToString();
            var content = new StringContent(value, _encoding, _mediaTypes[0]);

            return content;
        }

        #endregion Methods
    }
}