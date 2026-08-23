using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ElectronicShopApp.Services.Interfaces
{
	public interface IUserListsService
	{
		Task<IEnumerable<UserListDTO>> GetUserLists();
        Task<IEnumerable<CheckedUserListDTO>> GetCheckedUserLists(int productId);
        Task<IEnumerable<CheckedUserListDTO>> MenageUserListsProduct(int productId, IEnumerable<CheckedUserListDTO> checkedUserLists);

        Task<bool> AddUserList(UserListDTO userList);
		Task<bool> RemoveUserLists(List<int> userRemoveListIds);
	}
}
