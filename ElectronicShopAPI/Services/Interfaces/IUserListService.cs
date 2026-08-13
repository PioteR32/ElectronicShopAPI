using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface IUserListService
	{
		Task<IEnumerable<UserListDTO>> GetAllUserLists(int userId);
        Task<IEnumerable<CheckedUserListDTO>> GetAllUserListWithCheckedProduct(int productId, int userId);

        Task<IEnumerable<CheckedUserListDTO>> MenageUserListsProduct(IEnumerable<CheckedUserListDTO> checkedLists,
            int productId);
        Task<int> AddUserList(int userId, UserListDTO dto);
		Task<int> ModifyUserList(int userId, int listId, UserListDTO dto);
		Task<int> DeleteUserList(int userId, int listId);
		Task<int> DeleteUserLists(int userId, List<int> listIds);
	}
}
