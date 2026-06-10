using ElectronicShopAPI.Data;
using ElectronicShopAPI.DTO;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAPI.Services
{
	public interface IUserListItemService
	{
		Task<IEnumerable<UserListItemDetailDTO>> GetAllUserListItems(int userId, int listId);
		Task AddUserListItems(int userId, int listId, IEnumerable<UserListItemDTO> userListItemDtos);
		Task DeleteUserListItems(int userId, int listId, IEnumerable<UserListItemDTO> userListItemDtos);
		Task ModifyUserListItemsQuantity(int userId, int listId, UserListItemDTO userListItemDTO);
	}

	public class UserListItemService : IUserListItemService
	{
		private readonly DataContext _context;

		public UserListItemService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<UserListItemDetailDTO>> GetAllUserListItems(int userId, int listId)
		{
			if (!_context.UserLists.Any(ul => ul.Id == listId && ul.CustomerId == userId))
			{
				throw new KeyNotFoundException("User list not found or does not belong to the user");
			}

			var items = await _context.UserListItems
				.Where(c => c.UserListId == listId)
				.Join(_context.Products, uli => uli.ProductId, p => p.Id, (uli, p) => new { Uli = uli, P = p })
				.ToListAsync();

			return items.Select(x => UserListItemDetailDTO.FromModels(x.Uli, x.P));
		}

		public async Task AddUserListItems(int userId, int listId, IEnumerable<UserListItemDTO> userListItemDtos)
		{
			if (!_context.UserLists.Any(ul => ul.Id == listId && ul.CustomerId == userId))
			{
				throw new KeyNotFoundException("User list not found or does not belong to the user");
			}

			if (userListItemDtos.Any(uli => uli.Quantity <= 0))
			{
				throw new ArgumentException("Invalid quantity for one or more items");
			}

			// Validate product existence
			foreach (var dto in userListItemDtos)
			{
				if (await _context.Products.FindAsync(dto.ProductId) == null)
					throw new KeyNotFoundException("ProductId is bad one of the provided items");
			}
			var grouped = userListItemDtos.GroupBy(i => new { i.ProductId, i.UserListId });
			var reapatedItems = grouped.Where(g => g.Count() > 1);
			var notReapetedItems = grouped.Where(g => g.Count() == 1).SelectMany(g => g);

			if (reapatedItems.Any())
			{
				var notReapetedItemsList = notReapetedItems.ToList();
				foreach (var group in reapatedItems)
				{
					group.First().Quantity = group.Sum(i => i.Quantity);
					notReapetedItemsList.Add(group.First());
				}
				notReapetedItems = notReapetedItemsList;
			}
			
			var userListItems = notReapetedItems.Select(dto => new UserListItem() { UserListId = listId, ProductId = dto.ProductId, Quantity = dto.Quantity }).ToList();
			
			_context.UserListItems.AddRange(userListItems);

			await _context.SaveChangesAsync();
		}

		public async Task DeleteUserListItems(int userId, int listId, IEnumerable<UserListItemDTO> userListItemDtos)
		{
			if (!_context.UserLists.Any(ul => ul.Id == listId && ul.CustomerId == userId))
			{
				throw new KeyNotFoundException("User list not found or does not belong to the user");
			}

			if (userListItemDtos.Any(uli => uli.Quantity <= 0))
			{
				throw new ArgumentException("Invalid quantity for one or more items");
			}

			foreach (var dto in userListItemDtos)
			{
				if (await _context.Products.FindAsync(dto.ProductId) == null)
					throw new KeyNotFoundException("ProductId is bad one of the provided items");
			}

			var userListItems = userListItemDtos.Select(dto => new UserListItem() { UserListId = listId, ProductId = dto.ProductId, Quantity = dto.Quantity }).ToList();
			_context.UserListItems.RemoveRange(userListItems);
			await _context.SaveChangesAsync();
		}

		public async Task ModifyUserListItemsQuantity(int userId, int listId, UserListItemDTO userListItemDTO)
		{
			if (!_context.UserLists.Any(ul => ul.Id == listId && ul.CustomerId == userId))
			{
				throw new KeyNotFoundException("User list not found or does not belong to the user");
			}

			if (userListItemDTO.Quantity <= 0)
			{
				throw new ArgumentException("Invalid quantity for one or more items");
			}

			var userListItem = await _context.UserListItems.FirstOrDefaultAsync(uli => uli.UserListId == listId && uli.ProductId == userListItemDTO.ProductId);
			if (userListItem == null)
			{
				throw new KeyNotFoundException("Item not found in the user list");
			}

			userListItem.Quantity = userListItemDTO.Quantity;
			_context.UserListItems.Update(userListItem);
			await _context.SaveChangesAsync();
		}
	}
}
