using Shared.DTO;
using System.Threading.Tasks;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface ICustomersService
	{
		Task<bool> CreateUser(CustomerDTO dto);
		Task<string?> LogIn(string login, string password);
		Task<CustomerDTO?> ChangeUserNames(string firstName, string lastName);
		Task<CustomerDTO?> ChangeUserPassword(string password);
		Task<CustomerDTO?> GetProfile();
	}
}
