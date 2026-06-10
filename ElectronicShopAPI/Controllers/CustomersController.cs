using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 // Ścieżka do Twojego wygenerowanego DataContext
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Authorization;
using ElectronicShopAPI.DTO;
using System.Security.Claims;
using ElectronicShopAPI.Data;
using ElectronicShopAPI.Services;

namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CustomersController : ControllerBase
	{
		private readonly ICustomerService _customerService;
		public CustomersController(ICustomerService customerService)
		{
			_customerService = customerService;
		}
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] CustomerDTO cCDTO)
		{
			try
			{
				await _customerService.CreateUser(cCDTO);
				return Ok(new { message = "Create User Success" });
			}
			catch (InvalidOperationException ioe)
			{
				return BadRequest(new { message = "Create User Failed", error = ioe.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Create User Failed", error = ex.Message });
			}

		}
		[AllowAnonymous]
		[HttpPost("Login/{login}/Password/{password}")]
		public async Task<IActionResult> LogIn(string login, string password)
		{
			try
			{
				var token = await _customerService.LogIn(login, password);
				if (token != null)
					return Ok(new { message = "Login Success", Token = token });
				else
					return BadRequest(new { message = "Login Failed", error = "Invalid Email, Phone Number, or Password" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Create User Failed", error = ex.Message });
			}

		}
		
		[Authorize(Policy = "Read")]
		[HttpGet("Profile")]
		public async Task<IActionResult> GetUser()
		{
			try
			{
				// Retrieve the user id from the token's claims
				var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
				var user = await _customerService.GetUser(userId);
				if (user != null)
				{
					return Ok(user);
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
		//to do user filter DTO and add filter to get user method
		[Authorize(Policy = "AllCustomers")]
		[HttpGet("AllCustomers")]
		public async Task<IActionResult> GetUsers()
		{
			try
			{
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
				var user = await _customerService.GetUsers(userId);
				if (user != null)
				{
					return Ok(user);
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
