using NSubstitute;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiSendCommandAsyncTests
    {
        private const string _requestContent = "Test";

        #region Properties

        protected static Uri ApiUri => new("/api/command/send", UriKind.RelativeOrAbsolute);

        #endregion Properties

        #region Methods

        [Fact]
        public async void Execute_ApiCommand_ExpectedBehaviour()
        {
            // Arrange
            var clientMock = Substitute.For<IApiHttpClient>();
            var factoryMock = Substitute.For<IHttpResponseFactory>();
            var requestMock = Substitute.For<IApiCommandRequest>();
            var responseMock = Substitute.For<IApiCommandResponse>();
            var serializerMock = Substitute.For<IApiCommandContentSerializer>();
            var requestMessage = new HttpRequestMessage();

            var method = new HttpMethod("Testing");
            var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
            var content = new StringContent(_requestContent);

            requestMock.ApiUri.Returns(ApiUri);
            requestMock.Content.Returns(requestMock);

            serializerMock.Serialize(requestMock).Returns(content);

            factoryMock.CreateResponse(httpResponse).Returns(responseMock);

            clientMock.CreateRequest(method, ApiUri, content).Returns(requestMessage);
            clientMock.SendAsync(requestMessage).Returns(httpResponse);

            var command = new ApiSendCommandAsync(clientMock, serializerMock, factoryMock, method);

            // Act
            var commandResponse = await command.ExecuteAsync(requestMock);

            // Arrange

            Assert.Same(responseMock, commandResponse);
            _ = requestMock.Received(1).ApiUri;


            factoryMock.Received(1).CreateResponse(Arg.Any<HttpResponseMessage>());
            await clientMock.Received(1).SendAsync(Arg.Any<HttpRequestMessage>());

            clientMock.Received(1).CreateRequest(Arg.Any<HttpMethod>(), Arg.Any<Uri>(), Arg.Any<HttpContent>());
            serializerMock.Received(1).Serialize(Arg.Any<IApiCommandRequest>());
        }

        #endregion Methods
    }
}