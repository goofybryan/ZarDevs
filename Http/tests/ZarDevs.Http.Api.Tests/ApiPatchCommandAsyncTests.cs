using NSubstitute;
using System;
using System.Net.Http;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiPatchCommandAsyncTests : ApiCommandContentAsyncTests<ApiPatchCommandAsync>
    {
        #region Properties

        protected override Uri ApiUri => new("/api/command/patch", UriKind.RelativeOrAbsolute);

        #endregion Properties

        #region Methods

        protected override void AssertMocks(IApiHttpClient clientMock)
        {
            clientMock.Received(1).PatchAsync(Arg.Any<Uri>(), Arg.Any<HttpContent>());
        }

        protected override ApiPatchCommandAsync CreateCommand(IApiHttpClient clientMock, IApiCommandContentSerializer serializerMock, IHttpResponseFactory factoryMock, HttpResponseMessage responseMesage, HttpContent content)
        {
            clientMock.PatchAsync(ApiUri, content).Returns(responseMesage);

            return new ApiPatchCommandAsync(clientMock, serializerMock, factoryMock);
        }

        #endregion Methods
    }
}