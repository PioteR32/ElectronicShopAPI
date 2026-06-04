using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		//Scaffold-DbContext "Server=.;Database=electronic_shop;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context DataContext -Force
		public AuthController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		//[HttpPost("login")]
		//public IActionResult Login([FromBody] LoginModel model)
		//{
		//	// UPROSZCZENIE: W realnej aplikacji weryfikujesz hasło w bazie danych
		//	if (model.Username == "admin" && model.Password == "password")
		//	{
		//		var tokenHandler = new JwtSecurityTokenHandler();
		//		var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

		//		// Definiowanie Claims (tożsamości użytkownika)
		//		var tokenDescriptor = new SecurityTokenDescriptor
		//		{
		//			Subject = new ClaimsIdentity(new[]
		//			{
		//			new Claim(ClaimTypes.Name, model.Username),
		//			new Claim(ClaimTypes.Role, "Admin"), // Rola do autoryzacji
  //                  new Claim("CustomAppPermission", "CanReadReports") // Własne uprawnienie
  //              }),
		//			Expires = DateTime.UtcNow.AddHours(2),
		//			Issuer = _configuration["Jwt:Issuer"],
		//			Audience = _configuration["Jwt:Audience"],
		//			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		//		};

		//		var token = tokenHandler.CreateToken(tokenDescriptor);
		//		var tokenString = tokenHandler.WriteToken(token);

		//		return Ok(new { Token = tokenString });
		//	}

		//	return Unauthorized("Nieprawidłowe dane logowania");
		//}
	}
}

public record LoginModel(string Username, string Password);