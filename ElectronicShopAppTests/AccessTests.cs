using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ElectronicShopAPI;
using ElectronicShopAppTests.Support;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.DTO;

namespace ElectronicShopAppTests;

public class AccessTests
{
    [Theory]
    [InlineData(ShopApi.Email)]
    [InlineData(ShopApi.Phone)]
    public async Task LoginToken_CanReadOwnProfile(string login)
    {
        using var api = new ShopApi();
        using var client = api.CreateBuyerClient();
        await ShopApi.Login(client, login);
        var profile = await client.GetFromJsonAsync<CustomerDTO>("/api/Customer/Profile");
        Assert.Equal(ShopApi.Buyer, profile!.Id);
    }

    [Theory]
    [InlineData(ShopApi.Email, "incorrect")]
    [InlineData("missing@example.test", ShopApi.Password)]
    public async Task InvalidCredentials_ReturnNoToken(string login, string password)
    {
        using var api = new ShopApi();
        using var client = api.CreateBuyerClient();
        var response = await client.PostAsJsonAsync("/api/Customer/Login", new { login, password });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.False((await response.Content.ReadFromJsonAsync<JsonElement>()).TryGetProperty("token", out _));
    }

    [Theory]
    [InlineData("anonymous", HttpStatusCode.Unauthorized)]
    [InlineData("invalid-token", HttpStatusCode.Unauthorized)]
    [InlineData("buyer", HttpStatusCode.Forbidden)]
    [InlineData("editor", HttpStatusCode.OK)]
    public async Task ModifyProducts_RequiresPermission(string identity, HttpStatusCode expected)
    {
        using var api = new ShopApi();
        using var client = api.CreateBuyerClient();
        const int id = 60001;
        api.WithDatabase(db => { db.Products.Add(new() { Id = id, Name = "Mouse", Price = 15m }); db.SaveChanges(); });
        if (identity == "buyer") await ShopApi.Login(client);
        if (identity == "invalid-token") client.DefaultRequestHeaders.Authorization = new("Bearer", "invalid");
        if (identity == "editor")
        {
            var token = AuthorizationFnc.GenerateToken(123, "editor", "User",
                api.Services.GetRequiredService<IConfiguration>(), "ModifyProducts");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await client.PutAsJsonAsync("/api/Products/ModifyProducts",
            new[] { new ProductDTO { Id = id, Name = "Mouse", Price = 20m } });
        Assert.Equal(expected, response.StatusCode);
        api.WithDatabase(db => Assert.Equal(identity == "editor" ? 20m : 15m, db.Products.Find(id)!.Price));
    }
}
