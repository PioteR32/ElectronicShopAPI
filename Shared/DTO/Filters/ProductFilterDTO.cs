namespace Shared.DTO.Filters
{
	public class ProductFilterDTO
	{
		public decimal? MinPrice { get; set; } = null;
		public decimal? MaxPrice { get; set;} = null;
		public bool? IsInStock { get; set; } = null;
		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; } = 10;	
		public OrderByOptions OrderByOptions { get; set; } = 0;

		public List<int> FilterOptionsIds { get; set; } = new List<int>();

		
	}
}
