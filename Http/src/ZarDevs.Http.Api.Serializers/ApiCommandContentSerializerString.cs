﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Serialize a <see cref="IApiCommandRequest"/> content to HttpContent and deserialize the content of a <see cref="HttpResponseMessage"/> using the text format.
    /// </summary>
    public class ApiCommandContentSerializerString : IApiCommandContentSerializer, IApiCommandContentDeserializer
    {
        #region Fields

        private readonly Encoding _encoding;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the class.
        /// </summary>
        /// <param name="encoding">Optionally specify the content encoding, otherwise defaults <see cref="Encoding.Default"/></param>
        public ApiCommandContentSerializerString(Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.Default;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the content type that can be added to the headers or compared to.
        /// </summary>
        public IReadOnlyList<string> MediaTypes => HttpContentType.Txt;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Deserialize the content to the expected type <typeparamref name="TContent"/>
        /// </summary>
        /// <param name="content">The Http content to deserialize.</param>
        /// <param name="cancellationToken">Optionally add a cancellation token to the deserializer.</param>
        /// <typeparam name="TContent">The expected content type</typeparam>
        /// <returns>The deserialized content of type <typeparamref name="TContent"/></returns>
        public async Task<TContent> DeserializeAsync<TContent>(HttpContent content, CancellationToken cancellationToken = default)
        {
            object value = await content.ReadAsStringAsync().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            Type contentType = typeof(TContent);

            if (contentType == typeof(string))
                return (TContent)value;

            var converter = TypeDescriptor.GetConverter(typeof(TContent));

            return converter != null && converter.CanConvertFrom(typeof(string)) ? (TContent)converter.ConvertFrom(value) : default;
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

            var value = request.Content.ToString();
            var content = new StringContent(value, _encoding, MediaTypes[0]);

            return content;
        }

        #endregion Methods
    }
}