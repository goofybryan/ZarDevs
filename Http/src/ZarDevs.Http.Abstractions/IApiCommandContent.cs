using System.Collections.Generic;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// API command content descriptor
    /// </summary>
    public interface IApiCommandContent
    {
        #region Properties

        /// <summary>
        /// Get the content type that can be added to the headers or compared to.
        /// </summary>
        IReadOnlyList<string> MediaTypes { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check if the <paramref name="mediaType"/> is valid for this serializer.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        bool IsValidFor(string mediaType);

        #endregion Methods
    }
}