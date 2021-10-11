using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using ZarDevs.Http.Api.Serializers;

namespace ZarDevs.Http.Api.Tests.Serializers
{
    public class ApiCommandContentSerializerFormUrlEncodedTests
    {
        #region Methods

        [Fact]
        public void IsValidFor_WithInvalidMediaTypes_ReturnsFalse()
        {
            // Arrange
            var mediaTypes = new List<string> { "Invalid", "Media", "Types" };
            var serializer = new ApiCommandContentSerializerFormUrlEncoded(Array.Empty<IDefaultFormUrlEncodedContentParser>());

            // Act & Assert
            foreach (var mediaType in mediaTypes)
            {
                Assert.False(serializer.IsValidFor(mediaType));
            }
        }

        [Fact]
        public void IsValidFor_WithValidMediaTypes_ReturnsTrue()
        {
            // Arrange
            var mediaTypes = HttpContentType.FormUrlEncoded;
            var serializer = new ApiCommandContentSerializerFormUrlEncoded(Array.Empty<IDefaultFormUrlEncodedContentParser>());

            // Act & Assert
            foreach (var mediaType in mediaTypes)
            {
                Assert.True(serializer.IsValidFor(mediaType));
            }
        }

        [Fact]
        public void Serialize_WithParserReturnfalse_ThrowsException()
        {
            // Arrange
            var request = Substitute.For<IApiCommandRequest>();
            var parser = Substitute.For<IDefaultFormUrlEncodedContentParser>();
            var parser2 = Substitute.For<IDefaultFormUrlEncodedContentParser>();
            var mediaTypes = new List<string> { "Invalid", "Media", "Types" };
            var serializer = new ApiCommandContentSerializerFormUrlEncoded(new[] { parser, parser2 });

            parser.TryParse(Arg.Any<object>(), out Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(info => false);
            parser2.TryParse(Arg.Any<object>(), out Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(info => false);

            request.HasContent.Returns(true);
            request.Content.Returns(new object());

            // Act
            Assert.Throws<NotSupportedException>(() => serializer.Serialize(request));

            // Assert
            _ = parser.Received(1).TryParse(Arg.Any<object>(), out Arg.Any<IEnumerable<KeyValuePair<string, string>>>());
            _ = parser2.Received(1).TryParse(Arg.Any<object>(), out Arg.Any<IEnumerable<KeyValuePair<string, string>>>());

            _ = request.Received(1).HasContent;
            _ = request.Received(3).Content;
        }

        [Fact]
        public async void Serialize_WithParserReturnTrue_ReturnsFormEncodedContent()
        {
            // Arrange
            object value = "value";
            IDictionary<string, string> kv = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } };
            var request = Substitute.For<IApiCommandRequest>();
            var parser = Substitute.For<IDefaultFormUrlEncodedContentParser>();
            var mediaTypes = new List<string> { "Invalid", "Media", "Types" };
            var serializer = new ApiCommandContentSerializerFormUrlEncoded(new[] { parser });

            parser.TryParse(Arg.Any<object>(), out Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(info => { info[1] = kv; return true; });

            request.HasContent.Returns(true);
            request.Content.Returns(value);

            // Act
            var result = serializer.Serialize(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FormUrlEncodedContent>(result);
            var encodedResult = (FormUrlEncodedContent)result;
            var content = await encodedResult.ReadAsStringAsync();

            Assert.Equal("Key1=Value1&Key2=Value2", content);

            _ = parser.Received(1).TryParse(Arg.Any<object>(), out Arg.Any<IEnumerable<KeyValuePair<string, string>>>());

            _ = request.Received(1).HasContent;
            _ = request.Received(1).Content;
        }

        #endregion Methods
    }
}