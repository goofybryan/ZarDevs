using NSubstitute;
using System.Net.Http;
using Xunit;

namespace ZarDevs.Http.Api.Tests
{
    public class HttpResponseFactoryTests
    {
        #region Methods

        [Fact]
        public void CreateResponse_Execute_CreatesInstance()
        {
            // Arrange
            var mapMock = Substitute.For<IApiCommandContentTypeMap<IApiCommandContentDeserializer>>();
            var factory = new HttpResponseFactory(mapMock);
            var response = new HttpResponseMessage();

            // Act
            var commandResponse = factory.CreateResponse(response);

            // Assert
            Assert.NotNull(commandResponse);
            Assert.IsType<ApiCommandResponse>(commandResponse);
        }

        [Fact]
        public void GetDeserializer_Execute_ReturnsDeserializer()
        {
            // Arrange
            const string mediaType = "test/media";
            var deserializerMock = Substitute.For<IApiCommandContentDeserializer>();
            var mapMock = Substitute.For<IApiCommandContentTypeMap<IApiCommandContentDeserializer>>();
            var factory = new HttpResponseFactory(mapMock);

            mapMock[mediaType].Returns(deserializerMock);

            // Act
            var deserializer = factory.GetDeserializer(mediaType);

            // Assert
            Assert.NotNull(deserializer);
            Assert.Same(deserializerMock, deserializer);
        }

        #endregion Methods
    }
}