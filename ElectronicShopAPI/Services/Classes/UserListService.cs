using ElectronicShopAPI.Data;
using Shared.DTO;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAPI.Services.Classes
{
	public class UserListService : IUserListService
	{
		private readonly DataContext _context;

		public UserListService(DataContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<UserListDTO>> GetAllUserLists(int userId)
		{
			var lists = await _context.UserLists.Where(ul => ul.CustomerId == userId).ToListAsync();
			return lists.Select(ul => Extensions.Extensions.ToDto(ul));
		}

		public async Task<int> AddUserList(int userId, UserListDTO dto)
		{
			if (_context.UserLists.Count(ul => ul.CustomerId == userId) >= 30)
			{
			}

			var entity = new UserList() { CustomerId = userId, Name = dto.Name, Description = dto.Description };
			_context.UserLists.Add(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<int> ModifyUserList(int userId, int listId, UserListDTO dto)
		{
			var userList = await _context.UserLists.FirstOrDefaultAsync(ul => ul.Id == listId && ul.CustomerId == userId);
			if (userList == null) throw new KeyNotFoundException("User list not found or does not belong to the user");

			userList.Name = dto.Name;
			userList.Description = dto.Description;
			_context.UserLists.Update(userList);
			await _context.SaveChangesAsync();
			return userList.Id;
		}

		public async Task<int> DeleteUserList(int userId, int listId)
		{
			var userList = await _context.UserLists.FirstOrDefaultAsync(ul => ul.Id == listId && ul.CustomerId == userId);
			if (userList == null) throw new KeyNotFoundException("User list not found or does not belong to the user");

			_context.UserLists.Remove(userList);
			await _context.SaveChangesAsync();
			return listId;
		}
        public async Task<int> DeleteUserLists(int userId, List<int> listIds)
		{
			var userLists = await _context.UserLists.Where(ul => listIds.Contains(ul.Id) && ul.CustomerId == userId).ToListAsync();
			if (userLists == null || userLists.Count != listIds.Count) throw new KeyNotFoundException("One or more user lists not found or do not belong to the user");

			_context.UserLists.RemoveRange(userLists);
			await _context.SaveChangesAsync();
			return userLists.Count;
		}
	}
}
