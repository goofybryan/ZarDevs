using NSubstitute;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiCommandResponseTests
    {
        #region Methods

        [Fact]
        public void EnsureSuccess_NotSuccessfull_ThrowsException()
        {
            // Arrange
            const string reason = "Test";
            var factory = Substitute.For<IHttpResponseFactory>();
            var httpResponse = CreateResponse(HttpStatusCode.BadGateway, reason, null);
            var apiResponse = new ApiCommandResponse(factory, httpResponse);

            // Act
            var exception = Assert.Throws<ApiCommandException>(() => apiResponse.EnsureSuccess());

            // Assert
            Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
            Assert.Equal(reason, exception.Message);
            Assert.Same(apiResponse, exception.Response);

            Assert.False(apiResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.BadGateway, apiResponse.StatusCode);
            Assert.Equal(reason, apiResponse.Reason);
        }

        [Fact]
        public void EnsureSuccess_Successfull_NoException()
        {
            // Arrange
            const string reason = "Test";
            var factory = Substitute.For<IHttpResponseFactory>();
            var httpResponse = CreateResponse(HttpStatusCode.Accepted, reason, null);
            var apiResponse = new ApiCommandResponse(factory, httpResponse);

            // Act
            apiResponse.EnsureSuccess();

            // Arrange
            Assert.True(apiResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.Accepted, apiResponse.StatusCode);
            Assert.Equal(reason, apiResponse.Reason);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Content")]
        public async void TryGetContent_Execute_ExpectedResults(string responseContent)
        {
            // Arrange
            var factory = Substitute.For<IHttpResponseFactory>();
            var deserializer = Substitute.For<IApiCommandContentDeserializer>();
            var httpResponse = CreateResponse(HttpStatusCode.Accepted, "", responseContent);
            var apiResponse = new ApiCommandResponse(factory, httpResponse);

            factory.GetDeserializer("text/plain").Returns(deserializer);
            deserializer.DeserializeAsync<string>(httpResponse.Content).Returns(Task.FromResult(responseContent));

            // Act
            var value = await apiResponse.TryGetContent<string>();

            // Assert
            Assert.Equal(responseContent, value);
            factory.Received(1).GetDeserializer("text/plain");
            await deserializer.Received(1).DeserializeAsync<string>(Arg.Any<HttpContent>());
        }

        [Fact]
        public async void TryGetContent_WithNotSuccessfullStatusCode_ThrowsException()
        {
            // Arrange
            const string reason = "Test";
            var factory = Substitute.For<IHttpResponseFactory>();
            var httpResponse = CreateResponse(HttpStatusCode.BadGateway, reason, null);
            var apiResponse = new ApiCommandResponse(factory, httpResponse);

            // Act
            var exception = await Assert.ThrowsAsync<ApiCommandException>(async () => await apiResponse.TryGetContent<string>());

            // Assert
            Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
            Assert.Equal(reason, exception.Message);
            Assert.Same(apiResponse, exception.Response);

            Assert.False(apiResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.BadGateway, apiResponse.StatusCode);
            Assert.Equal(reason, apiResponse.Reason);
        }

        private HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string reason, string response)
        {
            HttpResponseMessage message = new(statusCode) { ReasonPhrase = reason };
            
            message.Content = new StringContent(response ?? "");

            return message;
        }

        #endregion Methods
    }
}