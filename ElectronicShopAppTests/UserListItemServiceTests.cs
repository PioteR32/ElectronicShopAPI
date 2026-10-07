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
    public class UserListItemServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly UserListItemService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserListItemServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new UserListItemService(_httpClient);
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
        public async Task GetAllUserListItems_ShouldReturnItems_WhenSuccessful()
        {
            // Arrange
            var items = new List<UserListItemDetailDTO>
            {
                new UserListItemDetailDTO { Id = 1, ProductName = "Detail 1" }
            };
            var json = JsonSerializer.Serialize(items, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Contains("/listId=")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetAllUserListItems(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task AddUserListItem_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var item = new UserListItemDTO { ProductName = "New Item" };
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post && r.RequestUri.ToString().Contains("/AddUserListItem")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.AddUserListItem(item);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveUserListItem_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Delete && r.RequestUri.ToString().Contains("/RemoveUserListItem/1")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.RemoveUserListItem(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUserListItem_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var item = new UserListItemDTO { Id = 1, ProductName = "Updated" };
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Put),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.UpdateUserListItem(item);

            // Assert
            Assert.True(result);
        }
    }
}
