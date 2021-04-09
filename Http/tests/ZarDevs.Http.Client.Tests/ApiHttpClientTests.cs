using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using ZarDevs.Http.Tests.WebServer;

namespace ZarDevs.Http.Client.Tests
{
    public class ApiHttpClientTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        #region Fields

        private readonly WebApplicationFactory<Startup> _factory;

        #endregion Fields

        #region Constructors

        public ApiHttpClientTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public async Task DeleteAsync_ApiCall_ExpectedBehaviour()
        {
            // Arrange
            IApiHttpRequestHandler handlerMock = Substitute.For<IApiHttpRequestHandler>();
            var apiClient = CreatClient(handlerMock);

            int id = new Random().Next(-100000, 100000);

            // Act
            var response = await apiClient.DeleteAsync($"/Test/{id}");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var json = await response.Content.ReadAsStringAsync();
            Assert.Empty(json);
            Assert.True(TestFactory.Instance.IsDeleted(id));

            await handlerMock.Received(1).HandleAsync(Arg.Any<HttpRequestMessage>());
        }

        [Fact]
        public async Task GetAsync_ApiCall_ReturnsList()
        {
            // Arrange
            IApiHttpRequestHandler handlerMock = Substitute.For<IApiHttpRequestHandler>();
            var apiClient = CreatClient(handlerMock);

            // Act
            var response = await apiClient.GetAsync("/Test/");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var json = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(json);

            var result = JsonConvert.DeserializeObject<IList<Test>>(json);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(10, result.Count);

            Assert.All(result, test => Assert.NotNull(TestFactory.Instance.GetCreated(test.Id)));

            await handlerMock.Received(1).HandleAsync(Arg.Any<HttpRequestMessage>());
        }

        [Fact]
        public async Task GetAsync_ApiCallWithId_ReturnsObject()
        {
            // Arrange
            IApiHttpRequestHandler handlerMock = Substitute.For<IApiHttpRequestHandler>();
            var apiClient = CreatClient(handlerMock);

            int id = new Random().Next(-100000, -1);

            // Act
            var response = await apiClient.GetAsync($"/Test/{id}");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var json = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(json);

            var result = JsonConvert.DeserializeObject<Test>(json);
            Assert.NotNull(result);
            Assert.NotNull(TestFactory.Instance.GetCreated(result.Id));
            Assert.Equal(id, result.Id);

            await handlerMock.Received(1).HandleAsync(Arg.Any<HttpRequestMessage>());
        }

        [Fact]
        public async Task PatchAsync_ApiCall_ExpectedBehaviour()
        {
            // Arrange
            IApiHttpRequestHandler handlerMock = Substitute.For<IApiHttpRequestHandler>();
            var apiClient = CreatClient(handlerMock);

            int id = new Random().Next(-100000, 100000);
            Test content = new() { Id = id };

            // Act
            var response = await apiClient.PatchAsync($"/Test/{id}", CreateContent(content));

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var json = await response.Content.ReadAsStringAsync();
            Assert.Empty(json);
            Assert.NotNull(TestFactory.Instance.GetChanged(id));

            await handlerMock.Received(1).HandleAsync(Arg.Any<HttpRequestMessage>());
        }

        [Fact]
        public async Task PostAsync_ApiCall_ExpectedBehaviour()
        {
            // Arrange
            IApiHttpRequestHandler handlerMock = Substitute.For<IApiHttpRequestHandler>();
            var apiClient = CreatClient(handlerMock);

            int id = new Random().Next(-100000, 100000);
            Test content = new() { Id = id };

            // Act
            var response = await apiClient.PostAsync($"/Test/{id}", CreateContent(content));

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var json = await response.Content.ReadAsStringAsync();
            Assert.Empty(json);
            Assert.NotNull(TestFactory.Instance.GetUpdated(id));

            await handlerMock.Received(1).HandleAsync(Arg.Any<HttpRequestMessage>());
        }

        [Fact]
        public async Task PutAsync_ApiCall_ExpectedBehaviour()
        {
            // Arrange
            IApiHttpRequestHandler handlerMock = Substitute.For<IApiHttpRequestHandler>();
            var apiClient = CreatClient(handlerMock);

            int id = new Random().Next(-100000, 100000);
            Test content = new() { Id = id };

            // Act
            var response = await apiClient.PutAsync($"/Test/{id}", CreateContent(content));

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var json = await response.Content.ReadAsStringAsync();
            Assert.Empty(json);
            Assert.NotNull(TestFactory.Instance.GetAdded(id));

            await handlerMock.Received(1).HandleAsync(Arg.Any<HttpRequestMessage>());
        }

        private static HttpContent CreateContent<T>(T content) => JsonContent.Create(content);

        private ApiHttpClient CreatClient(IApiHttpRequestHandler handlerMock)
        {
            handlerMock.HandleAsync(Arg.Any<HttpRequestMessage>()).Returns(Task.CompletedTask);
            var httpClient = _factory.CreateClient();
            return new ApiHttpClient(handlerMock, httpClient);
        }

        #endregion Methods
    }
}