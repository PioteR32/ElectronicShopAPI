using ElectronicShopAPI.Models;
using ElectronicShopAPI.Services.Classes;
using ElectronicShopAppTests.Support;
using Shared;
using Shared.DTO;
using Shared.DTO.Filters;

namespace ElectronicShopAppTests;

public class ProductServiceTests
{
    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 2)]
    [InlineData(null, 1)]
    public async Task StockFilter_RespectsExplicitValueAndDefault(bool? inStock, int expectedId)
    {
        await using var db = new TestDatabase().Open();
        db.Products.AddRange(new() { Id = 1, Name = "Available", NumOnStock = 1 },
            new() { Id = 2, Name = "Sold out", NumOnStock = 0 });
        await db.SaveChangesAsync();

        var result = await new ProductService(db).GetFilterProducts(new() { IsInStock = inStock });

        Assert.Equal(expectedId, Assert.Single(result).Id);
    }

    [Theory]
    [InlineData(OrderByOptions.SortByPriceAsc, 2, 3)]
    [InlineData(OrderByOptions.SortByPriceDesc, 3, 2)]
    public async Task PriceRange_IncludesBoundariesAndSorts(OrderByOptions sort, int first, int second)
    {
        await using var db = new TestDatabase().Open();
        db.Products.AddRange(Enumerable.Range(1, 4).Select(i => new Product
        { Id = i, Name = $"Product {i}", Price = i * 10m, NumOnStock = 1 }));
        await db.SaveChangesAsync();

        var result = await new ProductService(db).GetFilterProducts(new()
        { MinPrice = 20m, MaxPrice = 30m, OrderByOptions = sort });

        Assert.Equal(new[] { first, second }, result.Select(x => x.Id));
    }

    [Fact]
    public async Task Pagination_IsAppliedAfterSorting()
    {
        await using var db = new TestDatabase().Open();
        db.Products.AddRange(Enumerable.Range(1, 5).Select(i => new Product
        { Id = i, Name = $"Product {i}", Price = (6 - i) * 10m, NumOnStock = 1 }));
        await db.SaveChangesAsync();

        var result = await new ProductService(db).GetFilterProducts(new()
        { PageIndex = 2, PageSize = 2, OrderByOptions = OrderByOptions.SortByPriceAsc });

        Assert.Equal(new[] { 3, 2 }, result.Select(x => x.Id));
    }

    [Fact]
    public async Task MissingProduct_ReturnsNull()
    {
        await using var db = new TestDatabase().Open();
        Assert.Null(await new ProductService(db).GetProductById(999));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BatchWithMissingProduct_DoesNotPartiallyModifyOrDelete(bool delete)
    {
        var database = new TestDatabase();
        await using var db = database.Open();
        db.Products.Add(new() { Id = 1, Name = "Original", Price = 20m, NumOnStock = 5 });
        await db.SaveChangesAsync();
        var service = new ProductService(db);

        if (delete) await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteProducts([1, 999]));
        else await Assert.ThrowsAsync<KeyNotFoundException>(() => service.ModifyProducts(
            [new() { Id = 1, Name = "Changed", Price = 1m }, new() { Id = 999, Name = "Missing" }]));

        await using var verification = database.Open();
        var saved = Assert.Single(verification.Products);
        Assert.Equal("Original", saved.Name);
        Assert.Equal(20m, saved.Price);
        Assert.Equal(5, saved.NumOnStock);
    }

    [Fact]
    public async Task AddAndModify_PersistProductFields()
    {
        var database = new TestDatabase();
        await using var db = database.Open();
        var service = new ProductService(db);
        var id = Assert.Single(await service.AddProducts(
            [new() { Name = "Mouse", Description = "Wireless", Price = 19.99m, NumOnStock = 4 }]));
        var added = await service.GetProductById(id);
        Assert.NotNull(added);
        Assert.Equal("Mouse", added.Name);
        Assert.Equal("Wireless", added.Description);
        Assert.Equal(19.99m, added.Price);
        Assert.Equal(4, added.NumOnStock);

        Assert.Equal(id, Assert.Single(await service.ModifyProducts(
            [new() { Id = id, Name = "Keyboard", Description = "Wired", Price = 50m, NumOnStock = 2 }])));

        await using var verification = database.Open();
        var saved = Assert.Single(verification.Products);
        Assert.Equal("Keyboard", saved.Name);
        Assert.Equal("Wired", saved.Description);
        Assert.Equal(50m, saved.Price);
        Assert.Equal(2, saved.NumOnStock);
    }
}
