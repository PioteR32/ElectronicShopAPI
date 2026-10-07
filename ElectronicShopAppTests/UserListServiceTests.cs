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
    public class UserListServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly UserListService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserListServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new UserListService(_httpClient);
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
        public async Task GetUserLists_ShouldReturnLists_WhenSuccessful()
        {
            // Arrange
            var lists = new List<UserListDTO>
            {
                new UserListDTO { Id = 1, Name = "List 1" }
            };
            var json = JsonSerializer.Serialize(lists, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Contains("/GetUserLists")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetUserLists();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task AddUserList_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var list = new UserListDTO { Name = "New List" };
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post && r.RequestUri.ToString().Contains("/AddList")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.AddUserList(list);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveUserLists_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var ids = new List<int> { 1, 2 };
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Delete && r.RequestUri.ToString().Contains("/DeleteLists")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.RemoveUserLists(ids);

            // Assert
            Assert.True(result);
        }
    }
}
