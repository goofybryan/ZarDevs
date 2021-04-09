using System.Net.Http;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Http response factory that is used to create instances for the response and deserialization.
    /// </summary>
    public interface IHttpResponseFactory
    {
        #region Methods

        /// <summary>
        /// Create the a <see cref="IApiCommandResponse"/> response from the <see cref="HttpResponseMessage"/> <paramref name="response"/>.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> from the client.</param>
        IApiCommandResponse CreateResponse(HttpResponseMessage response);

        /// <summary>
        /// Get the deserializer for the <paramref name="mediaType"/>
        /// </summary>
        /// <param name="mediaType">The media type of the message.</param>
        IApiCommandContentDeserializer GetDeserializer(string mediaType);

        #endregion Methods
    }
}