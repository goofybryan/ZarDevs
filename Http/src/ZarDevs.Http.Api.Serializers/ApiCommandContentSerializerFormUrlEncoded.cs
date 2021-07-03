using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ZarDevs.Http.Api.Serializers
{
    /// <summary>
    /// Form url encoded serializer that can handle content that are of type ' <see cref="IEnumerable{T}"/>' where T is <see cref="KeyValuePair{TKey, TValue}"/>, <see cref="ValueTuple{T1,T2}"/> and
    /// <see cref="Tuple{T1,T2}"/>. T1 and T2 is ( <see cref="string"/>
    /// </summary>
    public class ApiCommandContentSerializerFormUrlEncoded : IApiCommandContentSerializer
    {
        #region Fields

        private readonly IList<IDefaultFormUrlEncodedContentParser> _parsers;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create new instance of the serializer
        /// </summary>
        /// <param name="parsers"></param>
        public ApiCommandContentSerializerFormUrlEncoded(IList<IDefaultFormUrlEncodedContentParser> parsers)
        {
            _parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the content type that can be added to the headers or compared to.
        /// </summary>
        public IReadOnlyList<string> MediaTypes => HttpContentType.FormUrlEncoded;

        #endregion Properties

        #region Methods

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

            foreach(var parser in _parsers)
            {
                if (parser.TryParse(request.Content, out var kv))
                    return new FormUrlEncodedContent(kv);
            }

            throw new NotSupportedException($"The content '{request.Content.GetType()}' is not supported. The content must be a IEnumerable of typeof(KeyValuePair<string, string>, (string key, string value), Tuple<string, string> or return a value from custom Func");
        }

        #endregion Methods
    }
}