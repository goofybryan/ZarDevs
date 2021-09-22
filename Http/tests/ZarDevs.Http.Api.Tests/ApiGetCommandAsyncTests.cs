using NSubstitute;
using System;
using System.Net.Http;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiGetCommandAsyncTests : ApiCommandAsyncTests<ApiGetCommandAsync>
    {
        #region Properties

        protected override Uri ApiUri => new("/api/command/get", UriKind.RelativeOrAbsolute);

        #endregion Properties

        #region Methods

        protected override void AssertMocks(IApiHttpClient clientMock)
        {
            clientMock.Received(1).GetAsync(Arg.Any<Uri>());
        }

        protected override ApiGetCommandAsync CreateCommand(IApiHttpClient clientMock, IHttpResponseFactory factoryMock, HttpResponseMessage responseMesage)
        {
            clientMock.GetAsync(ApiUri).Returns(responseMesage);

            return new ApiGetCommandAsync(clientMock, factoryMock);
        }

        #endregion Methods
    }
}