using NSubstitute;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace ZarDevs.Http.Api.Tests.Serializers
{
    public class ApiCommandContentSerializerStringTests
    {
        #region Methods

        [Theory]
        [InlineData("<SerializerTestClass><Key1>Value1</Key1><Key2>2</Key2></SerializerTestClass>")]
        [InlineData(1)]
        [InlineData(2.2)]
        [InlineData(true)]
        public async Task Deserialize_TypeString_ReturnsDeserializedObject<T>(T value)
        {
            // Arrange
            var serializer = new ApiCommandContentSerializerString();
            var content = new StringContent(value.ToString(), Encoding.ASCII, HttpContentType.Json[0]);

            // Act
            var result = await serializer.DeserializeAsync<T>(content);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<T>(result);
            Assert.Equal(value, result);
        }

        [Fact]
        public void IsValidFor_WithInvalidMediaTypes_ReturnsFalse()
        {
            // Arrange
            var mediaTypes = new List<string> { "Invalid", "Media", "Types" };
            var serializer = new ApiCommandContentSerializerString();

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
            var mediaTypes = HttpContentType.Txt;
            var serializer = new ApiCommandContentSerializerString();

            // Act & Assert
            foreach (var mediaType in mediaTypes)
            {
                Assert.True(serializer.IsValidFor(mediaType));
            }
        }

        [Fact]
        public async Task Serialize_WithContant_ReturnsJsonContent()
        {
            // Arrange
            var serializer = new ApiCommandContentSerializerString();

            SerializerTestClass value = new() { Key1 = "Value1", Key2 = 2 };
            var request = Substitute.For<IApiCommandRequest>();
            request.HasContent.Returns(true);
            request.Content.Returns(value);

            // Act
            var result = serializer.Serialize(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<StringContent>(result);
            var content = await result.ReadAsStringAsync();

           Assert.Equal("Key1=Value1,Key2=2", content);

            _ = request.Received(1).HasContent;
            _ = request.Received(1).Content;
        }

        [Fact]
        public void Serialize_WithNoContant_ReturnsNull()
        {
            // Arrange
            var serializer = new ApiCommandContentSerializerString();

            var request = Substitute.For<IApiCommandRequest>();
            request.HasContent.Returns(false);

            // Act
            var result = serializer.Serialize(request);

            // Assert
            Assert.Null(result);

            _ = request.Received(1).HasContent;
            _ = request.Received(0).Content;
        }

        #endregion Methods
    }
}
