namespace Shared.DTO
{
	using System.ComponentModel.DataAnnotations;

	public class CustomerDTO
	{
		public int? Id { get; set; }
		[Required]
		public string FirstName { get; set; } = null!;

		[Required]
		public string LastName { get; set; } = null!;

		[Required]
		public string Email { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;

		public string? PhoneNumber { get; set; }

		// conversions moved to ElectronicShopAPI.Extensions.DtoExtensions
	}
}
