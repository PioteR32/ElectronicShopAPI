using System.Security.Claims;
using ElectronicShopAPI.Controllers;
using ElectronicShopAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace ElectronicShopAppTests;

public class ShoppingCartsControllerTests
{
    private sealed class CartService : IShoppingCartService
    {
        public TaskCompletionSource<IEnumerable<ShoppingCartItemDTO>> Completion { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        public int? UserId { get; private set; }
        public string? Search { get; private set; }
        public Task<IEnumerable<ShoppingCartItemDTO>> FindItemsInShoppingCart(int userId, string find)
        {
            UserId = userId;
            Search = find;
            return Completion.Task;
        }
        public Task<IEnumerable<object>> GetUserShoppingCart(int userId) => throw new NotSupportedException();
        public Task AddItemsToShoppingCart(int userId, OrderItemDTO[] items) => throw new NotSupportedException();
        public Task RemoveItemsFromShoppingCart(int userId, OrderItemDTO[] items) => throw new NotSupportedException();
        public Task ModifyQuantityItemsInShoppingCart(int userId, OrderItemDTO[] items) => throw new NotSupportedException();
    }

    private static ShoppingCartsController Controller(CartService service, string? userId = "42") => new(service)
    {
        ControllerContext = new()
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(userId is null ? [] :
                    new[] { new Claim(ClaimTypes.NameIdentifier, userId) }, "Test"))
            }
        }
    };

    [Fact]
    public async Task Find_AwaitsServiceAndReturnsItemsForAuthenticatedCustomer()
    {
        var service = new CartService();
        var pending = Controller(service).FindItemsInShoppingCart("Laptop");
        Assert.False(pending.IsCompleted);
        ShoppingCartItemDTO[] items = [new() { CustomerId = 42, ProductId = 1, Quantity = 2 }];
        service.Completion.SetResult(items);

        var result = Assert.IsType<OkObjectResult>(await pending);
        Assert.Same(items, result.Value);
        Assert.Equal(42, service.UserId);
        Assert.Equal("Laptop", service.Search);
    }

    [Fact]
    public async Task Find_AsyncServiceFailureReturnsBadRequest()
    {
        var service = new CartService();
        var pending = Controller(service).FindItemsInShoppingCart("Laptop");
        service.Completion.SetException(new InvalidOperationException("Database unavailable"));
        Assert.IsType<BadRequestObjectResult>(await pending);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("invalid")]
    public async Task Find_InvalidIdentityDoesNotCallService(string? userId)
    {
        var service = new CartService();
        Assert.IsType<BadRequestObjectResult>(await Controller(service, userId).FindItemsInShoppingCart("Laptop"));
        Assert.Null(service.UserId);
    }
}
