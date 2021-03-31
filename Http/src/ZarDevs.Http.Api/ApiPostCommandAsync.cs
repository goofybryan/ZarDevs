using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    public class ApiPostCommandAsync : ApiContentCommandAsync, IApiPostCommandAsync
    {
        #region Constructors

        public ApiPostCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory responseFactory) : base(httpClient, responseFactory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandContentRequest contentRequest)
        {
            return await HttpClient.PostAsync(contentRequest.ApiUri, contentRequest.Content.WriteAsJson());
        }

        #endregion Methods
    }
}