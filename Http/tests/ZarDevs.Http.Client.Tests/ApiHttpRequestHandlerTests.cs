using NSubstitute;
using System.Net.Http;
using Xunit;

namespace ZarDevs.Http.Client.Tests
{
    public class ApiHttpRequestHandlerTests
    {
        #region Methods

        [Fact]
        public async void BaseClassInfrastructure_Test_ExpectedBehaviour()
        {
            // Arrange
            var mock = new ApiHttpRequestHandlerMock();
            var append1 = Substitute.For<IApiHttpRequestHandler>();
            var append2 = Substitute.For<IApiHttpRequestHandler>();

            var next = Substitute.For<IApiHttpRequestHandler>();
            var ignore = Substitute.For<IApiHttpRequestHandler>();

            HttpRequestMessage message = new HttpRequestMessage();

            // Act
            mock.AppendHandler(append1);
            mock.AppendHandler(append2);

            mock.SetNextHandler(ignore);
            mock.SetNextHandler(next);

            await mock.HandleAsync(message);

            // Assert
            Assert.NotNull(mock.RequestMessage);
            Assert.Same(message, mock.RequestMessage);

            Assert.NotNull(mock.Next);
            Assert.NotEqual(ignore, mock.Next);
            Assert.Same(next, mock.Next);

            Assert.NotNull(mock.Handlers);
            Assert.NotEmpty(mock.Handlers);
            Assert.Contains(append1, mock.Handlers);
            Assert.Contains(append2, mock.Handlers);
            Assert.Equal(0, mock.Handlers.IndexOf(append1));
            Assert.Equal(1, mock.Handlers.IndexOf(append2));

            await next.Received(1).HandleAsync(message);
            await append1.Received(1).HandleAsync(message);
            await append2.Received(1).HandleAsync(message);
        }

        #endregion Methods
    }
}