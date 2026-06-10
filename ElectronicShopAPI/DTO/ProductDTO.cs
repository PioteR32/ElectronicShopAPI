namespace ElectronicShopAPI.DTO
{
	public class ProductDTO
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public decimal Price { get; set; }

		public bool? IsInStock { get => NumOnStock > 0; }
		public int NumOnStock { get; set; }

		public static ProductDTO FromModel(Models.Product p) => new ProductDTO
		{
			Id = p.Id,
			Name = p.Name,
			Description = p.Description,
			Price = p.Price,
			NumOnStock = p.NumOnStock
		};

		public static IEnumerable<ProductDTO> FromModels(IEnumerable<Models.Product> products) => products.Select(FromModel);
	}
}

