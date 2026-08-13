using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Authorization;
using Shared.DTO;
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
		[HttpGet("GetUserLists")]
		public async Task<IActionResult> GetAllUserLists()
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				var userLists = await _userListService.GetAllUserLists(userId);

				return Ok(userLists);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to retrieve users lists", error = ex.Message });
			}
		}
		[Authorize(Policy = "Write")]
		[HttpPost("AddList")]
		public async Task<IActionResult> AddAllUserList([FromBody] UserListDTO userListDto)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				var id = await _userListService.AddUserList(userId, userListDto);
				return Ok(id);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add user list", error = ex.Message });
			}
		}
        
        [Authorize(Policy = "Read")]
		[HttpGet("GetAllUserListWithCheckedProduct/{productId}")]
		public async Task<IActionResult> GetAllUserListWithCheckedProduct(int productId)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				var id = await _userListService.GetAllUserListWithCheckedProduct(productId, userId);
				return Ok(id);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add user list", error = ex.Message });
			}
		}
        [Authorize(Policy = "Write")]
        [HttpPost("MenageUserListsProduct/{productId}")]
        public async Task<IActionResult> MenageUserListsProduct(int productId, [FromBody] IEnumerable<CheckedUserListDTO> checkedLists)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var id = await _userListService.MenageUserListsProduct(checkedLists, productId);
            return Ok(id);
        }

        [Authorize(Policy = "Write")]
		[HttpPatch]
		public async Task<IActionResult> ModifyUserList(int listId, [FromBody] UserListDTO userListDto)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				var id = await _userListService.ModifyUserList(userId, listId, userListDto);
				return Ok(id);
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
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				var id = await _userListService.DeleteUserList(userId, listId);
				return Ok(id);
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
		[HttpDelete("DeleteLists")]
		public async Task<IActionResult> DeleteUserLists([FromBody] List<int> listIds)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
				var id = await _userListService.DeleteUserLists(userId, listIds);
				
				return Ok(id);
               
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
