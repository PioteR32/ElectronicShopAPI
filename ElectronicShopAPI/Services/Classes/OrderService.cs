using ElectronicShopAPI.Data;
using Shared.DTO;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAPI.Services.Classes
{
	public class OrderService : IOrderService
	{
		private readonly DataContext _context;

		public OrderService(DataContext context)
		{
			_context = context;
		}

		private async Task<Order> GenerateOrderItems(OrderDTO orderDTO, int townId, string? userId)
		{
			Order order = new Order
			{
				CustomerId = orderDTO.CustomerId,
				TownId = townId,
				Email = orderDTO.Email,
				Street = orderDTO.Street,
				House = orderDTO.House,
				OrderDate = DateTime.Now,
				OrderItems = new List<OrderItem>()
			};

			var productIds = orderDTO.OrderItemDTO.Select(oi => oi.ProductId).ToList();
			var products = _context.Products.Where(p => productIds.Contains(p.Id)).ToList();

			if (userId == null)
			{
				if (products.Where(p => p.NumOnStock == 0).Any())
				{
					throw new Exception("Failed to create order: Some products not in stock");
				}

				foreach (var dto in orderDTO.OrderItemDTO)
				{
					var prod = products.FirstOrDefault(p => p.Id == dto.ProductId) ?? throw new Exception("Failed to create order");
					if (dto.Quantity > prod.NumOnStock)
						throw new Exception("Failed to create order: Quantity must be smaller than available stock");

					var orderItem = new OrderItem { ProductId = prod.Id, Price = prod.Price, Quantity = dto.Quantity };
					order.OrderItems.Add(orderItem);
				}

				order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.Price);
			}
			else
			{
				int IuserId = int.Parse(userId);
				var shoppingCartItems = _context.ShoppingCarts.Where(c => c.CustomerId == IuserId)
					.Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) => new { c.Quantity, p.Price, p.NumOnStock });

				if (!shoppingCartItems.Any(x => x.NumOnStock >= x.Quantity))
				{
					throw new Exception("Failed to create order: No valid items in shopping cart");
				}

				order.TotalAmount = shoppingCartItems.Sum(cp => cp.Quantity * cp.Price);
			}

			foreach (var oi in order.OrderItems)
			{
				var product = products.Find(p => p.Id == oi.ProductId);
				if (product != null)
				{
					product.NumOnStock -= oi.Quantity;
					_context.Products.Update(product);
				}
			}

			return order;
		}

		public async Task<int> CreateOrder(OrderDTO orderDTO, string? userId)
		{
			if (string.IsNullOrWhiteSpace(orderDTO.Town))
				throw new ArgumentException("Town must be provided");

			Town? town = _context.Towns.FirstOrDefault(t => t.Name == orderDTO.Town.ToLower());
			if (town == null)
			{
				town = new Town { Name = orderDTO.Town };
				_context.Towns.Add(town);
				await _context.SaveChangesAsync();
			}

			var order = await GenerateOrderItems(orderDTO, town.Id, userId);

			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

			return order.Id;
		}
	}
}
