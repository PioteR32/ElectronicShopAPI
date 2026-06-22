using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Authorization;
using ElectronicShopAPI.DTO;
using ElectronicShopAPI.Data;
using ElectronicShopAPI.Services;
namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ShoppingCartsController: ControllerBase
	{
		private readonly IShoppingCartService _shoppingCartService;
		public ShoppingCartsController(IShoppingCartService shoppingCartService)
		{
			_shoppingCartService = shoppingCartService;
		}

		[Authorize(Policy = "Read")]
		[HttpGet("ShoppingCart")]
		public async Task<IActionResult> GetUserShoppingCart()
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				var shoppingCarts = await _shoppingCartService.GetUserShoppingCart(userId);
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
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				await _shoppingCartService.AddItemsToShoppingCart(userId, item);
				return Ok(true);
			}
			catch (ArgumentException ae)
			{
				return BadRequest(new { message = "Invalid item data", error = ae.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add item to shopping cart", error = ex.Message });
			}
		}

		[Authorize(Policy = "Delete")]
		[HttpDelete]
		public async Task<IActionResult> RemoveItemsFromShoppingCart([FromBody] OrderItemDTO[] items)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				await _shoppingCartService.RemoveItemsFromShoppingCart(userId, items);
				return Ok(true);
			}
			catch (ArgumentException ae)
			{
				return BadRequest(new { message = "Invalid item data", error = ae.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to remove item from shopping cart", error = ex.Message });
			}
		}
		[Authorize(Policy = "Write")]
		[HttpPatch]
		public async Task<IActionResult> ModifyQuantityItemsInShoppingCart([FromBody] OrderItemDTO[] item)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				await _shoppingCartService.ModifyQuantityItemsInShoppingCart(userId, item);
				return Ok(true);
			}
			catch (ArgumentException ae)
			{
				return BadRequest(new { message = "Invalid item data", error = ae.Message });
			}
			catch (KeyNotFoundException knf)
			{
				return BadRequest(new { message = "Item not found in shopping cart", error = knf.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to remove item from shopping cart", error = ex.Message });
			}
		}

		[Authorize(Policy = "Read")]
		[HttpGet("FindShoppingCart")]
		public async Task<IActionResult> FindItemsInShoppingCart([FromBody] string name)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				return Ok(_shoppingCartService.FindItemsInShoppingCart(userId, name)); 
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to retrieve shopping cart", error = ex.Message });
			}
		}

	}
}
