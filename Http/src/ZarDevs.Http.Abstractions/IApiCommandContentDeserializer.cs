using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Deserialize the content of a <see cref="HttpResponseMessage"/>.
    /// </summary>
    public interface IApiCommandContentDeserializer : IApiCommandContent
    {
        #region Methods

        /// <summary>
        /// Deserialize the content to the expected type <typeparamref name="TContent"/>
        /// </summary>
        /// <param name="content">The Http content to deserialize.</param>
        /// <param name="cancellationToken">Optionally add a cancellation token to the deserializer.</param>
        /// <typeparam name="TContent">The expected content type</typeparam>
        /// <returns>The deserialized content of type <typeparamref name="TContent"/></returns>
        Task<TContent> DeserializeAsync<TContent>(HttpContent content, CancellationToken cancellationToken = default);

        #endregion Methods
    }
}