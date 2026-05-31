
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ElectronicShopAPI
{
	enum UserRole
	{
		Admin,
		User,
		Guest
	}
	enum Permission
	{
		Read,
		Write,
		Delete,
		ModifyProducts,
		AllCustomers,
		AllPrivilages,
	}
	public static class AuthorizationFnc
	{
		public static string GenerateToken(int userId, string username, string role, IConfiguration configuration,params string[] customPermission )
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
					new Claim(ClaimTypes.Name, username),
					new Claim(ClaimTypes.Role, role),
					new Claim("Permission", string.Join(",", customPermission))
				}),
				Expires = DateTime.UtcNow.AddHours(2),
				Issuer = configuration["Jwt:Issuer"],
				Audience = configuration["Jwt:Audience"],
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
