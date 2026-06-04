namespace ElectronicShopAPI.DTO
{
	public class UserListItemDTO
	{
		public int? UserListId { get; set; }

		public int? ProductId { get; set; }
		public int Quantity { get; set; } = 1;
	}
}
