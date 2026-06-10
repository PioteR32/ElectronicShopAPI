namespace ElectronicShopAPI.DTO
{
	public class OrderItemDTO
	{
		public int? Id { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }

		public static OrderItemDTO FromModel(Models.OrderItem oi) => new OrderItemDTO
		{
			Id = oi.Id,
			ProductId = oi.ProductId ?? -1,
			Quantity = oi.Quantity
		};
	}
}
