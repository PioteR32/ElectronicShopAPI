using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Authorization;
using ElectronicShopAPI.DTO;
using ElectronicShopAPI.Data;
namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ShoppingCartsController: ControllerBase
	{
		private readonly DataContext _context;
		public ShoppingCartsController(DataContext context)
		{
			_context = context;
		}
		[Authorize(Policy = "Read")]
		[HttpGet]
		public async Task<IActionResult> GetUserShoppingCart()
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				var shoppingCarts = _context.ShoppingCarts.Where(sc => sc.CustomerId == userId)
					.Join(_context.Products, sc => sc.ProductId, p => p.Id, (sc, p) => new { ShoppingCart = sc, Product = p });
				return Ok(shoppingCarts);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to retrieve shopping cart", error = ex.Message });
			}
		}
		[Authorize(Policy = "Write")]
		[HttpPost]
		public async Task<IActionResult> AddItemsToShoppingCart([FromBody] OrderItemDTO[] item)
		{
			try
			{
				List<ShoppingCart> shoppingCartItems = new List<ShoppingCart>();
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

				if(item == null || item.Any(i => i.ProductId < 0) || item.Any(i => i.Quantity <= 0))
				{
					return BadRequest(new { message = "Invalid item data" });
				}

				foreach (var i in item)
				{
					shoppingCartItems.Add(new ShoppingCart() { CustomerId = userId, ProductId = i.ProductId, Quantity = i.Quantity });
				}
				
				_context.ShoppingCarts.AddRange(shoppingCartItems);

				await _context.SaveChangesAsync();

				return Ok(shoppingCartItems);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear();
				return BadRequest(new { message = "Failed to add item to shopping cart", error = ex.Message });
			}
		}

		[Authorize(Policy = "Delete")]
		[HttpPost]
		public async Task<IActionResult> RemoveItemsFromShoppingCart([FromBody] OrderItemDTO[] items)
		{
			try
			{
				List<ShoppingCart> shoppingCartItems = new List<ShoppingCart>();
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

				if(items == null || items.Any(i => i.ProductId < 0) || items.Any(i => i.Quantity <= 0))
				{
					return BadRequest(new { message = "Invalid item data" });
				}

				var itemsToRemove = _context.ShoppingCarts.Where(sc => sc.CustomerId == userId && items.Any(si => si.ProductId == sc.ProductId));
				_context.ShoppingCarts.RemoveRange(itemsToRemove);

				await _context.SaveChangesAsync();

				return Ok(shoppingCartItems);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear();
				return BadRequest(new { message = "Failed to remove item from shopping cart", error = ex.Message });
			}
		}
		[Authorize(Policy = "Write")]
		[HttpPost]
		public async Task<IActionResult> ModifyQuantityItemsInShoppingCart([FromBody] OrderItemDTO[] item)
		{
			try
			{
				List<ShoppingCart> shoppingCartItems = new List<ShoppingCart>();
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

				if(item == null || item.Any(i => i.ProductId < 0) || item.Any(i => i.Quantity <= 0))
				{
					return BadRequest(new { message = "Invalid item data" });
				}

				var itemsToUpdate = _context.ShoppingCarts.Where(sc => sc.CustomerId == userId && item.Any(si => si.ProductId == sc.ProductId));
				foreach (var shoppingCartItem in itemsToUpdate)
				{
					var itemToUpdate = item.FirstOrDefault(i => i.ProductId == shoppingCartItem.ProductId);
					if (itemToUpdate != null)
					{
						shoppingCartItem.Quantity = itemToUpdate.Quantity;
					}
					else
					{
						return BadRequest(new { message = "Item not found in shopping cart" });
					}
				}
				_context.ShoppingCarts.UpdateRange(itemsToUpdate);

				await _context.SaveChangesAsync();

				return Ok(shoppingCartItems);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear();
				return BadRequest(new { message = "Failed to remove item from shopping cart", error = ex.Message });
			}
		}
	}
}
