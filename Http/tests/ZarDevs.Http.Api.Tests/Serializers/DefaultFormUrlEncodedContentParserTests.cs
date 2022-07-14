using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZarDevs.Http.Api.Serializers;

namespace ZarDevs.Http.Api.Tests.Serializers
{
    public class DefaultFormUrlEncodedContentParserTests
    {
        [Fact]
        public void TryParse_KeyValuePair_ReturnsSameValue()
        { 
            // Arrange
            var parser = new DefaultFormUrlEncodedContentParser();
            var list = new List<KeyValuePair<string, string>>();

            list.Add(KeyValuePair.Create("K1", "V1"));
            list.Add(KeyValuePair.Create("K2", "V2"));
            list.Add(KeyValuePair.Create("K3", "V3"));

            // Act
            var result = parser.TryParse(list, out var parsedResult);

            // Assert
            Assert.True(result);
            Assert.NotNull(parsedResult);
            Assert.Same(list, parsedResult);
            Assert.Equal(list, parsedResult);
        }

        [Fact]
        public void TryParse_NamedPair_ReturnsSameValue()
        {
            // Arrange
            var parser = new DefaultFormUrlEncodedContentParser();
            var list = new List<(string key, string value)>();

            list.Add(new ("K1", "V1"));
            list.Add(new ("K2", "V2"));
            list.Add(new ("K3", "V3"));

            // Act
            var result = parser.TryParse(list, out var parsedResult);

            // Assert
            Assert.True(result);
            Assert.NotNull(parsedResult);

            var parsedList = parsedResult.ToArray();
            Assert.Equal(list.Count, parsedList.Length);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(list[i].key, parsedList[i].Key);
                Assert.Equal(list[i].value, parsedList[i].Value);
            }
        }

        [Fact]
        public void TryParse_Tuple_ReturnsSameValue()
        {
            // Arrange
            var parser = new DefaultFormUrlEncodedContentParser();
            var list = new List<Tuple<string, string>>();

            list.Add(new("K1", "V1"));
            list.Add(new("K2", "V2"));
            list.Add(new("K3", "V3"));

            // Act
            var result = parser.TryParse(list, out var parsedResult);

            // Assert
            Assert.True(result);
            Assert.NotNull(parsedResult);

            var parsedList = parsedResult.ToArray();
            Assert.Equal(list.Count, parsedList.Length);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(list[i].Item1, parsedList[i].Key);
                Assert.Equal(list[i].Item2, parsedList[i].Value);
            }
        }

        [Fact]
        public void TryParse_ValueTuple_ReturnsSameValue()
        {
            // Arrange
            var parser = new DefaultFormUrlEncodedContentParser();
            var list = new List<ValueTuple<string, string>>();

            list.Add(new("K1", "V1"));
            list.Add(new("K2", "V2"));
            list.Add(new("K3", "V3"));

            // Act
            var result = parser.TryParse(list, out var parsedResult);

            // Assert
            Assert.True(result);
            Assert.NotNull(parsedResult);

            var parsedList = parsedResult.ToArray();
            Assert.Equal(list.Count, parsedList.Length);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(list[i].Item1, parsedList[i].Key);
                Assert.Equal(list[i].Item2, parsedList[i].Value);
            }
        }
    }
}
