using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ElectronicShopApp.Services.Classes;
using ElectronicShopApp.Services.Interfaces;
using Moq;
using Moq.Protected;
using Shared.DTO;
using Xunit;

namespace ElectronicShopAppTests
{
    public class CategoriesServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly CategoriesService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public CategoriesServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            _service = new CategoriesService(_httpClient);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        [Fact]
        public async Task GetCategories_ShouldReturnCategories_WhenSuccessful()
        {
            // Arrange
            var categories = new List<CategoryDTO>
            {
                new CategoryDTO { Id = 1, Name = "Category 1" },
                new CategoryDTO { Id = 2, Name = "Category 2" }
            };
            var json = JsonSerializer.Serialize(categories, _jsonOptions);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            // Act
            var result = await _service.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Category 1", result.First().Name);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory_WhenFound()
        {
            // Arrange
            var category = new CategoryDTO { Id = 1, Name = "Category 1" };
            var json = JsonSerializer.Serialize(category, _jsonOptions);
            _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(r =>
            r.Method == HttpMethod.Get &&
            r.RequestUri != null &&
            r.RequestUri.ToString().EndsWith("/1")),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            // Act
            var result = await _service.GetCategoryById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Category 1", result.Name);
        }

        [Fact]
        public async Task AddCategories_ShouldReturnIds_WhenSuccessful()
        {
            // Arrange
            var categories = new[] { new CategoryDTO { Name = "New 1" }, new CategoryDTO { Name = "New 2" } };
            var ids = new List<int> { 1, 2 };
            var json = JsonSerializer.Serialize(ids, _jsonOptions);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post),
                It.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.AddCategories(categories);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DeleteCategories_ShouldReturnIds_WhenSuccessful()
        {
            // Arrange
            var ids = new[] { 1, 2 };
            var resultIds = new List<int> { 1, 2 };
            var json = JsonSerializer.Serialize(resultIds, _jsonOptions);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Delete),
                It.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.DeleteCategories(ids);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ModifyCategories_ShouldReturnIds_WhenSuccessful()
        {
            // Arrange
            var categories = new[] { new CategoryDTO { Id = 1, Name = "Modified" } };
            var ids = new List<int> { 1 };
            var json = JsonSerializer.Serialize(ids, _jsonOptions);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Put),
                It.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.ModifyCategories(categories);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnEmptyList_OnException()
        {
            // Arrange
            _handlerMock.Setup(h => h.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Network error"));

            // Act
            var result = await _service.GetCategories();

            // Assert
            Assert.Empty(result);
        }
    }
}

