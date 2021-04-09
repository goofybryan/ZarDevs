using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    internal class ApiCommandContentSerializerJson : IApiCommandContentSerializer, IApiCommandContentDeserializer
    {
        #region Fields

        private const string _mediaType = "application/json";
        private readonly Encoding _encoding;
        private readonly Formatting _formatting;
        private readonly IList<string> _mediaTypes;

        #endregion Fields

        #region Constructors

        public ApiCommandContentSerializerJson(Encoding encoding = null, Formatting formatting = Formatting.None)
        {
            _encoding = encoding ?? Encoding.Default;
            _formatting = formatting;
            _mediaTypes = new List<string> { _mediaType };
        }

        #endregion Constructors

        #region Properties

        public IList<string> MediaTypes => _mediaTypes;

        #endregion Properties

        #region Methods

        public async Task<TContent> DeserializeAsync<TContent>(HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            TContent value = JsonConvert.DeserializeObject<TContent>(json);
            return value;
        }

        public bool IsValidFor(string mediaType) => _mediaTypes.Any(type => StringComparer.OrdinalIgnoreCase.Equals(mediaType, type));

        public HttpContent Serialize(IApiCommandRequest request)
        {
            if (!request.HasContent)
                return null;

            var value = request.Content;
            var jsonString = JsonConvert.SerializeObject(value, _formatting);
            var content = new StringContent(jsonString, _encoding, _mediaType);

            return content;
        }

        #endregion Methods
    }
}