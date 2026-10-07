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
using Shared.DTO.Filters;
using Xunit;

namespace ElectronicShopAppTests
{
    public class ProductsServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly ProductsService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductsServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new ProductsService(_httpClient);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        [Fact]
        public async Task GetProducts_ShouldReturnProducts_WhenSuccessful()
        {
            // Arrange
            var products = new List<ProductDTO>
            {
                new ProductDTO { Id = 1, Name = "Product 1" },
                new ProductDTO { Id = 2, Name = "Product 2" }
            };
            var json = JsonSerializer.Serialize(products, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Contains("/1-10")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetProducts(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenFound()
        {
            // Arrange
            var product = new ProductDTO { Id = 1, Name = "Product 1" };
            var json = JsonSerializer.Serialize(product, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().EndsWith("/1")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetProductById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetProductSpecsById_ShouldReturnSpecs_WhenSuccessful()
        {
            // Arrange
            var specs = new List<ProductSpecificationDTO>
            {
                new ProductSpecificationDTO { Id = 1, Name = "Spec 1" }
            };
            var json = JsonSerializer.Serialize(specs, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().EndsWith("/GetProductSpecsById/1")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetProductSpecsById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
