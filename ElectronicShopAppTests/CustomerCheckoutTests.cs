using System.Net;
using System.Net.Http.Json;
using ElectronicShopAPI.Data;
using ElectronicShopAPI.Services.Classes;
using ElectronicShopAppTests.Support;
using Shared.DTO;

namespace ElectronicShopAppTests;

public class CustomerCheckoutTests
{
    private const int Mouse = 65001, Cable = 65002;

    private static OrderDTO Request(int buyer) => new()
    {
        CustomerId = buyer, Email = ShopApi.Email, Street = "Polna", House = "1", Town = "Kraków",
        TotalAmount = 0.01m, OrderItemDTO = []
    };

    private static void Seed(DataContext db, bool insufficient = false)
    {
        db.Products.AddRange(new() { Id = Mouse, Name = "Mouse", Price = 30m, NumOnStock = 5 },
            new() { Id = Cable, Name = "Cable", Price = 8m, NumOnStock = 1 });
        db.ShoppingCarts.AddRange(new() { CustomerId = ShopApi.Buyer, ProductId = Mouse, Quantity = 2 },
            new() { CustomerId = 999, ProductId = Cable, Quantity = 1 });
        if (insufficient)
            db.ShoppingCarts.Add(new() { CustomerId = ShopApi.Buyer, ProductId = Cable, Quantity = 2 });
        db.SaveChanges();
    }

    [Fact]
    public async Task Checkout_PersistsOwnCartLinesAtCatalogPriceAndDecreasesStock()
    {
        var database = new TestDatabase();
        await using var db = database.Open();
        Seed(db);
        int id = await new OrderService(db).CreateOrder(Request(ShopApi.Buyer), ShopApi.Buyer.ToString());
        await using var saved = database.Open();
        var order = saved.Orders.Single(o => o.Id == id);
        Assert.Equal(ShopApi.Buyer, order.CustomerId);
        Assert.Equal(60m, order.TotalAmount);
        var line = Assert.Single(saved.OrderItems.Where(i => i.OrderId == id));
        Assert.Equal(Mouse, line.ProductId);
        Assert.Equal(2, line.Quantity);
        Assert.Equal(30m, line.Price);
        Assert.Equal(3, saved.Products.Find(Mouse)!.NumOnStock);
        Assert.Equal(1, saved.Products.Find(Cable)!.NumOnStock);
    }

    [Fact]
    public async Task Checkout_RejectsEntireCartWhenOneLineExceedsStock()
    {
        var database = new TestDatabase();
        await using var db = database.Open();
        Seed(db, insufficient: true);
        var error = await Record.ExceptionAsync(() =>
            new OrderService(db).CreateOrder(Request(ShopApi.Buyer), ShopApi.Buyer.ToString()));
        await using var saved = database.Open();
        Assert.Empty(saved.Orders);
        Assert.Empty(saved.OrderItems);
        Assert.Equal(5, saved.Products.Find(Mouse)!.NumOnStock);
        Assert.Equal(1, saved.Products.Find(Cable)!.NumOnStock);
        Assert.NotNull(error);
    }

    [Theory]
    [InlineData(ShopApi.Buyer, HttpStatusCode.OK)]
    [InlineData(999, HttpStatusCode.BadRequest)]
    public async Task CreateOrder_UsesAuthenticatedIdentity(int requestedBuyer, HttpStatusCode expected)
    {
        using var api = new ShopApi();
        using var client = api.CreateBuyerClient();
        api.WithDatabase(db => Seed(db));
        await ShopApi.Login(client);
        var response = await client.PostAsJsonAsync("/CreateOrder", Request(requestedBuyer));
        Assert.Equal(expected, response.StatusCode);
        api.WithDatabase(db =>
        {
            var orders = db.Orders.Where(o => o.CustomerId == requestedBuyer);
            if (expected == HttpStatusCode.OK) Assert.Equal(60m, Assert.Single(orders).TotalAmount);
            else Assert.Empty(orders);
        });
    }
}
