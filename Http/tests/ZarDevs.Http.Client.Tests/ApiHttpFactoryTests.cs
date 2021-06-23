using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace ZarDevs.Http.Client.Tests
{
    public class ApiHttpFactoryTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AddRequestHandler_Execute_AddsHandlerToMap(bool bindingExists)
        {
            // Arrange
            IApiHttpRequestHandlerBinding keyBinding = Substitute.For<IApiHttpRequestHandlerBinding>();
            object key = 1;

            var factoryMock = Substitute.For<IApiHttpHandlerFactory>();
            var mappingMock = Substitute.For<IApiHttpRequestHandlerBindingMap>();

            var factory = new ApiHttpFactory(new HttpClient(), factoryMock, mappingMock);

            if (bindingExists)
            {
                mappingMock.TryGet(key).Returns(keyBinding);
            }
            else
            {
                mappingMock.TryGet(key).Returns((IApiHttpRequestHandlerBinding) null);
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
    }
}
