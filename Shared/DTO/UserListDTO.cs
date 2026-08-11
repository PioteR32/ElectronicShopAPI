namespace Shared.DTO
{
	public class UserListDTO
	{
		public int Id { get; set; }
		public int? CustomerId { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		// conversion moved to ElectronicShopAPI.Extensions.DtoExtensions

	}
}
