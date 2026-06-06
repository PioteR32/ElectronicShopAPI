using ElectronicShopAPI.Data;
using ElectronicShopAPI.DTO;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAPI.Services
{
	public interface IUserListService
	{
		Task<IEnumerable<UserList>> GetAllUserLists(int userId);
		Task AddUserList(int userId, UserListDTO dto);
		Task ModifyUserList(int userId, int listId, UserListDTO dto);
		Task DeleteUserList(int userId, int listId);
	}

	public class UserListService : IUserListService
	{
		private readonly DataContext _context;

		public UserListService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<UserList>> GetAllUserLists(int userId)
		{
			return await _context.UserLists.Where(ul => ul.CustomerId == userId).ToListAsync();
		}

		public async Task AddUserList(int userId, UserListDTO dto)
		{
			if(_context.UserLists.Count(ul => ul.CustomerId == userId) >= 30)
			{
				throw new Exception("User cannot have more than 30 lists");
			}
			_context.UserLists.Add(new UserList() { CustomerId = userId, Name = dto.Name, Description = dto.Description });
			await _context.SaveChangesAsync();
		}

		public async Task ModifyUserList(int userId, int listId, UserListDTO dto)
		{
			var userList = await _context.UserLists.FirstOrDefaultAsync(ul => ul.Id == listId && ul.CustomerId == userId);
			if (userList == null) throw new KeyNotFoundException("User list not found or does not belong to the user");

			userList.Name = dto.Name;
			userList.Description = dto.Description;
			_context.UserLists.Update(userList);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteUserList(int userId, int listId)
		{
			var userList = await _context.UserLists.FirstOrDefaultAsync(ul => ul.Id == listId && ul.CustomerId == userId);
			if (userList == null) throw new KeyNotFoundException("User list not found or does not belong to the user");

			_context.UserLists.Remove(userList);
			await _context.SaveChangesAsync();
		}
	}
}
