using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shared.DTO;

namespace ElectronicShopApp.Services.Interfaces
{
    public interface IUserListService
    {
        Task<IEnumerable<UserListDTO>> GetUserLists();
        Task<IEnumerable<CheckedUserListDTO>> GetCheckedUserLists(int productId);
        Task<IEnumerable<CheckedUserListDTO>> ManageUserListsProduct(int productId, IEnumerable<CheckedUserListDTO> checkedUserLists);
        Task<bool> AddUserList(UserListDTO userList);
        Task<bool> RemoveUserLists(List<int> userRemoveListIds);
        Task<bool> DeleteUserListItems(int userId, int userListId, IEnumerable<int> itemIds);
    }
}
