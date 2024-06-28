using NSubstitute;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace ZarDevs.Http.Api.Tests.Serializers
{
    public class ApiCommandContentSerializerXmlTests
    {
        #region Methods

        [Fact]
        public async Task Deserialize_TypeSerializerTestClass_ReturnsDeserializedObject()
        {
            // Arrange
            var serializer = new ApiCommandContentSerializerXml();

            var content = new StringContent("<SerializerTestClass><Key1>Value1</Key1><Key2>2</Key2></SerializerTestClass>", Encoding.ASCII, HttpContentType.Json[0]);

            // Act
            var result = await serializer.DeserializeAsync<SerializerTestClass>(content);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SerializerTestClass>(result);
            Assert.Equal("Value1", result.Key1);
            Assert.Equal(2, result.Key2);
        }


        [Fact]
        public void IsValidFor_WithInvalidMediaTypes_ReturnsFalse()
        {
            // Arrange
            var mediaTypes = new List<string> { "Invalid", "Media", "Types" };
            var serializer = new ApiCommandContentSerializerXml();

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
            var mediaTypes = HttpContentType.Xml;
            var serializer = new ApiCommandContentSerializerXml();

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
            var serializer = new ApiCommandContentSerializerXml();

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

            XDocument expected = XDocument.Parse(content);

            var key1Element = expected.Root.Element("Key1");
            Assert.NotNull(key1Element);
            Assert.Equal("Value1", key1Element.Value);
            var key2Element = expected.Root.Element("Key2");
            Assert.NotNull(key2Element);
            Assert.Equal("2", key2Element.Value);

            _ = request.Received(1).HasContent;
            _ = request.Received(1).Content;
        }

        [Fact]
        public void Serialize_WithNoContant_ReturnsNull()
        {
            // Arrange
            var serializer = new ApiCommandContentSerializerXml();

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
