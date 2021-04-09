using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Serialize a <see cref="IApiCommandRequest"/> content to deserialize a <see cref="HttpResponseMessage"/>.
    /// </summary>
    public interface IApiCommandContentDeserializer : IApiCommandContent
    {

        /// <summary>
        /// Deserialize the content to the expected type <typeparamref name="TContent"/>
        /// </summary>
        /// <param name="content">The Http content to deserialize.</param>
        /// <typeparam name="TContent">The expected content type</typeparam>
        /// <returns>The deserialized content of type <typeparamref name="TContent"/></returns>
        Task<TContent> DeserializeAsync<TContent>(HttpContent content);

        /// <summary>
        /// Get an indicator if the deserializer is valid for the <paramref name="mediaType"/>
        /// </summary>
        /// <param name="mediaType">The media type to check.</param>
        /// <returns></returns>
        bool IsValidFor(string mediaType);
    }
}