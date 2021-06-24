using NSubstitute;
using System;
using Xunit;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiCommandFactoryTests
    {
        #region Fields

        private const string _key = "Key";
        private const string _mediaType = "text/Test";

        #endregion Fields

        #region Methods

        [Fact]
        public void CreateDeleteCommand_Execute_ReturnsCommand()
        {
            CreateCommandTest(factory => factory.CreateDeleteCommand(_key), typeof(ApiDeleteCommandAsync), 0);
        }

        [Fact]
        public void CreateGetCommand_Execute_ReturnsCommand()
        {
            CreateCommandTest(factory => factory.CreateGetCommand(_key), typeof(ApiGetCommandAsync), 0);
        }

        [Fact]
        public void CreatePatchCommand_Execute_ReturnsCommand()
        {
            CreateCommandTest((factory, mediaType) => factory.CreatePatchCommand(mediaType, _key), typeof(ApiPatchCommandAsync));
        }

        [Fact]
        public void CreatePostCommand_Execute_ReturnsCommand()
        {
            CreateCommandTest((factory, mediaType) => factory.CreatePostCommand(mediaType, _key), typeof(ApiPostCommandAsync));
        }

        [Fact]
        public void CreatePutCommand_Execute_ReturnsCommand()
        {
            CreateCommandTest((factory, mediaType) => factory.CreatePutCommand(mediaType, _key), typeof(ApiPutCommandAsync));
        }

        [Fact]
        public void CreateRequest_Execute_RestursRequest()
        {
            // Arrange
            Uri uri = new("/", UriKind.Relative);
            object content = new();

            IApiHttpFactory httpFactory = Substitute.For<IApiHttpFactory>();
            IHttpResponseFactory responseFactory = Substitute.For<IHttpResponseFactory>();
            IApiCommandContentSerializer serializer = Substitute.For<IApiCommandContentSerializer>();
            IApiCommandContentTypeMap<IApiCommandContentSerializer> serializers = Substitute.For<IApiCommandContentTypeMap<IApiCommandContentSerializer>>();

            var factory = new ApiCommandFactory(httpFactory, responseFactory, serializers);

            // Act
            var request = factory.CreateRequest(uri, content);

            // Assert
            Assert.NotNull(request);
            Assert.Equal(uri, request.ApiUri);
            Assert.Same(content, request.Content);
        }

        [Fact]
        public void CreateSendCommand_Execute_ReturnsCommand()
        {
            CreateCommandTest((factory, mediaType) => factory.CreateSendCommand(mediaType, new System.Net.Http.HttpMethod("METHOD"), _key), typeof(ApiSendCommandAsync));
        }

        private void CreateCommandTest(Func<ApiCommandFactory, string, IApiCommandAsync> create, Type commandType)
        {
            // Act
            CreateCommandTest(factory => create(factory, _mediaType), commandType, 1);

            // Assert
        }

        private void CreateCommandTest(Func<ApiCommandFactory, IApiCommandAsync> create, Type commandType, int serializerMapCalls)
        {
            // Arrange
            IApiHttpFactory httpFactory = Substitute.For<IApiHttpFactory>();
            IHttpResponseFactory responseFactory = Substitute.For<IHttpResponseFactory>();
            IApiHttpClient httpClient = Substitute.For<IApiHttpClient>();
            IApiCommandContentSerializer serializer = Substitute.For<IApiCommandContentSerializer>();
            IApiCommandContentTypeMap<IApiCommandContentSerializer> serializers = Substitute.For<IApiCommandContentTypeMap<IApiCommandContentSerializer>>();

            serializers[_mediaType].Returns(serializer);
            httpFactory.NewClient(_key).Returns(httpClient);

            var factory = new ApiCommandFactory(httpFactory, responseFactory, serializers);

            // Act
            var command = create(factory);

            // Assert
            Assert.NotNull(command);
            Assert.IsType(commandType, command);

            httpFactory.Received(1).NewClient(Arg.Any<object>());
            httpFactory.Received(1).NewClient(_key);
            _ = serializers.Received(serializerMapCalls)[_mediaType];
        }

        #endregion Methods
    }
}