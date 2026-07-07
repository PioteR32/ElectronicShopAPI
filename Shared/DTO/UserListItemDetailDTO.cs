namespace Shared.DTO
{
	public class UserListItemDetailDTO
	{
		public UserListItemDTO Item { get; set; } = null!;
		public ProductDTO Product { get; set; } = null!;

		// conversions moved to ElectronicShopAPI.Extensions.DtoExtensions
	}
}
