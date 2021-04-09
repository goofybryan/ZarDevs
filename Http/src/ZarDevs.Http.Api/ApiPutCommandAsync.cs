using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    internal class ApiPutCommandAsync : ApiContentCommandAsync
    {
        #region Constructors

        public ApiPutCommandAsync(IApiHttpClient httpClient, IApiCommandContentSerializer serializer, IHttpResponseFactory factory) : base(httpClient, serializer, factory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override async Task<HttpResponseMessage> OnApiCall(Uri apiUri, HttpContent content)
        {
            return await HttpClient.PutAsync(apiUri, content);
        }

        #endregion Methods
    }
}