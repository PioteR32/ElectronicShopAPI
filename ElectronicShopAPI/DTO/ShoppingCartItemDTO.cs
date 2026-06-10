namespace ElectronicShopAPI.DTO
{
	public class ShoppingCartItemDTO
	{
		public int ShoppingCartId { get; set; }
		public int? CustomerId { get; set; }
		public int? ProductId { get; set; }
		public ProductDTO Product { get; set; } = null!;
		public int Quantity { get; set; }

		public static ShoppingCartItemDTO FromModels(Models.ShoppingCart sc, Models.Product p) => new ShoppingCartItemDTO
		{
			ShoppingCartId = sc.Id,
			CustomerId = sc.CustomerId,
			ProductId = sc.ProductId,
			Product = ProductDTO.FromModel(p),
			Quantity = sc.Quantity
		};
	}
}