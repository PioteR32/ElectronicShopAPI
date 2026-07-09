namespace Shared.DTO
{
	public class CommentDTO
	{
		public int Id { get; set; }
		public string Comment { get; set; } = string.Empty;
		public byte Rating { get; set; }
		public int ProductId { get; set; }
		public int CustomerId { get; set; }
		public string CustomerFirstName { get; set; } = string.Empty;
		public string CustomerLastName { get; set; } = string.Empty;
	}
}
