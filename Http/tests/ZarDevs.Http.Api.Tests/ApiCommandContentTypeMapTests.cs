using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ZarDevs.Http.Api.Tests
{
    public class ApiCommandContentTypeMapTests
    {
        #region Methods

        [Fact]
        public void Indexor_WithExistingMediaType_ExepctedBehaviour()
        {
            // Arrange
            const string mediaType = "IWantThis";
            var apiContent = CreateContentMocks("Test1", "Test2", mediaType);
            var map = new ApiCommandContentTypeMap<IApiCommandContent>(apiContent.Values.ToArray());

            // Act
            var contentType = map[mediaType];
            var contentType2 = map[mediaType];

            // Assert
            Assert.NotNull(contentType);
            Assert.NotNull(contentType2);
            Assert.Same(contentType, contentType2);

            Assert.Same(apiContent[mediaType], contentType);

            contentType.Received(1).IsValidFor(Arg.Any<string>());
        }

        [Fact]
        public void Indexor_WithNotExistingMediaType_ThrowsException()
        {
            // Arrange
            const string mediaType = "DoesNotExist";
            var apiContent = CreateContentMocks("Test1", "Test2");
            var map = new ApiCommandContentTypeMap<IApiCommandContent>(apiContent.Values.ToArray());

            // Act
            var exception = Assert.Throws<ApiCommandContentTypeNotFoundException>(() => _ = map[mediaType]);

            // Assert
            Assert.Equal(mediaType, exception.MediaType);

            foreach (var content in apiContent)
            {
                content.Value.Received(1).IsValidFor(Arg.Any<string>());
            }
        }

        [Fact]
        public void TryGetSerializer_WithExistingMediaType_ExepctedBehaviour()
        {
            // Arrange
            const string mediaType = "IWantThis";
            var apiContent = CreateContentMocks("Test1", "Test2", mediaType);
            var map = new ApiCommandContentTypeMap<IApiCommandContent>(apiContent.Values.ToArray());

            // Act
            var found = map.TryGetSerializer(mediaType, out var contentType);
            var found2 = map.TryGetSerializer(mediaType, out var contentType2);

            // Assert
            Assert.NotNull(contentType);
            Assert.NotNull(contentType2);
            Assert.Same(contentType, contentType2);

            Assert.True(found);
            Assert.True(found2);

            Assert.Same(apiContent[mediaType], contentType);

            contentType.Received(1).IsValidFor(Arg.Any<string>());
        }

        [Fact]
        public void TryGetSerializer_WithNotExistingMediaType_ThrowsException()
        {
            // Arrange
            const string mediaType = "DoesNotExist";
            var apiContent = CreateContentMocks("Test1", "Test2");
            var map = new ApiCommandContentTypeMap<IApiCommandContent>(apiContent.Values.ToArray());

            // Act
            var found = map.TryGetSerializer(mediaType, out var contentType);

            // Assert
            Assert.Null(contentType);
            Assert.False(found);

            foreach (var content in apiContent)
            {
                content.Value.Received(1).IsValidFor(Arg.Any<string>());
            }
        }

        private static IDictionary<string, IApiCommandContent> CreateContentMocks(params string[] mediaTypes)
        {
            var mediaTypeContent = new Dictionary<string, IApiCommandContent>();

            foreach (var mediaType in mediaTypes)
            {
                var contentType = Substitute.For<IApiCommandContent>();
                contentType.IsValidFor(mediaType).Returns(true);
                mediaTypeContent.Add(mediaType, contentType);
            }

            return mediaTypeContent;
        }

        #endregion Methods
    }

    internal class ApiCommandContentTypeMock : IApiCommandContent
    {
        #region Constructors

        public ApiCommandContentTypeMock(params string[] mediaTypes)
        {
            MediaTypes = mediaTypes?.Length > 0 ? new List<string>(mediaTypes) : Array.Empty<string>();
        }

        #endregion Constructors

        #region Properties

        public int IsValidForCallCount { get; set; }
        public IList<string> MediaTypes { get; }

        #endregion Properties

        #region Methods

        public bool IsValidFor(string mediaType)
        {
            IsValidForCallCount++;
            return MediaTypes.Contains(mediaType);
        }

        #endregion Methods
    }
}