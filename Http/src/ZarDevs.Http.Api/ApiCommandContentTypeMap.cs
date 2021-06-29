using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api command content type map interface used to retrieve the <typeparamref name="TContentType"/> value for the media type.
    /// </summary>
    public interface IApiCommandContentTypeMap<TContentType> where TContentType : IApiCommandContent
    {
        /// <summary>
        /// Get the <typeparamref name="TContentType"/> value from the map for the specified <paramref name="mediaType"/>
        /// </summary>
        /// <param name="mediaType">The media type name</param>
        /// <returns>The specified <typeparamref name="TContentType"/> value.</returns>
        /// <exception cref="ApiCommandContentTypeNotFoundException">This is thrown when no <typeparamref name="TContentType"/> value is found for the <paramref name="mediaType"/></exception>
        TContentType this[string mediaType] { get; }

        /// <summary>
        /// Try and get the <typeparamref name="TContentType"/> value from the map for the specified <paramref name="mediaType"/>
        /// </summary>
        /// <param name="mediaType">The media type name</param>
        /// <param name="contentType">Return specified <typeparamref name="TContentType"/> value.</param>
        /// <returns>A <c>true</c> if the value is found.</returns>
        bool TryGetSerializer(string mediaType, out TContentType contentType);
    }

    /// <summary>
    /// Api command content serializer map used to look up the appropriate <typeparamref name="TContentType"/> value for the specified media type
    /// </summary>
    public class ApiCommandContentTypeMap<TContentType> : IApiCommandContentTypeMap<TContentType> where TContentType : IApiCommandContent
    {
        private readonly IDictionary<string, TContentType> _map;
        private readonly IList<TContentType> _contentTypes;

        /// <summary>
        /// Create a new instance of the map.
        /// </summary>
        /// <param name="contentTypes">Specify the list of content types to map.</param>
        public ApiCommandContentTypeMap(IList<TContentType> contentTypes)
        {
            if (contentTypes is null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (contentTypes.Count == 0)
            {
                throw new ArgumentException($"{nameof(contentTypes)} cannot be an empty collection.", nameof(contentTypes));
            }

            _contentTypes = contentTypes ?? throw new ArgumentNullException(nameof(contentTypes));
            _map = new Dictionary<string, TContentType>();
        }

        /// <summary>
        /// Get the <typeparamref name="TContentType"/> value from the map for the specified <paramref name="mediaType"/>
        /// </summary>
        /// <param name="mediaType">The media type name</param>
        /// <returns>The specified <typeparamref name="TContentType"/> value.</returns>
        /// <exception cref="ApiCommandContentTypeNotFoundException">This is thrown when no <typeparamref name="TContentType"/> value is found for the <paramref name="mediaType"/></exception>
        public TContentType this[string mediaType] => TryGetSerializer(mediaType, out TContentType contentType) ? contentType : throw new ApiCommandContentTypeNotFoundException(mediaType);

        /// <summary>
        /// Try and get the <typeparamref name="TContentType"/> value from the map for the specified <paramref name="mediaType"/>
        /// </summary>
        /// <param name="mediaType">The media type name</param>
        /// <param name="contentType">Return specified <typeparamref name="TContentType"/> value.</param>
        /// <returns>A <c>true</c> if the value is found.</returns>
        public bool TryGetSerializer(string mediaType, out TContentType contentType)
        {
            if (_map.TryGetValue(mediaType, out contentType))
                return true;

            contentType = _contentTypes.FirstOrDefault(ct => ct.IsValidFor(mediaType));

            if (contentType is null)
                return false;

            _map[mediaType] = contentType;

            return true;
        }
    }
}