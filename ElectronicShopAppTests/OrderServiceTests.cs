using ElectronicShopAPI.Models;
using ElectronicShopAPI.Services.Classes;
using ElectronicShopAppTests.Support;
using Shared.DTO;

namespace ElectronicShopAppTests;

public class OrderServiceTests
{
    private static OrderDTO Order(params OrderItemDTO[] items) => new()
    {
        Email = "buyer@example.test", Street = "Testowa", House = "1", Town = "warszawa",
        TotalAmount = 0.01m, OrderItemDTO = items.ToList()
    };

    [Fact]
    public async Task GuestOrder_UsesStoredPricesAndDecreasesStock()
    {
        using var database = new OrderDatabase();
        await using var db = database.Open();
        db.Products.AddRange(
            new() { Id = 1, Name = "Mouse", Price = 12.50m, NumOnStock = 5 },
            new() { Id = 2, Name = "Keyboard", Price = 30m, NumOnStock = 2 });
        await db.SaveChangesAsync();

        var id = await new OrderService(db).CreateOrder(Order(
            new() { ProductId = 1, Quantity = 2 }, new() { ProductId = 2, Quantity = 2 }), null);

        await using var verification = database.Open();
        var saved = Assert.Single(verification.Orders);
        Assert.Equal(id, saved.Id);
        Assert.Equal(85m, saved.TotalAmount);
        Assert.Equal("buyer@example.test", saved.Email);
        Assert.Equal("Testowa", saved.Street);
        Assert.Equal("1", saved.House);
        Assert.Equal(Assert.Single(verification.Towns).Id, saved.TownId);
        Assert.Collection(verification.OrderItems.OrderBy(x => x.ProductId),
            item => { Assert.Equal(id, item.OrderId); Assert.Equal(12.50m, item.Price); Assert.Equal(2, item.Quantity); },
            item => { Assert.Equal(id, item.OrderId); Assert.Equal(30m, item.Price); Assert.Equal(2, item.Quantity); });
        Assert.Equal(3, verification.Products.Single(x => x.Id == 1).NumOnStock);
        Assert.Equal(0, verification.Products.Single(x => x.Id == 2).NumOnStock);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(2, 3)]
    public async Task InsufficientStock_DoesNotSaveOrderOrDecreaseOtherStock(int stock, int quantity)
    {
        using var database = new OrderDatabase();
        await using var db = database.Open();
        db.Products.AddRange(new() { Id = 1, Name = "Available", NumOnStock = 10 },
            new() { Id = 2, Name = "Unavailable", NumOnStock = stock });
        await db.SaveChangesAsync();
        // The relational model has a default stock of 300 for inserts with zero.
        db.Products.Single(p => p.Id == 2).NumOnStock = stock;
        await db.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() => new OrderService(db).CreateOrder(Order(
            new() { ProductId = 1, Quantity = 1 }, new() { ProductId = 2, Quantity = quantity }), null));

        await using var verification = database.Open();
        Assert.Empty(verification.Orders);
        Assert.Empty(verification.OrderItems);
        Assert.Equal(10, verification.Products.Single(x => x.Id == 1).NumOnStock);
        Assert.Equal(stock, verification.Products.Single(x => x.Id == 2).NumOnStock);
        Assert.Empty(verification.Towns);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task MissingTown_IsRejectedBeforeAnyWrite(string? town)
    {
        await using var db = new TestDatabase().Open();
        var order = Order(new OrderItemDTO { ProductId = 1, Quantity = 1 });
        order.Town = town;
        await Assert.ThrowsAsync<ArgumentException>(() => new OrderService(db).CreateOrder(order, null));
        Assert.Empty(db.Towns);
        Assert.Empty(db.Orders);
    }
}
