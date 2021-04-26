using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ZarDevs.Http.Api.Serializers
{
    /// <summary>
    /// Form url encoded serializer that can handle content that are of type '<see cref="IEnumerable{T}"/>' 
    /// where T is <see cref="KeyValuePair{TKey, TValue}"/>, <see cref="ValueTuple{T1,T2}"/> and <see cref="Tuple{T1,T2}"/>.
    /// T1 and T2 is (<see cref="string"/>
    /// </summary>
    public class ApiCommandContentSerializerFormUrlEncoded : IApiCommandContentSerializer
    {
        #region Properties

        /// <summary>
        /// Get the content type that can be added to the headers or compared to.
        /// </summary>
        public IList<string> MediaTypes => HttpContentType.FormUrlEncoded;

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

            if (request.Content is IEnumerable<KeyValuePair<string, string>> kv)
                return new FormUrlEncodedContent(kv);

            if (request.Content is IEnumerable<(string key, string value)> v)
                return new FormUrlEncodedContent(v.Select(i => new KeyValuePair<string, string>(i.key, i.value)));

            if (request.Content is IEnumerable<Tuple<string, string>> t)
                return new FormUrlEncodedContent(t.Select(i => new KeyValuePair<string, string>(i.Item1, i.Item2)));

            return null;
        }

        #endregion Methods
    }
}