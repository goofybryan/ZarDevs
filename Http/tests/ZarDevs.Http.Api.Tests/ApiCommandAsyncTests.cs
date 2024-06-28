using NSubstitute;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public abstract class ApiCommandAsyncTests<T> where T : ApiCommandAsync
    {
        #region Properties

        protected abstract Uri ApiUri { get; }

        #endregion Properties

        #region Methods

        [Fact]
        public async Task Execute_ApiCommand_ExpectedBehaviour()
        {
            // Arrange
            var clientMock = Substitute.For<IApiHttpClient>();
            var factoryMock = Substitute.For<IHttpResponseFactory>();
            var requestMock = Substitute.For<IApiCommandRequest>();
            var responseMock = Substitute.For<IApiCommandResponse>();
            var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);

            MockRequest(requestMock);

            factoryMock.CreateResponse(httpResponse).Returns(responseMock);

            var command = CreateCommand(clientMock, factoryMock, httpResponse);

            // Act
            var commandResponse = await command.ExecuteAsync(requestMock);

            // Arrange
            AssertMocks(clientMock, factoryMock, requestMock, responseMock, commandResponse);
        }

        protected virtual void AssertMocks(IApiHttpClient clientMock, IHttpResponseFactory factoryMock, IApiCommandRequest requestMock, IApiCommandResponse responseMock, IApiCommandResponse commandResponse)
        {
            Assert.Same(responseMock, commandResponse);
            _ = requestMock.Received(1).ApiUri;
            factoryMock.Received(1).CreateResponse(Arg.Any<HttpResponseMessage>());
            AssertMocks(clientMock);
        }

        protected abstract void AssertMocks(IApiHttpClient clientMock);

        protected abstract T CreateCommand(IApiHttpClient clientMock, IHttpResponseFactory factoryMock, HttpResponseMessage responseMesage);

        protected virtual void MockRequest(IApiCommandRequest requestMock)
        {
            requestMock.ApiUri.Returns(ApiUri);
        }

        #endregion Methods
    }
}