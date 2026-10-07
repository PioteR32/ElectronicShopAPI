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
    public class CustomersServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly CustomersService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public CustomersServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new CustomersService(_httpClient);
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
        public async Task CreateUser_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var dto = new CustomerDTO { Email = "test@test.com", Password = "password" };
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.CreateUser(dto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task LogIn_ShouldReturnToken_WhenSuccessful()
        {
            // Arrange
            var loginDto = new LoginDTO { Login = "user", Password = "password" };
            var responseData = new { token = "valid_token" };
            var json = JsonSerializer.Serialize(responseData, _jsonOptions);
            
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post && r.RequestUri.ToString().Contains("/Login")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.LogIn("user", "password");

            // Assert
            Assert.Equal("valid_token", result);
        }

        [Fact]
        public async Task GetProfile_ShouldReturnProfile_WhenSuccessful()
        {
            // Arrange
            var profile = new CustomerDTO { Email = "profile@test.com" };
            var json = JsonSerializer.Serialize(profile, _jsonOptions);
            _handlerMock.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().EndsWith("/Profile")),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

        // Note: Global.ApplyAuthHeader is called in GetProfile. 
        // Since we're using a mock handler, as long as we don't care about the header in the assertion, it's fine.
            // However, in a real test, we might want to check if the header is set if Global is used.

            // Act
            var result = await _service.GetProfile();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("profile@test.com", result.Email);
        }
    }
}
