using System;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Exception thrown when the <see cref="ApiCommandContentTypeMap{TContentType}"/> cannot find a <see cref="IApiCommandContent"/> value for the media type..
    /// </summary>
    [Serializable]
    public class ApiCommandContentTypeNotFoundException : Exception
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="ApiCommandContentTypeNotFoundException"/> exception.
        /// </summary>
        /// <param name="mediaType">The media type that could not be found.</param>
        public ApiCommandContentTypeNotFoundException(string mediaType) : this(mediaType, $"The serializer for '{mediaType}' cannot be found.") { }

        /// <summary>
        /// Create a new instance of the <see cref="ApiCommandContentTypeNotFoundException"/> exception.
        /// </summary>
        /// <param name="mediaType">The media type that could not be found.</param>
        /// <param name="message">The message explaining the exeption.</param>
        public ApiCommandContentTypeNotFoundException(string mediaType, string message) : base(message)
        {
            if (string.IsNullOrWhiteSpace(mediaType))
            {
                throw new ArgumentException($"'{nameof(mediaType)}' cannot be null or whitespace.", nameof(mediaType));
            }

            MediaType = mediaType;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The media type the exeption was thrown for.
        /// </summary>
        public string MediaType { get; }

        #endregion Properties
    }
}