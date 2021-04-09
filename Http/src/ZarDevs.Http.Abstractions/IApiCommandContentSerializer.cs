using System.Net.Http;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Serialize a <see cref="IApiCommandRequest"/> content to HttpContent.
    /// </summary>
    public interface IApiCommandContentSerializer : IApiCommandContent
    {
        #region Methods

        /// <summary>
        /// Serialize the content
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The <see cref="HttpContent"/> abstract object.</returns>
        HttpContent Serialize(IApiCommandRequest request);

        #endregion Methods
    }
}