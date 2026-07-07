namespace Shared.DTO.Filters
{
	public class ProductFilterDTO
	{
		public int? Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; } = null;
		public decimal? MinPrice { get; set; } = null;
		public decimal? MaxPrice { get; set;} = null;
		public bool? IsInStock { get; set; } = null;
		public int PageNumber { get; } = 1;
		public int PageSize { get; } = 10;
	}
}
