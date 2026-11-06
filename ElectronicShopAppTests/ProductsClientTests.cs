using System.Net;
using ElectronicShopApp.Services.Classes;

namespace ElectronicShopAppTests;

public class ProductsClientTests
{
    private sealed class Handler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(respond(request));
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(3, 25)]
    public async Task GetProducts_SendsRequestedPageAndReadsCamelCaseJson(int page, int size)
    {
        string? path = null;
        HttpMethod? method = null;
        using var client = new HttpClient(new Handler(request =>
        {
            path = request.RequestUri!.AbsolutePath;
            method = request.Method;
            return new(HttpStatusCode.OK) { Content = new StringContent(
                "[{\"id\":7,\"name\":\"Mouse\",\"description\":\"Wireless\",\"price\":19.99,\"numOnStock\":4}]") };
        }));

        var result = await new ProductsService(client).GetProducts(page, size);

        Assert.Equal($"/api/Products/{page}-{size}", path);
        Assert.Equal(HttpMethod.Get, method);
        var product = Assert.Single(result);
        Assert.Equal(7, product.Id);
        Assert.Equal("Mouse", product.Name);
        Assert.Equal("Wireless", product.Description);
        Assert.Equal(19.99m, product.Price);
        Assert.Equal(4, product.NumOnStock);
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.Unauthorized)]
    public async Task GetProducts_HttpFailureReturnsEmptyCollection(HttpStatusCode status)
    {
        using var client = new HttpClient(new Handler(_ => new(status)));
        Assert.Empty(await new ProductsService(client).GetProducts(1, 10));
    }

    [Theory]
    [InlineData("null")]
    [InlineData("not json")]
    [InlineData("[]")]
    public async Task GetProducts_EmptyOrInvalidPayloadReturnsEmptyCollection(string payload)
    {
        using var client = new HttpClient(new Handler(_ => new(HttpStatusCode.OK)
        { Content = new StringContent(payload) }));
        Assert.Empty(await new ProductsService(client).GetProducts(1, 10));
    }

    [Fact]
    public async Task GetProducts_NetworkFailureReturnsEmptyCollection()
    {
        using var client = new HttpClient(new Handler(_ => throw new HttpRequestException("Offline")));
        Assert.Empty(await new ProductsService(client).GetProducts(1, 10));
    }
}
