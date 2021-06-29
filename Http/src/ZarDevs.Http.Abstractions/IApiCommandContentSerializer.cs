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
        /// Serialize the <see cref="IApiCommandRequest.Content"/> to a <see cref="HttpContent"/>
        /// </summary>
        /// <param name="request">The request to serialize.</param>
        /// <returns>The <see cref="HttpContent"/> abstract object.</returns>
        HttpContent Serialize(IApiCommandRequest request);

        #endregion Methods
    }
}