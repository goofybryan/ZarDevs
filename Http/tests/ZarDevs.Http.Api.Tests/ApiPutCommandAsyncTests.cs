using NSubstitute;
using System;
using System.Net.Http;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiPutCommandAsyncTests : ApiCommandContentAsyncTests<ApiPutCommandAsync>
    {
        #region Properties

        protected override Uri ApiUri => new("/api/command/put", UriKind.RelativeOrAbsolute);

        #endregion Properties

        #region Methods

        protected override void AssertMocks(IApiHttpClient clientMock)
        {
            clientMock.Received(1).PutAsync(Arg.Any<Uri>(), Arg.Any<HttpContent>());
        }

        protected override ApiPutCommandAsync CreateCommand(IApiHttpClient clientMock, IApiCommandContentSerializer serializerMock, IHttpResponseFactory factoryMock, HttpResponseMessage responseMesage, HttpContent content)
        {
            clientMock.PutAsync(ApiUri, content).Returns(responseMesage);

            return new ApiPutCommandAsync(clientMock, serializerMock, factoryMock);
        }

        #endregion Methods
    }
}