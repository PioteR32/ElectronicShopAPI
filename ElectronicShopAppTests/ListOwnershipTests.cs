using System.Net;
using System.Net.Http.Json;
using ElectronicShopAppTests.Support;
using Shared.DTO;

namespace ElectronicShopAppTests;

public class ListOwnershipTests
{
    private const int Own = 61001, Foreign = 61002, Product = 61003;

    private static async Task<HttpClient> Setup(ShopApi api)
    {
        var client = api.CreateBuyerClient();
        api.WithDatabase(db =>
        {
            db.UserLists.AddRange(new() { Id = Own, CustomerId = ShopApi.Buyer, Name = "Mine" },
                new() { Id = Foreign, CustomerId = 999, Name = "Other" });
            db.Products.Add(new() { Id = Product, Name = "Mouse" });
            foreach (var id in new[] { Own, Foreign })
                db.UserListItems.Add(new() { UserListId = id, ProductId = Product, Quantity = 2 });
            db.SaveChanges();
        });
        await ShopApi.Login(client);
        return client;
    }

    [Theory]
    [InlineData(Own, 5, HttpStatusCode.OK)]
    [InlineData(Foreign, 5, HttpStatusCode.BadRequest)]
    [InlineData(Own, 0, HttpStatusCode.BadRequest)]
    public async Task ChangeQuantity_RequiresOwnershipAndPositiveQuantity(int listId, int quantity, HttpStatusCode expected)
    {
        using var api = new ShopApi();
        using var client = await Setup(api);
        var response = await client.PatchAsJsonAsync($"/api/UserListItem?listId={listId}",
            new UserListItemDTO { UserListId = listId, ProductId = Product, Quantity = quantity });
        Assert.Equal(expected, response.StatusCode);
        api.WithDatabase(db =>
        {
            Assert.Equal(2, db.UserListItems.Single(i => i.UserListId == Foreign).Quantity);
            Assert.Equal(expected == HttpStatusCode.OK ? quantity : 2, db.UserListItems.Single(i => i.UserListId == Own).Quantity);
        });
    }

    [Fact]
    public async Task GetLists_ShowsOnlyOwnerAndDeniesReadingForeignItems()
    {
        using var api = new ShopApi();
        using var client = await Setup(api);
        var lists = await client.GetFromJsonAsync<List<UserListDTO>>("/api/UserLists/GetUserLists");
        Assert.Equal(Own, Assert.Single(lists!).Id);
        var items = await client.GetFromJsonAsync<List<UserListItemDetailDTO>>($"/api/UserListItem/listId={Own}");
        Assert.Equal("Mouse", Assert.Single(items!).ProductName);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.GetAsync($"/api/UserListItem/listId={Foreign}")).StatusCode);
    }

    [Fact]
    public async Task CannotSelectProductForAnotherCustomerByForgingOwnerInBody()
    {
        using var api = new ShopApi();
        using var client = await Setup(api);
        await client.PostAsJsonAsync($"/api/UserLists/MenageUserListsProduct/{Product}",
            new[] { new CheckedUserListDTO { Id = Foreign, CustomerId = 999, Name = "Other", IsChecked = false } });
        api.WithDatabase(db => Assert.Equal(2, Assert.Single(db.UserListItems.Where(i => i.UserListId == Foreign)).Quantity));
    }

    [Theory]
    [InlineData(Own, HttpStatusCode.OK)]
    [InlineData(Foreign, HttpStatusCode.BadRequest)]
    public async Task RenameList_ChecksOwner(int id, HttpStatusCode expected)
    {
        using var api = new ShopApi();
        using var client = await Setup(api);
        var response = await client.PatchAsJsonAsync($"/api/UserLists?listId={id}", new UserListDTO { Name = "Renamed" });
        Assert.Equal(expected, response.StatusCode);
        api.WithDatabase(db =>
        {
            Assert.Equal("Other", db.UserLists.Find(Foreign)!.Name);
            Assert.Equal(id == Own ? "Renamed" : "Mine", db.UserLists.Find(Own)!.Name);
        });
    }
}
