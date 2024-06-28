using NSubstitute;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ZarDevs.Http.Api.Tests.Serializers
{
    public class ApiCommandContentSerializerJsonTests
    {
        #region Methods

        [Fact]
        public async Task Deserialize_TypeSerializerTestClass_ReturnsDeserializedObject()
        {
            // Arrange
            var serializer = new ApiCommandContentSerializerJson(new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));
            var content = new StringContent("{\"key1\":\"Value1\",\"key2\":2}", Encoding.ASCII, HttpContentType.Json[0]);

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
            var serializer = new ApiCommandContentSerializerJson(new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

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
            var mediaTypes = HttpContentType.Json;
            var serializer = new ApiCommandContentSerializerJson(new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

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
            var serializer = new ApiCommandContentSerializerJson(new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

            SerializerTestClass value = new (){ Key1 = "Value1", Key2 = 2 };
            var request = Substitute.For<IApiCommandRequest>();
            request.HasContent.Returns(true);
            request.Content.Returns(value);

            // Act
            var result = serializer.Serialize(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<JsonContent>(result);
            var jsonContent = (JsonContent)result;
            var content = await jsonContent.ReadAsStringAsync();

            Assert.Equal("{\"key1\":\"Value1\",\"key2\":2}", content);

            _ = request.Received(1).HasContent;
            _ = request.Received(1).Content;
        }

        [Fact]
        public void Serialize_WithNoContant_ReturnsNull()
        {
            // Arrange
            var serializer = new ApiCommandContentSerializerJson(new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

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
