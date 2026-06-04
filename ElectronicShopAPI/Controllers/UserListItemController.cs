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
	public class UserListItemController:ControllerBase
	{
		private readonly DataContext _context;
		public UserListItemController(DataContext context)
		{
			_context = context;
		}
		[Authorize(Policy = "Read")]
		[HttpGet]
		public async Task<IActionResult> GetAllUserListItems(int listId)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				if(!_context.UserLists.Any(ul => ul.Id == listId && ul.CustomerId == userId))
				{
					return BadRequest(new { message = "User list not found or does not belong to the user" });
				}

				var userListItems = await _context.UserListItems.Where(c => c.UserListId == listId)
					.Join(_context.Products, uli => uli.ProductId, p => p.Id, (uli, p) => new { UserListItem = uli, Product = p })
					.ToListAsync();

				return Ok(userListItems);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to retrieve user list items", error = ex.Message });
			}
		}


		[Authorize(Policy = "Write")]
		[HttpPost]
		public async Task<IActionResult> AddUserListItems(int listId, [FromBody] IEnumerable<UserListItemDTO> userListItemDtos)
		{
			try
			{
				List<UserListItem> userListItems = new List<UserListItem>();
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				
				if(!_context.UserLists.Any(ul => ul.Id == listId && ul.CustomerId == userId))
				{
					return BadRequest(new { message = "User list not found or does not belong to the user" });
				}
				if(userListItemDtos.Any(uli => uli.Quantity <= 0))
				{
					return BadRequest(new { message = "Invalid quantity for one or more items" });
				}
				if(userListItemDtos.Any(uli => _context.Products.Find(uli.ProductId) == null))
				{
					return BadRequest(new { message = "ProductId is bad one of the provided items" });
				}

				foreach (var userListItemDto in userListItemDtos)
				{
					userListItems.Add(new UserListItem() { UserListId = listId, ProductId = userListItemDto.ProductId, Quantity = userListItemDto.Quantity });
				}

				_context.UserListItems.AddRange(userListItems);
				await _context.SaveChangesAsync();

				return Ok(true);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear(); // Clear the change tracker to avoid issues with failed transactions
				return BadRequest(new { message = "Failed to retrieve user list items", error = ex.Message });
			}
		}


		[Authorize(Policy = "Delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteUserListItems(int listId, [FromBody] IEnumerable<UserListItemDTO> userListItemDtos)
		{
			try
			{
				List<UserListItem> userListItems = new List<UserListItem>();
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				
				if(!_context.UserLists.Any(ul => ul.Id == listId && ul.CustomerId == userId))
				{
					return BadRequest(new { message = "User list not found or does not belong to the user" });
				}
				if(userListItemDtos.Any(uli => uli.Quantity <= 0))
				{
					return BadRequest(new { message = "Invalid quantity for one or more items" });
				}
				if(userListItemDtos.Any(uli => _context.Products.Find(uli.ProductId) == null))
				{
					return BadRequest(new { message = "ProductId is bad one of the provided items" });
				}

				foreach (var userListItemDto in userListItemDtos)
				{
					userListItems.Add(new UserListItem() { UserListId = listId, ProductId = userListItemDto.ProductId, Quantity = userListItemDto.Quantity });
				}

				_context.UserListItems.RemoveRange(userListItems);
				await _context.SaveChangesAsync();

				return Ok(true);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear(); // Clear the change tracker to avoid issues with failed transactions
				return BadRequest(new { message = "Failed to retrieve user list items", error = ex.Message });
			}
		}

		[Authorize(Policy = "Write")]
		[HttpPost]
		public async Task<IActionResult> ModifyUserListItemsQuantity(int listId,[FromBody] UserListItemDTO userListItemDTO)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				
				if(!_context.UserLists.Any(ul => ul.Id == listId && ul.CustomerId == userId))
				{
					return BadRequest(new { message = "User list not found or does not belong to the user" });
				}
				if(userListItemDTO.Quantity <= 0)
				{
					return BadRequest(new { message = "Invalid quantity for one or more items" });
				}

				UserListItem? userListItem = await _context.UserListItems.FirstOrDefaultAsync(uli => uli.UserListId == listId && uli.ProductId == userListItemDTO.ProductId);
				if (userListItem == null)
				{
					return BadRequest(new { message = "Item not found in the user list" });
				}

				userListItem.Quantity = userListItemDTO.Quantity;

				_context.UserListItems.Update(userListItem);
				await _context.SaveChangesAsync();

				return Ok(true);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear(); // Clear the change tracker to avoid issues with failed transactions
				return BadRequest(new { message = "Failed to retrieve user list items", error = ex.Message });
			}
		}
	}
}
