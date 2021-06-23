using NSubstitute;
using System;
using Xunit;

namespace ZarDevs.Http.Client.Tests
{
    public class ApiHttpRequestHandlerBindingTests
    {
        #region Methods

        [Fact]
        public void AppendHandler_UsingGeneric_AddsSequentially()
        {
            // Arrange
            var factoryMock = Substitute.For<IApiHttpHandlerFactory>();

            var binding = new ApiHttpRequestHandlerBinding(factoryMock, typeof(ApiHttpRequestHandlerMock));

            // Act
            binding.AppendHandler<ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType1Mock>();
            binding.AppendHandler<ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType2Mock>();
            binding.AppendHandler<ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType3Mock>();
            binding.AppendHandler<ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType4Mock>();
            binding.AppendHandler<ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType5Mock>();

            // Assert
            Assert.Equal(5, binding.Bindings.Count);
            Assert.Equal(typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType1Mock), binding.Bindings[0].HandlerType);
            Assert.Equal(typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType2Mock), binding.Bindings[1].HandlerType);
            Assert.Equal(typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType3Mock), binding.Bindings[2].HandlerType);
            Assert.Equal(typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType4Mock), binding.Bindings[3].HandlerType);
            Assert.Equal(typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType5Mock), binding.Bindings[4].HandlerType);
        }

        [Fact]
        public void AppendHandler_UsingType_AddsSequentially()
        {
            // Arrange
            var factoryMock = Substitute.For<IApiHttpHandlerFactory>();
            var binding = new ApiHttpRequestHandlerBinding(factoryMock, typeof(ApiHttpRequestHandlerMock));

            var handler1 = typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType1Mock);
            var handler2 = typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType2Mock);
            var handler3 = typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType3Mock);
            var handler4 = typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType4Mock);
            var handler5 = typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType5Mock);

            // Act
            binding.AppendHandler(handler1);
            binding.AppendHandler(handler2);
            binding.AppendHandler(handler3);
            binding.AppendHandler(handler4);
            binding.AppendHandler(handler5);

            // Assert
            Assert.Equal(5, binding.Bindings.Count);
            Assert.Equal(handler1, binding.Bindings[0].HandlerType);
            Assert.Equal(handler2, binding.Bindings[1].HandlerType);
            Assert.Equal(handler3, binding.Bindings[2].HandlerType);
            Assert.Equal(handler4, binding.Bindings[3].HandlerType);
            Assert.Equal(handler5, binding.Bindings[4].HandlerType);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void Build_Execute_ExpectedBehaviour(bool hasNextHandler, bool hasAppendHandlers)
        {
            // Arrange
            var handlerMock = Substitute.For<IApiHttpRequestHandler>();
            var factoryMock = Substitute.For<IApiHttpHandlerFactory>();
            factoryMock.GetHandler(typeof(ApiHttpRequestHandlerMock)).Returns(handlerMock);

            var binding = new ApiHttpRequestHandlerBinding(factoryMock, typeof(ApiHttpRequestHandlerMock));

            var nextHandlersAssert = hasNextHandler ? ArrangeNext(handlerMock, binding) : () => handlerMock.Received(0).SetNextHandler(Arg.Any<IApiHttpRequestHandler>());
            var appendHandlersAssert = hasAppendHandlers ? ArrangeHandlers(handlerMock, binding) : () => handlerMock.Received(0).AppendHandler(Arg.Any<IApiHttpRequestHandler>());

            // Act
            var handler = binding.Build();

            // Assert
            Assert.Same(handlerMock, handler);

            nextHandlersAssert.Invoke();
            appendHandlersAssert.Invoke();
        }

        [Fact]
        public void SetNextHandler_UsingGeneric_AddsSequentially()
        {
            // Arrange
            var factoryMock = Substitute.For<IApiHttpHandlerFactory>();
            var binding = new ApiHttpRequestHandlerBinding(factoryMock, typeof(ApiHttpRequestHandlerMock));

            // Act
            binding.SetNextHandler<ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType1Mock>();
            binding.SetNextHandler<ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType2Mock>();

            // Assert
            Assert.NotNull(binding.Next);
            Assert.NotEqual(typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType1Mock), binding.Next.HandlerType);
            Assert.Equal(typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType2Mock), binding.Next.HandlerType);
        }

        [Fact]
        public void SetNextHandler_UsingType_AddsSequentially()
        {
            // Arrange
            var factoryMock = Substitute.For<IApiHttpHandlerFactory>();
            var binding = new ApiHttpRequestHandlerBinding(factoryMock, typeof(ApiHttpRequestHandlerMock));

            var handler1 = typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType1Mock);
            var handler2 = typeof(ApiHttpRequestHandlerMock.ApiHttpRequestHandlerDifferentType2Mock);

            // Act
            binding.SetNextHandler(handler1);
            binding.SetNextHandler(handler2);

            // Assert
            Assert.NotNull(binding.Next);
            Assert.NotEqual(handler1, binding.Next.HandlerType);
            Assert.Equal(handler2, binding.Next.HandlerType);
        }

        private static Action ArrangeHandlers(IApiHttpRequestHandler handlerMock, ApiHttpRequestHandlerBinding binding)
        {
            var handlerBinding1 = Substitute.For<IApiHttpRequestHandlerBinding>();
            var handlerBinding2 = Substitute.For<IApiHttpRequestHandlerBinding>();
            var handlerBinding3 = Substitute.For<IApiHttpRequestHandlerBinding>();

            var handler1 = Substitute.For<IApiHttpRequestHandler>();
            var handler2 = Substitute.For<IApiHttpRequestHandler>();
            var handler3 = Substitute.For<IApiHttpRequestHandler>();

            handlerBinding1.Build().Returns(handler1);
            handlerBinding2.Build().Returns(handler2);
            handlerBinding3.Build().Returns(handler3);

            binding.Bindings.Add(handlerBinding1);
            binding.Bindings.Add(handlerBinding2);
            binding.Bindings.Add(handlerBinding3);

            return () =>
            {
                handlerBinding1.Received(1).Build();
                handlerBinding2.Received(1).Build();
                handlerBinding3.Received(1).Build();

                handlerMock.Received(3).AppendHandler(Arg.Any<IApiHttpRequestHandler>());
                handlerMock.Received(1).AppendHandler(handler1);
                handlerMock.Received(1).AppendHandler(handler2);
                handlerMock.Received(1).AppendHandler(handler3);
            };
        }

        private static Action ArrangeNext(IApiHttpRequestHandler handlerMock, ApiHttpRequestHandlerBinding binding)
        {
            var handlerBinding = Substitute.For<IApiHttpRequestHandlerBinding>();
            var handler = Substitute.For<IApiHttpRequestHandler>();

            handlerBinding.Build().Returns(handler);

            binding.Next = handlerBinding;

            return () =>
            {
                handlerBinding.Received(1).Build();

                handlerMock.Received(1).SetNextHandler(Arg.Any<IApiHttpRequestHandler>());
                handlerMock.Received(1).SetNextHandler(handler);
            };
        }

        #endregion Methods
    }
}