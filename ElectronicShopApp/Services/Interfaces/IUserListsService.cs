using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicShopApp.ViewModels.DTOs;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface IUserListsService
	{
		Task<IEnumerable<UserListDTO>> GetUserLists();
		Task<bool> AddUserList(UserListDTO userList);
		Task<bool> RemoveUserLists(List<int> userRemoveListIds);
	}
}
