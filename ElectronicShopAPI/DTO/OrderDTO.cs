using ElectronicShopAPI.Models;

namespace ElectronicShopAPI.DTO
{
	public class OrderDTO
	{
		public int Id { get; set; }

		public int? CustomerId { get; set; }

		public DateTime? OrderDate { get; set; }

		public decimal TotalAmount { get; set; }

		public string Email { get; set; } = null!;

		public string Street { get; set; } = null!;

		public string House { get; set; } = null!;

		public string Town { get; set; }

		public List<OrderItemDTO> OrderItemDTO { get; set; }
	}
	public class OrderItemDTO
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; } // Liczba pojedyncza - spójna z bazą
	}
}