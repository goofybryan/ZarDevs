using System;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// API command request
    /// </summary>
    public class ApiCommandRequest : IApiCommandRequest
    {
        #region Constructors

        /// <summary>
        /// Create a new instance with the <paramref name="apiPath"/> and optional <paramref name="content"/>
        /// </summary>
        /// <param name="apiPath">The url path, can be relative or absolute.</param>
        /// <param name="content">Optionally specify the content.</param>
        public ApiCommandRequest(string apiPath, object content = null)
            : this(new Uri(apiPath, UriKind.RelativeOrAbsolute), content)
        {
        }

        /// <summary>
        /// Create a new instance with the <paramref name="apiPath"/> and optional <paramref name="content"/>
        /// </summary>
        /// <param name="apiUri">Specify the uri path.</param>
        /// <param name="content">Optionally specify the content.</param>
        public ApiCommandRequest(Uri apiUri, object content = null)
        {
            ApiUri = apiUri ?? throw new ArgumentNullException(nameof(apiUri));
            Content = content;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The API url the request is for. By default, if not specified, url can be set to <see cref="UriKind.RelativeOrAbsolute"/>.
        /// </summary>
        public Uri ApiUri { get; }

        /// <summary>
        /// And optional content that will be serialized and added to the content.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Returns a value if there is content specified.
        /// </summary>
        public bool HasContent => Content != null;

        #endregion Properties
    }
}