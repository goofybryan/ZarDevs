using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api
{
    public class ApiPatchCommandAsync : ApiContentCommandAsync, IApiPutCommandAsync
    {
        #region Fields

        private readonly IHttpResponseFactory _responseFactory;

        #endregion Fields

        #region Constructors

        public ApiPatchCommandAsync(IApiHttpClient httpClient, IHttpResponseFactory factory) : base(httpClient, factory)
        {
            _responseFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        #endregion Constructors

        #region Methods

        protected override async Task<HttpResponseMessage> OnApiCall(ApiCommandContentRequest contentRequest)
        {

            return await HttpClient.PatchAsync(contentRequest.ApiUri, contentRequest.Content.WriteAsJson());
        }

        #endregion Methods
    }
}