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
    public class CommentsServiceTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly CommentsService _service;
        private readonly JsonSerializerOptions _jsonOptions;

        public CommentsServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            _service = new CommentsService(_httpClient);
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
        public async Task GetCommentsForProduct_ShouldReturnComments_WhenSuccessful()
        {
            // Arrange
            var comments = new List<CommentDTO>
            {
                new CommentDTO { Id = 1, Comment = "Comment 1" },
                new CommentDTO { Id = 2, Comment = "Comment 2" }
            };
            var json = JsonSerializer.Serialize(comments, _jsonOptions);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetCommentsForProduct(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Comment 1", result.First().Comment);
        }

        [Fact]
        public async Task GetById_ShouldReturnComment_WhenFound()
        {
            // Arrange
            var comment = new CommentDTO { Id = 1, Comment = "Comment 1" };
            var json = JsonSerializer.Serialize(comment, _jsonOptions);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().EndsWith("/1")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Comment 1", result.Comment);
        }

        [Fact]
        public async Task Create_ShouldReturnId_WhenSuccessful()
        {
            // Arrange
            var dto = new CommentDTO { Comment = "New Comment" };
            var responseData = new { id = 10 };
            var json = JsonSerializer.Serialize(responseData, _jsonOptions);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.Create(dto);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task Update_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var dto = new CommentDTO { Comment = "Updated Comment" };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Put),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.Update(1, dto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Delete),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            var result = await _service.Delete(1);

            // Assert
            Assert.True(result);
        }
    }
}
