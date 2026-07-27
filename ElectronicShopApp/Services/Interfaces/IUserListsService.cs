using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface IUserListsService
	{
		Task<IEnumerable<UserListDTO>> GetUserLists();
		Task<bool> AddUserList(UserListDTO userList);
	}
}
