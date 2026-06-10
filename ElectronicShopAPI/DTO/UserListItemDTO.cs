namespace ElectronicShopAPI.DTO
{
	public class UserListItemDTO
	{
		public int? Id { get; set; }
		public int? UserListId { get; set; }

		public int? ProductId { get; set; }
		public int Quantity { get; set; } = 1;

		public static UserListItemDTO FromModel(Models.UserListItem uli) => new UserListItemDTO
		{
			Id = uli.Id,
			UserListId = uli.UserListId,
			ProductId = uli.ProductId,
			Quantity = uli.Quantity
		};
	}
}
