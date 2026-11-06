using ElectronicShopAPI.Models;
using ElectronicShopAPI.Services.Classes;
using ElectronicShopAppTests.Support;
using Shared.DTO;

namespace ElectronicShopAppTests;

public class ShoppingCartServiceTests
{
    [Theory]
    [InlineData("add")]
    [InlineData("remove")]
    [InlineData("modify")]
    public async Task InvalidBatch_IsRejectedWithoutChangingCart(string operation)
    {
        foreach (var invalid in new OrderItemDTO[]?[]
        {
            null,
            [new() { ProductId = -1, Quantity = 1 }],
            [new() { ProductId = 1, Quantity = 0 }],
            [new() { ProductId = 1, Quantity = -1 }],
            [new() { ProductId = 1, Quantity = 3 }, new() { ProductId = 2, Quantity = 0 }]
        })
        {
            var database = new TestDatabase();
            await using var db = database.Open();
            db.ShoppingCarts.Add(new() { CustomerId = 10, ProductId = 1, Quantity = 2 });
            await db.SaveChangesAsync();
            var service = new ShoppingCartService(db);

            await Assert.ThrowsAsync<ArgumentException>(() => operation switch
            {
                "add" => service.AddItemsToShoppingCart(10, invalid!),
                "remove" => service.RemoveItemsFromShoppingCart(10, invalid!),
                _ => service.ModifyQuantityItemsInShoppingCart(10, invalid!)
            });

            await using var verification = database.Open();
            Assert.Equal(2, Assert.Single(verification.ShoppingCarts).Quantity);
        }
    }

    [Fact]
    public async Task AddItems_PersistsEveryItemForRequestedCustomer()
    {
        var database = new TestDatabase();
        await using var db = database.Open();
        await new ShoppingCartService(db).AddItemsToShoppingCart(10,
            [new() { ProductId = 1, Quantity = 2 }, new() { ProductId = 2, Quantity = 3 }]);

        await using var verification = database.Open();
        Assert.Collection(verification.ShoppingCarts.OrderBy(x => x.ProductId),
            item => { Assert.Equal(10, item.CustomerId); Assert.Equal(1, item.ProductId); Assert.Equal(2, item.Quantity); },
            item => { Assert.Equal(10, item.CustomerId); Assert.Equal(2, item.ProductId); Assert.Equal(3, item.Quantity); });
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ChangeCart_DoesNotChangeAnotherCustomersItems(bool remove)
    {
        var database = new TestDatabase();
        await using var db = database.Open();
        db.ShoppingCarts.AddRange(
            new() { Id = 1, CustomerId = 10, ProductId = 1, Quantity = 2 },
            new() { Id = 2, CustomerId = 20, ProductId = 1, Quantity = 7 },
            new() { Id = 3, CustomerId = 10, ProductId = 2, Quantity = 4 });
        await db.SaveChangesAsync();
        var service = new ShoppingCartService(db);
        OrderItemDTO[] request = [new() { ProductId = 1, Quantity = 5 }];

        if (remove) await service.RemoveItemsFromShoppingCart(10, request);
        else await service.ModifyQuantityItemsInShoppingCart(10, request);

        await using var verification = database.Open();
        Assert.Equal(7, verification.ShoppingCarts.Single(x => x.Id == 2).Quantity);
        Assert.Equal(4, verification.ShoppingCarts.Single(x => x.Id == 3).Quantity);
        if (remove) Assert.DoesNotContain(verification.ShoppingCarts, x => x.Id == 1);
        else Assert.Equal(5, verification.ShoppingCarts.Single(x => x.Id == 1).Quantity);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Laptop")]
    [InlineData("portable")]
    [InlineData("missing")]
    public async Task FindItems_FiltersByCustomerAndProductText(string? search)
    {
        await using var db = new TestDatabase().Open();
        db.Products.Add(new() { Id = 1, Name = "Laptop", Description = "portable computer", Price = 123.45m });
        // The other customer's row ID deliberately equals the requested customer ID.
        db.ShoppingCarts.AddRange(
            new() { Id = 90, CustomerId = 10, ProductId = 1, Quantity = 2 },
            new() { Id = 10, CustomerId = 20, ProductId = 1, Quantity = 7 });
        await db.SaveChangesAsync();

        var result = await new ShoppingCartService(db).FindItemsInShoppingCart(10, search!);

        if (search == "missing") Assert.Empty(result);
        else
        {
            var item = Assert.Single(result);
            Assert.Equal(10, item.CustomerId);
            Assert.Equal(90, item.ShoppingCartId);
            Assert.Equal(2, item.Quantity);
            Assert.Equal("Laptop", item.Product.Name);
            Assert.Equal(123.45m, item.Product.Price);
        }
    }
}
