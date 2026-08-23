using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shared.DTO;

namespace ElectronicShopApp.Services.Interfaces
{
    public interface IUserListItemService
    {
        // existing methods
        Task<IEnumerable<UserListItemDTO>> GetUserListItems(int userId, int userListId);
        Task<bool> AddUserListItem(UserListItemDTO userListItem);
        Task<bool> RemoveUserListItem(int itemId);
        Task<bool> UpdateUserListItem(UserListItemDTO userListItem);

        // convenience methods used by ViewModels
        Task<IEnumerable<UserListItemDetailDTO>> GetAllUserListItems(int listId);
        Task DeleteUserListItems(int userId, int listId, IEnumerable<UserListItemDTO> userListItemDtos);
    }
}
