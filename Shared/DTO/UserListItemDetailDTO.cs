namespace Shared.DTO
{
	public class UserListItemDetailDTO
	{
        public int? Id { get; set; }
        public int? UserListId { get; set; }

        public int? ProductId { get; set; }
        public int Quantity { get; set; } = 1;

       public string ProductName { get; set; } = string.Empty;
    }
}
