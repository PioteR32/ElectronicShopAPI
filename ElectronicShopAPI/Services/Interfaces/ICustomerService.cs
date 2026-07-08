using Shared.DTO;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface ICustomerService
	{
		Task CreateUser(CustomerDTO dto);
		Task<string?> LogIn(string login, string password);
		Task<CustomerDTO?> GetUser(int userId);
		Task<CustomerDTO?> GetUsers(int userId);
	}
}
