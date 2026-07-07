namespace Shared.DTO
{
	public class OrderItemDTO
	{
		public int? Id { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }

		// conversion moved to ElectronicShopAPI.Extensions.DtoExtensions
	}
}
