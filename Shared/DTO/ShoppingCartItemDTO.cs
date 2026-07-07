namespace Shared.DTO
{
	public class ShoppingCartItemDTO
	{
		public int ShoppingCartId { get; set; }
		public int? CustomerId { get; set; }
		public int? ProductId { get; set; }
		public ProductDTO Product { get; set; } = null!;
		public int Quantity { get; set; }

		// conversions moved to ElectronicShopAPI.Extensions.DtoExtensions
	}
}