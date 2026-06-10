using ElectronicShopAPI.Data;
using ElectronicShopAPI.DTO;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAPI.Services
{
	public interface IShoppingCartService
	{
		Task<IEnumerable<object>> GetUserShoppingCart(int userId);
		Task AddItemsToShoppingCart(int userId, OrderItemDTO[] items);
		Task RemoveItemsFromShoppingCart(int userId, OrderItemDTO[] items);
		Task ModifyQuantityItemsInShoppingCart(int userId, OrderItemDTO[] items);
	}

	public class ShoppingCartService : IShoppingCartService
	{
		private readonly DataContext _context;

		public ShoppingCartService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<object>> GetUserShoppingCart(int userId)
		{
			var shoppingCarts = _context.ShoppingCarts.Where(sc => sc.CustomerId == userId)
				.Join(_context.Products, sc => sc.ProductId, p => p.Id, (sc, p) => new { ShoppingCart = sc, Product = p });
			var shoppingCartItems = await shoppingCarts.Select(sc => new ShoppingCartItemDTO
			{
				ShoppingCartId = sc.ShoppingCart.Id,
				CustomerId = sc.ShoppingCart.CustomerId,
				ProductId = sc.ShoppingCart.ProductId,
				Product = ProductDTO.FromModel(sc.Product),
				Quantity = sc.ShoppingCart.Quantity
			}).ToListAsync();
			return shoppingCartItems;
		}

		public async Task AddItemsToShoppingCart(int userId, OrderItemDTO[] items)
		{
			if (items == null || items.Any(i => i.ProductId < 0) || items.Any(i => i.Quantity <= 0))
			{
				throw new ArgumentException("Invalid item data");
			}

			var shoppingCartItems = new List<ShoppingCart>();
			foreach (var i in items)
			{
				shoppingCartItems.Add(new ShoppingCart() { CustomerId = userId, ProductId = i.ProductId, Quantity = i.Quantity });
			}

			_context.ShoppingCarts.AddRange(shoppingCartItems);
			await _context.SaveChangesAsync();
		}

		public async Task RemoveItemsFromShoppingCart(int userId, OrderItemDTO[] items)
		{
			if (items == null || items.Any(i => i.ProductId < 0) || items.Any(i => i.Quantity <= 0))
			{
				throw new ArgumentException("Invalid item data");
			}

			var itemsToRemove = _context.ShoppingCarts.Where(sc => sc.CustomerId == userId && items.Any(si => si.ProductId == sc.ProductId));
			_context.ShoppingCarts.RemoveRange(itemsToRemove);
			await _context.SaveChangesAsync();
		}

		public async Task ModifyQuantityItemsInShoppingCart(int userId, OrderItemDTO[] items)
		{
			if (items == null || items.Any(i => i.ProductId < 0) || items.Any(i => i.Quantity <= 0))
			{
				throw new ArgumentException("Invalid item data");
			}

			var itemsToUpdate = _context.ShoppingCarts.Where(sc => sc.CustomerId == userId && items.Any(si => si.ProductId == sc.ProductId));
			foreach (var shoppingCartItem in itemsToUpdate)
			{
				var itemToUpdate = items.FirstOrDefault(i => i.ProductId == shoppingCartItem.ProductId);
				if (itemToUpdate != null)
				{
					shoppingCartItem.Quantity = itemToUpdate.Quantity;
				}
				else
				{
					throw new KeyNotFoundException("Item not found in shopping cart");
				}
			}

			_context.ShoppingCarts.UpdateRange(itemsToUpdate);
			await _context.SaveChangesAsync();
		}
	}
}
