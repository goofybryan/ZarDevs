using NSubstitute;
using System;
using System.Net.Http;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiDeleteCommandAsyncTests : ApiCommandAsyncTests<ApiDeleteCommandAsync>
    {
        #region Properties

        protected override Uri ApiUri => new("/api/command/delete", UriKind.RelativeOrAbsolute);

        #endregion Properties

        #region Methods

        protected override void AssertMocks(IApiHttpClient clientMock)
        {
            clientMock.Received(1).DeleteAsync(Arg.Any<Uri>());
        }

        protected override ApiDeleteCommandAsync CreateCommand(IApiHttpClient clientMock, IHttpResponseFactory factoryMock, HttpResponseMessage responseMesage)
        {
            clientMock.DeleteAsync(ApiUri).Returns(responseMesage);

            return new ApiDeleteCommandAsync(clientMock, factoryMock);
        }

        #endregion Methods
    }
}