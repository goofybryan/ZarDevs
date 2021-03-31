using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api PUT command interface.
    /// </summary>
    public interface IApiPutCommandAsync : ICommandAsync<ApiCommandRequest, ApiCommandResponse>
    {
    }

    /// <summary>
    /// Api PUT command that will send a PUT request to the server. The request must have a body.
    /// </summary>
    public class ApiPutCommandAsync : ApiContentCommandAsync, IApiPutCommandAsync
    {
        #region Constructors

        public ApiPutCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory factory) : base(httpClient, factory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandContentRequest contentRequest)
        {
            return await HttpClient.PutAsync(contentRequest.ApiUri, contentRequest.Content.WriteAsJson());
        }

        #endregion Methods
    }
}