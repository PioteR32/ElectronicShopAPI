using ElectronicShopApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace ElectronicShopAppTests
{
    public class ServicesTests : IDisposable
    {
        public ServicesTests()
        {
            // Reset Global state before each test
            Global.authorizeToken = string.Empty;
        }

        public void Dispose()
        {
            // Reset Global state after each test
            Global.authorizeToken = string.Empty;
        }

        [Fact]
        public void Global_Login_ShouldSetToken()
        {
            // Arrange
            string token = "test_token";

            // Act
            Global.Login(token);

            // Assert
            Assert.Equal(token, Global.authorizeToken);
        }

        [Fact]
        public void Global_Login_WithNull_ShouldNotChangeToken()
        {
            // Arrange
            string originalToken = "original";
            Global.Login(originalToken);

            // Act
            Global.Login(null);

            // Assert
            Assert.Equal(originalToken, Global.authorizeToken);
        }

        [Fact]
        public void Global_ApplyAuthHeader_ShouldSetHeader_WhenTokenExistsAndAuthNeeded()
        {
            // Arrange
            var client = new HttpClient();
            string token = "test_token";
            Global.Login(token);

            // Act
            Global.ApplyAuthHeader(client, true);

            // Assert
            Assert.NotNull(client.DefaultRequestHeaders.Authorization);
            Assert.Equal("Bearer", client.DefaultRequestHeaders.Authorization.Scheme);
            Assert.Equal(token, client.DefaultRequestHeaders.Authorization.Parameter);
        }

        [Fact]
        public void Global_ApplyAuthHeader_ShouldNotSetHeader_WhenTokenDoesNotExist()
        {
            // Arrange
            var client = new HttpClient();
            Global.Login(null);

            // Act
            Global.ApplyAuthHeader(client, true);

            // Assert
            Assert.Null(client.DefaultRequestHeaders.Authorization);
        }

        [Fact]
        public void Global_ApplyAuthHeader_ShouldClearHeader_WhenAuthNotNeeded()
        {
            // Arrange
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "some_token");
            Global.Login("some_token");

            // Act
            Global.ApplyAuthHeader(client, false);

            // Assert
            Assert.Null(client.DefaultRequestHeaders.Authorization);
        }

        [Fact]
        public void ServicesFnc_AddAuthHeaderIfPresent_ShouldSetHeader_WhenTokenExists()
        {
            // Arrange
            var client = new HttpClient();
            string token = "test_token";
            Global.Login(token);

            // Act
            ServicesFnc.AddAuthHeaderIfPresent(client);

            // Assert
            Assert.NotNull(client.DefaultRequestHeaders.Authorization);
            Assert.Equal("Bearer", client.DefaultRequestHeaders.Authorization.Scheme);
            Assert.Equal(token, client.DefaultRequestHeaders.Authorization.Parameter);
        }

        [Fact]
        public void ServicesFnc_AddAuthHeaderIfPresent_ShouldClearHeader_WhenNoToken()
        {
            // Arrange
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "old_token");
            Global.Login(null);

            // Act
            ServicesFnc.AddAuthHeaderIfPresent(client);

            // Assert
            Assert.Null(client.DefaultRequestHeaders.Authorization);
        }

        [Theory]
        [InlineData("key", new[] { "val1", "val2" }, "&key=val1&key=val2")]
        public void ServicesFnc_TableToUrlString_ShouldFormatCorrectly(string key, IEnumerable<object> values, string expected)
        {
            // Act
            var result = ServicesFnc.TableToUrlString(key, values);

            // Assert
            // Note: The implementation starts with & if the first element is processed. 
            // Actually, looking at the implementation: url += $"&{nameOfPropertie}={item}";
            // So for first item it becomes "&key=val1"
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ServicesFnc_TableToUrlString_ShouldReturnEmpty_WhenValuesAreNullOrEmpty()
        {
            Assert.Equal(string.Empty, ServicesFnc.TableToUrlString("key", Enumerable.Empty<string>()));
        }
    }
}