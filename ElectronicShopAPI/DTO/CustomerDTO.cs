namespace ElectronicShopAPI.DTO
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

		public static CustomerDTO FromModel(Models.Customer c) => new CustomerDTO
		{
			Id = c.Id,
			FirstName = c.FirstName,
			LastName = c.LastName,
			Email = c.Email,
			Password = c.Password,
			PhoneNumber = c.PhoneNumber
		};
	}
}
