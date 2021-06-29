using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    internal class ApiPostCommandAsync : ApiContentCommandAsync
    {
        #region Constructors

        public ApiPostCommandAsync(IApiHttpClient httpClient, IApiCommandContentSerializer serializer, IHttpResponseFactory responseFactory) : base(httpClient, serializer, responseFactory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content)
        {
            return HttpClient.PostAsync(apiUri, content);
        }

        #endregion Methods
    }
}