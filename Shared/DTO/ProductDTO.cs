namespace Shared.DTO
{
	public class ProductDTO
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public decimal Price { get; set; }

		public bool? IsInStock { get => NumOnStock > 0; }
		public int NumOnStock { get; set; }

		// conversion now provided by ElectronicShopAPI.Extensions.DtoExtensions
	}
}

