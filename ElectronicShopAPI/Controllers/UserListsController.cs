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
	public class UserListsController: ControllerBase
	{
		private readonly IUserListService _userListService;
		public UserListsController(IUserListService userListService)
		{
			_userListService = userListService;
		}
		[Authorize(Policy = "Read")]
		[HttpGet]
		public async Task<IActionResult> GetAllUserLists()
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				var userLists = await _userListService.GetAllUserLists(userId);

				return Ok(userLists);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to retrieve users lists", error = ex.Message });
			}
		}
		[Authorize(Policy = "Write")]
		[HttpPost]
		public async Task<IActionResult> AddAllUserList([FromBody] UserListDTO userListDto)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				await _userListService.AddUserList(userId, userListDto);
				return Ok(true);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add user list", error = ex.Message });
			}
		}

		[Authorize(Policy = "Write")]
		[HttpPatch]
		public async Task<IActionResult> ModifyUserList(int listId, [FromBody] UserListDTO userListDto)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				await _userListService.ModifyUserList(userId, listId, userListDto);
				return Ok(true);
			}
			catch (KeyNotFoundException knf)
			{
				return BadRequest(new { message = knf.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add user list", error = ex.Message });
			}
		}

		[Authorize(Policy = "Delete")]
		[HttpDelete]
		public async Task<IActionResult> DeleteUserList(int listId)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				await _userListService.DeleteUserList(userId, listId);
				return Ok(true);
			}
			catch (KeyNotFoundException knf)
			{
				return BadRequest(new { message = knf.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add user list", error = ex.Message });
			}
		}
	}
}
