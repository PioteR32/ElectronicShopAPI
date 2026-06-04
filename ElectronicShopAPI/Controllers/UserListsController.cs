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
	public class UserListsController: ControllerBase
	{
		private readonly DataContext _context;
		public UserListsController(DataContext context)
		{
			_context = context;
		}
		[Authorize(Policy = "Read")]
		[HttpGet]
		public async Task<IActionResult> GetAllUserLists()
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				var userLists = await _context.UserLists.Where(ul => ul.CustomerId == userId).ToListAsync();

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
				_context.UserLists.Add(new UserList() { CustomerId = userId, Name = userListDto.Name, Description = userListDto.Description });

				await _context.SaveChangesAsync();	

				return Ok(true);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear();
				return BadRequest(new { message = "Failed to add user list", error = ex.Message });
			}
		}

		[Authorize(Policy = "Write")]
		[HttpPost]
		public async Task<IActionResult> ModifyUserList(int listId, [FromBody] UserListDTO userListDto)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				var userList = await _context.UserLists.FirstOrDefaultAsync(ul => ul.Id == listId && ul.CustomerId == userId);
				if (userList == null)
				{
					return BadRequest(new { message = "User list not found or does not belong to the user" });
				}

				userList.Name = userListDto.Name;
				userList.Description = userListDto.Description;
				_context.UserLists.Update(userList);
				await _context.SaveChangesAsync();	

				return Ok(true);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear();
				return BadRequest(new { message = "Failed to add user list", error = ex.Message });
			}
		}

		[Authorize(Policy = "Delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteUserList(int listId)
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				var userList = await _context.UserLists.FirstOrDefaultAsync(ul => ul.Id == listId && ul.CustomerId == userId);
				if (userList == null)
				{
					return BadRequest(new { message = "User list not found or does not belong to the user" });
				}

				_context.UserLists.Remove(userList);
				await _context.SaveChangesAsync();	

				return Ok(true);
			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear();
				return BadRequest(new { message = "Failed to add user list", error = ex.Message });
			}
		}
	}
}
