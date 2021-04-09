using System;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Interface defining an API request. This is what is needed to initiate an api call.
    /// </summary>
    public interface IApiCommandRequest
    {
        #region Properties

        /// <summary>
        /// The API url the request is for. By default, if not specified, url can be set to <see cref="UriKind.RelativeOrAbsolute"/>.
        /// </summary>
        Uri ApiUri { get; }

        /// <summary>
        /// And optional content that will be serialized and added to the content.
        /// </summary>
        object Content { get; set; }

        /// <summary>
        /// Returns a value if there is content specified.
        /// </summary>
        bool HasContent { get; }

        #endregion Properties
    }
}