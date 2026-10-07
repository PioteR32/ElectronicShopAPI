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
    public class FilterOptionsServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly FilterOptionsService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public FilterOptionsServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new FilterOptionsService(_httpClient);
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
        public async Task GetOptions_ShouldReturnOptions_WhenSuccessful()
        {
            // Arrange
            var options = new List<FilterOptionDTO>
            {
                new FilterOptionDTO { Id = 1, Name = "Option 1" }
            };
            var json = JsonSerializer.Serialize(options, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().EndsWith("/filtersOptions")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetOptions();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task AddOptions_ShouldReturnIds_WhenSuccessful()
        {
            // Arrange
            var options = new[] { new FilterOptionDTO { Name = "New Opt" } };
            var ids = new List<int> { 10 };
            var json = JsonSerializer.Serialize(ids, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.AddOptions(options);

            // Assert
            Assert.Single(result);
        }
    }
}
