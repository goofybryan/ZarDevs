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
        IList<string> MediaTypes { get; }

        #endregion Properties
    }
}