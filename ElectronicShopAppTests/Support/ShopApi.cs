using ElectronicShopAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElectronicShopAppTests.Support;

internal sealed class ShopApi : WebApplicationFactory<DataContext>
{
    private readonly string database = Guid.NewGuid().ToString();
    public const int Buyer = 50101;
    public const string Email = "buyer@example.test";
    public const string Phone = "501010101";
    public const string Password = "test-buyer-123";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<DataContext>>();
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(database));
        });
    }

    public void WithDatabase(Action<DataContext> action)
    {
        using var scope = Services.CreateScope();
        action(scope.ServiceProvider.GetRequiredService<DataContext>());
    }

    public HttpClient CreateBuyerClient()
    {
        var client = CreateClient();
        WithDatabase(db =>
        {
            db.Customers.Add(new() { Id = Buyer, FirstName = "Anna", LastName = "Test",
                Email = Email, PhoneNumber = Phone, Password = Password });
            db.SaveChanges();
        });
        return client;
    }

    public static async Task Login(HttpClient client, string login = Email)
    {
        using var response = await client.PostAsJsonAsync("/api/Customer/Login", new { login, password = Password });
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body.GetProperty("token").GetString());
    }
}
