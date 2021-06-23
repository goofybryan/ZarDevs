using NSubstitute;
using Xunit;

namespace ZarDevs.Http.Client.Tests
{
    public class ApiHttpRequestHandlerBindingMapTests
    {
        [Theory]
        [InlineData(null, null, true)]
        [InlineData("", "", true)]
        [InlineData("", null, false)]
        [InlineData(1, "1", false)]
        [InlineData(1, 1, true)]
        public void TrySetAndGet_Exceute_SetsAndReturns(object key1, object key2, bool same)
        {
            // Arrange
            var map = new ApiHttpRequestHandlerBindingMap();
            var binding1 = Substitute.For<IApiHttpRequestHandlerBinding>();
            var binding2 = Substitute.For<IApiHttpRequestHandlerBinding>();

            // Act
            map.TryAdd(key1, binding1);
            map.TryAdd(key2, binding2);

            var result1 = map.TryGet(key1);
            var result2 = map.TryGet(key2);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);

            if(same)
            {
                Assert.Same(result1, result2);
                Assert.NotSame(binding1, result1);
                Assert.Same(binding2, result1);
            }
            else
            {
                Assert.NotSame(result1, result2);
                Assert.Same(binding1, result1);
                Assert.Same(binding2, result2);
            }
        }
    }
}