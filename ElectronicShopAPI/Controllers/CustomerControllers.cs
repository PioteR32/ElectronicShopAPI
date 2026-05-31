
using ElectronicShopAPI.Data; // Ścieżka do Twojego wygenerowanego DataContext
using ElectronicShopAPI.DTO;
using ElectronicShopAPI.Models;
using Microsoft.AspNetCore.Authorization; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CustomerControllers : ControllerBase
	{
		private readonly DataContext _context;
		private readonly IConfiguration _configuration;
		public CustomerControllers(DataContext dataContext, IConfiguration configuration)
		{
			_context = dataContext;
			_configuration = configuration;
		}
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] CustomerDTO cCDTO)
		{
			try
			{
				if (!_context.Customers.Any(c => c.Email == cCDTO.Email && c.PhoneNumber == cCDTO.PhoneNumber))
				{
					await _context.Customers.AddAsync(new Customer()
					{
						FirstName = cCDTO.FirstName
						,
						LastName = cCDTO.LastName
					,
						Email = cCDTO.Email
					,
						PhoneNumber = cCDTO.PhoneNumber
					,
						Password = cCDTO.Password
					});
					await _context.SaveChangesAsync();
					return Ok(new { message = "Create User Success" });
				}
				else
					return BadRequest(new { message = "Create User Failed", error = "Email or Phone Number already exists" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Create User Failed", error = ex.Message });
			}

		}
		[AllowAnonymous]
		[HttpGet("Login/{login}/Password/{password}")]
		public async Task<IActionResult> LogIn(string login, string password)
		{
			try
			{
				if (login == "admin" && password == "password")
				{

					return Ok(new { Token = AuthorizationFnc.GenerateToken(-1, login, "Admin", _configuration, [Permission.AllPrivilages.ToString()]) });
				}
				var customer = _context.Customers.Where(c => (c.Email == login || c.PhoneNumber == login) && c.Password == password).First();
				if (customer != null)
				{

					return Ok(new { message = "Login Success", Token = AuthorizationFnc.GenerateToken(customer.Id, login, "User", _configuration, [Permission.Read.ToString(), Permission.Write.ToString(), Permission.Delete.ToString()]) });
				}
				else
				{
					return BadRequest(new { message = "Login Failed", error = "Invalid Email, Phone Number, or Password" });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Create User Failed", error = ex.Message });
			}

		}
		
		[Authorize(Policy = "Read")]
		[HttpGet]
		public async Task<IActionResult> GetUser()
		{
			try
			{
				var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
				var user = await _context.Customers.Where(c => c.Id == userId).FirstOrDefaultAsync();
				if (user != null)
				{
					return Ok(new { message = "Get User Success",user = user });
				}
				else
				{
					return NotFound(new { message = "User not found" });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Get User Failed", error = ex.Message });
			}
		}
	}
}
