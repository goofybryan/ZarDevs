using NSubstitute;
using System.Net.Http;
using Xunit;

namespace ZarDevs.Http.Client.Tests
{
    public class ApiHttpFactoryTests
    {
        #region Methods

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AddRequestHandler_Execute_AddsHandlerToMap(bool bindingExists)
        {
            // Arrange
            var factoryMock = Substitute.For<IApiHttpHandlerFactory>();
            var mappingMock = Substitute.For<IApiHttpRequestHandlerBindingMap>();

            var factory = new ApiHttpFactory(new HttpClient(), factoryMock, mappingMock);

            IApiHttpRequestHandlerBinding keyBinding = Substitute.For<IApiHttpRequestHandlerBinding>();
            const int key = 1;

            if (bindingExists)
            {
                mappingMock.TryGet(key).Returns(keyBinding);
            }
            else
            {
                mappingMock.TryGet(key).Returns((IApiHttpRequestHandlerBinding)null);
                factoryMock.CreateHandlerBinding<ApiHttpRequestHandlerMock>().Returns(keyBinding);
            }

            // Act
            var binding = factory.AddRequestHandler<ApiHttpRequestHandlerMock>(key);

            // Assert
            Assert.NotNull(binding);
            Assert.Same(keyBinding, binding);

            mappingMock.Received(1).TryGet(Arg.Any<object>());
            factoryMock.Received(bindingExists ? 0 : 1).CreateHandlerBinding<ApiHttpRequestHandlerMock>();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void NewClient_Execute_ReturnsNewClient(bool bindingExists)
        {
            // Arrange
            var factoryMock = Substitute.For<IApiHttpHandlerFactory>();
            var mappingMock = Substitute.For<IApiHttpRequestHandlerBindingMap>();

            var factory = new ApiHttpFactory(new HttpClient(), factoryMock, mappingMock);

            const int key = 1;
            IApiHttpRequestHandlerBinding keyBinding = null;

            if (bindingExists)
            {
                keyBinding = Substitute.For<IApiHttpRequestHandlerBinding>();
            }

            mappingMock.TryGet(key).Returns(keyBinding);

            // Act
            var client = factory.NewClient(key);

            // Assert
            Assert.NotNull(client);
            mappingMock.Received(1).TryGet(Arg.Any<object>());
            keyBinding?.Received(1).Build();
        }

        #endregion Methods
    }
}