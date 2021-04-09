using System;

namespace ZarDevs.Http.Api
{
    internal class ApiCommandRequest : IApiCommandRequest
    {
        #region Constructors

        public ApiCommandRequest(string apiPath, object content = null)
            : this(new Uri(apiPath, UriKind.RelativeOrAbsolute), content)
        {
        }

        public ApiCommandRequest(Uri apiUri, object content = null)
        {
            ApiUri = apiUri ?? throw new ArgumentNullException(nameof(apiUri));
            Content = content;
        }

        #endregion Constructors

        #region Properties

        public Uri ApiUri { get; }
        public object Content { get; set; }
        public bool HasContent => Content != null;

        #endregion Properties
    }
}