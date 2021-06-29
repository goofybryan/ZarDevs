using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Command for getting, this will call the <see cref="IApiHttpClient.GetAsync(System.Uri)"/>
    /// </summary>
    public class ApiGetCommandAsync : ApiCommandAsync
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="ApiGetCommandAsync"/>
        /// </summary>
        /// <param name="httpClient">The <see cref="IApiHttpClient"/> client used for sending the delete request.</param>
        /// <param name="responseFactory">The <see cref="IHttpResponseFactory"/> factory used for creating the response.</param>
        public ApiGetCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient, responseFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Call the specific get api call for the specified <paramref name="request"/>
        /// </summary>
        /// <param name="request">The request message that contains the content need for the server call.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the <see cref="IApiHttpClient"/> call.</returns>
        protected override async Task<HttpResponseMessage> OnApiCall(IApiCommandRequest request)
        {
            return await HttpClient.GetAsync(request.ApiUri);
        }

        #endregion Methods
    }
}