using NSubstitute;
using Xunit;
using ZarDevs.Runtime;

namespace ZarDevs.Http.Client.Tests
{
    public class DefaultHttpHandlerFactoryTests
    {
        #region Methods

        [Fact]
        public void CreateHandlerBinding_Execute_ReturnsNewBinding()
        {
            // Arrange
            ICreate create = Substitute.For<ICreate>();
            var factory = new DefaultHttpHandlerFactory(create);

            // Act
            var binding = factory.CreateHandlerBinding<ApiHttpRequestHandlerMock>();

            // Assert
            Assert.NotNull(binding);
            Assert.IsType<ApiHttpRequestHandlerBinding<ApiHttpRequestHandlerMock>>(binding);
        }

        [Fact]
        public void GetHandler_Execute_ReturnsExpectedMock()
        {
            // Arrange
            var mockHandler = new ApiHttpRequestHandlerMock();
            ICreate create = Substitute.For<ICreate>();
            create.New<ApiHttpRequestHandlerMock>().Returns(mockHandler);

            var factory = new DefaultHttpHandlerFactory(create);

            // Act
            var handler = factory.GetHandler<ApiHttpRequestHandlerMock>();

            // Assert
            Assert.NotNull(handler);
            Assert.Same(mockHandler, handler);

            create.Received(1).New<ApiHttpRequestHandlerMock>();
        }

        #endregion Methods
    }
}