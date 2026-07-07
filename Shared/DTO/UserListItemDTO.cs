namespace Shared.DTO
{
	public class UserListItemDTO
	{
		public int? Id { get; set; }
		public int? UserListId { get; set; }

		public int? ProductId { get; set; }
		public int Quantity { get; set; } = 1;

		// conversions moved to ElectronicShopAPI.Extensions.DtoExtensions
	}
}
