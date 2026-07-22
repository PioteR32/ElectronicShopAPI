using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 // Ścieżka do Twojego wygenerowanego DataContext
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Authorization;
using Shared.DTO;
using System.Security.Claims;
using ElectronicShopAPI.Data;
using ElectronicShopAPI.Services;

namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CustomerController : ControllerBase
	{
		private readonly ICustomerService _customerService;
		public CustomerController(ICustomerService customerService)
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
		[HttpPost("Login")]
		public async Task<IActionResult> LogIn([FromBody] LoginDTO loginDTO)
		{
			try
			{
				var token = await _customerService.LogIn(loginDTO.Login, loginDTO.Password);
				if (token != null)
					return Ok(new { Message = "Succesfull", Token = token });
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
				var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
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
				var users = await _customerService.GetUsers(userId);
				if (users != null)
				{
					return Ok(users);
				}
				else
				{
					return NotFound(new { message = "Users not found" });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Get Users Failed", error = ex.Message });
			}
		}
	}
}
