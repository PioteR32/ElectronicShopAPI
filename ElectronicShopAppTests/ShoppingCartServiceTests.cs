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
    public class ShoppingCartServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly ShoppingCartService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public ShoppingCartServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new ShoppingCartService(_httpClient);
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
        public async Task GetCart_ShouldReturnCartItems_WhenSuccessful()
        {
            // Arrange
            var items = new List<ShoppingCartItemDTO>
            {
                new ShoppingCartItemDTO { Id = 1, ProductId = 10, Quantity = 2 }
            };
            var json = JsonSerializer.Serialize(items, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Contains("/GetCart/1")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetCart(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(10, result.First().ProductId);
        }
    }
}
