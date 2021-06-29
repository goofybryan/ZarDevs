using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Command for deleting, this will call the <see cref="IApiHttpClient.DeleteAsync(System.Uri)"/>
    /// </summary>
    public class ApiDeleteCommandAsync : ApiCommandAsync
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="ApiDeleteCommandAsync"/>
        /// </summary>
        /// <param name="httpClient">The <see cref="IApiHttpClient"/> client used for sending the delete request.</param>
        /// <param name="responseFactory">The <see cref="IHttpResponseFactory"/> factory used for creating the response.</param>
        public ApiDeleteCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient, responseFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Call the specific delete api call for the specified <paramref name="request"/>
        /// </summary>
        /// <param name="request">The request message that contains the content need for the server call.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the <see cref="IApiHttpClient"/> call.</returns>
        protected override async Task<HttpResponseMessage> OnApiCall(IApiCommandRequest request)
        {
            return await HttpClient.DeleteAsync(request.ApiUri);
        }

        #endregion Methods
    }
}