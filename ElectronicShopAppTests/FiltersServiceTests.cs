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
using Shared.DTO.Filters;
using Xunit;

namespace ElectronicShopAppTests
{
    public class FiltersServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly FiltersService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public FiltersServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new FiltersService(_httpClient);
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
        public async Task GetFilters_ShouldReturnFilters_WhenSuccessful()
        {
            // Arrange
            var filters = new List<FilterDTO>
            {
                new FilterDTO { Id = 1, Name = "Filter 1" }
            };
            var json = JsonSerializer.Serialize(filters, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().EndsWith("/filters")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetFilters();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task AddFilters_ShouldReturnIds_WhenSuccessful()
        {
            // Arrange
            var filters = new[] { new FilterDTO { Name = "New Filter" } };
            var ids = new List<int> { 5 };
            var json = JsonSerializer.Serialize(ids, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post && r.RequestUri.ToString().Contains("/AddFilters")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.AddFilters(filters);

            // Assert
            Assert.Single(result);
        }
    }
}
