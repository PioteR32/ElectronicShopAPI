namespace ElectronicShopAPI.DTO
{
	public class UserListItemDetailDTO
	{
		public UserListItemDTO Item { get; set; } = null!;
		public ProductDTO Product { get; set; } = null!;

		public static UserListItemDetailDTO FromModels(Models.UserListItem uli, Models.Product p) => new UserListItemDetailDTO
		{
			Item = UserListItemDTO.FromModel(uli),
			Product = ProductDTO.FromModel(p)
		};
	}
}
