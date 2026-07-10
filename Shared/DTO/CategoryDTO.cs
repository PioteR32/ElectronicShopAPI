namespace Shared.DTO
{
	public class CategoryDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public int? ParentId { get; set; }
	}
}
