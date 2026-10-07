using System;
using System.Collections.Generic;
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
    public class OrdersServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly OrdersService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public OrdersServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new OrdersService(_httpClient);
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
        public async Task CreateOrder_ShouldReturnOrderId_WhenSuccessful()
        {
            // Arrange
            var dto = new OrderDTO { CustomerId = 1 };
            var responseData = new { orderId = 123 };
            var json = JsonSerializer.Serialize(responseData, _jsonOptions);
            
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post && r.RequestUri.ToString().Contains("/CreateOrder")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.CreateOrder(dto);

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnNull_WhenError()
        {
            // Arrange
            var dto = new OrderDTO { CustomerId = 1 };
            _handlerMock.Setup(h => h.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.CreateOrder(dto);

            // Assert
            Assert.Null(result);
        }
    }
}
