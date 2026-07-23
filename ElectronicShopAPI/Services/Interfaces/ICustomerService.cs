using Shared.DTO;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface ICustomerService
	{
		Task CreateUser(CustomerDTO dto);
		Task<string?> LogIn(string login, string password);
		Task<CustomerDTO?> ChangeUserNames(int id,string firstName, string lastName);
		Task<CustomerDTO?> ChangeUserPassword(int id,string newPassword);
		Task<CustomerDTO?> GetUser(int userId);
		Task<CustomerDTO?> GetUsers(int userId);
	}
}
