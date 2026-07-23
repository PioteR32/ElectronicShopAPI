using ElectronicShopAPI.Data;
using Shared.DTO;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAPI.Services.Classes
{
	public class CustomerService : ICustomerService
	{
		private readonly DataContext _context;
		private readonly IConfiguration _configuration;

		public CustomerService(DataContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		public async Task CreateUser(CustomerDTO dto)
		{
			if (_context.Customers.Any(c => c.Email == dto.Email && c.PhoneNumber == dto.PhoneNumber))
			{
				throw new InvalidOperationException("Email or Phone Number already exists");
			}

			await _context.Customers.AddAsync(new Customer
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Email = dto.Email,
				PhoneNumber = dto.PhoneNumber,
				Password = dto.Password
			});
			await _context.SaveChangesAsync();
		}

		public async Task<string?> LogIn(string login, string password)
		{
			if (login == "admin" && password == "password")
			{
				return AuthorizationFnc.GenerateToken(-1, login, "Admin", _configuration, Permission.AllPrivilages.ToString());
			}

			var customer = await _context.Customers.Where(c => (c.Email == login || c.PhoneNumber == login) && c.Password == password).FirstOrDefaultAsync();
			if (customer != null)
			{
				return AuthorizationFnc.GenerateToken(customer.Id, login, "User", _configuration, Permission.Read.ToString(), Permission.Write.ToString(), Permission.Delete.ToString());
			}

			return null;
		}

		public async Task<CustomerDTO?> GetUser(int userId)
		{
			var c = await _context.Customers.Where(cu => cu.Id == userId).FirstOrDefaultAsync();
			return c == null ? null : Extensions.Extensions.ToDto(c);
		}

		public async Task<CustomerDTO?> GetUsers(int userId)
		{
			return await GetUser(userId);
		}

		public async Task<CustomerDTO?> ChangeUserNames(int id,string firstName, string lastName)
		{
			var c = await _context.Customers.Where(cu => cu.Id == id).FirstOrDefaultAsync();
			c.FirstName = firstName;
			c.LastName = lastName;
			await _context.SaveChangesAsync();
			return Extensions.Extensions.ToDto(c);
		}

		public async Task<CustomerDTO?> ChangeUserPassword(int id, string newPassword)
		{
			var c = await _context.Customers.Where(cu => cu.Id == id).FirstOrDefaultAsync();
			c.Password = newPassword;
			await _context.SaveChangesAsync();
			return Extensions.Extensions.ToDto(c);
		}
	}
}
