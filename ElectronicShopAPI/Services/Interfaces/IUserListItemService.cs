using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface IUserListItemService
	{
		Task<IEnumerable<UserListItemDetailDTO>> GetAllUserListItems(int userId, int listId);
		Task AddUserListItems( IEnumerable<UserListItemDTO> userListItemsDto);
		Task AddUserListItem( UserListItemDTO userListItemsDto);
		Task DeleteUserListItems(int userId, int listId, IEnumerable<UserListItemDTO> userListItemDtos);
		Task ModifyUserListItemsQuantity(int userId, int listId, UserListItemDTO userListItemDTO);
	}
}
