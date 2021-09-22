using NSubstitute;
using System;
using System.Net.Http;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiPostCommandAsyncTests : ApiCommandContentAsyncTests<ApiPostCommandAsync>
    {
        #region Properties

        protected override Uri ApiUri => new("/api/command/post", UriKind.RelativeOrAbsolute);

        #endregion Properties

        #region Methods

        protected override void AssertMocks(IApiHttpClient clientMock)
        {
            clientMock.Received(1).PostAsync(Arg.Any<Uri>(), Arg.Any<HttpContent>());
        }

        protected override ApiPostCommandAsync CreateCommand(IApiHttpClient clientMock, IApiCommandContentSerializer serializerMock, IHttpResponseFactory factoryMock, HttpResponseMessage responseMesage, HttpContent content)
        {
            clientMock.PostAsync(ApiUri, content).Returns(responseMesage);

            return new ApiPostCommandAsync(clientMock, serializerMock, factoryMock);
        }

        #endregion Methods
    }
}