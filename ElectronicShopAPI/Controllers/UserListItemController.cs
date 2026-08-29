using ElectronicShopAPI.Models; // Path to your models
using ElectronicShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using System.Security.Claims;
// using ElectronicShopAPI.Data; // not needed, logic moved to service
namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserListItemController:ControllerBase
	{
		private readonly IUserListItemService _userListItemService;

		public UserListItemController(IUserListItemService userListItemService)
		{
			_userListItemService = userListItemService;
		}
		[Authorize(Policy = "Read")]
		[HttpGet("listId={listId}")]
		public async Task<IActionResult> GetAllUserListItems(int listId)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				var userListItems = await _userListItemService.GetAllUserListItems(userId, listId);
				return Ok(userListItems);
			}
			catch (KeyNotFoundException knf)
			{
				return BadRequest(new { message = knf.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add user list items", error = ex.Message });
			}
		}


		[Authorize(Policy = "Write")]
		[HttpPost]
		public async Task<IActionResult> AddUserListItems([FromBody] IEnumerable<UserListItemDTO> userListItemDtos)
		{
			var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
			try
			{
				await _userListItemService.AddUserListItems(userListItemDtos);
				return Ok(true);
			}
			catch (KeyNotFoundException knf)
			{
				return BadRequest(new { message = knf.Message });
			}
			catch (ArgumentException ae)
			{
				return BadRequest(new { message = ae.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to retrieve user list items", error = ex.Message });
			}
		}
       

		[Authorize(Policy = "Delete")]
		[HttpPost("delete")]
		public async Task<IActionResult> DeleteUserListItems([FromBody] IEnumerable<UserListItemDTO> userListItemDtos)
		{
			
			try
			{
                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                if (userListItemDtos == null || !userListItemDtos.Any())
					return BadRequest(new { message = "No user list items provided for deletion" });

				await _userListItemService.DeleteUserListItems(userId, (int)userListItemDtos.First().UserListId, userListItemDtos);
				return Ok(true);
			}
			catch (KeyNotFoundException knf)
			{
				return BadRequest(new { message = knf.Message });
			}
			catch (ArgumentException ae)
			{
				return BadRequest(new { message = ae.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to delete user list items", error = ex.Message });
			}
		}

		[Authorize(Policy = "Write")]
		[HttpPatch]
		public async Task<IActionResult> ModifyUserListItemsQuantity(int listId,[FromBody] UserListItemDTO userListItemDTO)
		{
			var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
			try
			{
				await _userListItemService.ModifyUserListItemsQuantity(userId, listId, userListItemDTO);
				return Ok(true);
			}
			catch (KeyNotFoundException knf)
			{
				return BadRequest(new { message = knf.Message });
			}
			catch (ArgumentException ae)
			{
				return BadRequest(new { message = ae.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to modify user list items", error = ex.Message });
			}
		}
	}
}
