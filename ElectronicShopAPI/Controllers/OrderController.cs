using Microsoft.AspNetCore.Mvc;
using ElectronicShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using ElectronicShopAPI.DTO;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ElectronicShopAPI.Data; // Ścieżka do Twoich modeli




namespace ElectronicShopAPI.Controllers
{
	public class OrderController : ControllerBase
	{
		private readonly DataContext _context;
		public OrderController(DataContext dataContext)
		{
			_context = dataContext;
		}
		async Task<Order> GenerateOrderItems(OrderDTO orderDTO, int townId, string? userId)
		{
			Order order = new Order
			{
				CustomerId = orderDTO.CustomerId,
				TownId = townId,
				Email = orderDTO.Email,
				Street = orderDTO.Street,
				House = orderDTO.House,
				OrderDate = DateTime.Now,
				OrderItems = new List<OrderItem>() // ensure collection is initialized
			};

			if (userId == null)
			{
				// collect product ids from DTO in-memory
				var productIds = orderDTO.OrderItemDTO.Select(oi => oi.ProductId).ToList();

				// materialize product info from database (no assignments inside expression)
				var products = _context.Products
					.Where(p => productIds.Contains(p.Id))
					.Select(p => new { p.Id, p.IsInStock, p.Price })
					.ToList();

				// detect missing product ids from DTO
				var missing = productIds.Except(products.Select(p => p.Id)).ToList();
				if (missing.Any())
					throw new Exception("Failed to create order: Products not found: " + string.Join(", ", missing));

				foreach (var product in products)
				{
					if (product.IsInStock != true)
						throw new Exception($"Failed to create order: Product with ID {product.Id} not in stock");

					var dtoItem = orderDTO.OrderItemDTO.FirstOrDefault(q => q.ProductId == product.Id);
					var quantity = dtoItem?.Quantity ?? 0;

					order.OrderItems.Add(new OrderItem
					{
						ProductId = product.Id,
						Quantity = quantity,
						Price = product.Price
					});
				}

				order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.Price);
			}
			else
			{
				int IuserId = int.Parse(userId);
				var shoppingCartItems = _context.ShoppingCarts.Where(c => c.CustomerId == IuserId)
					.Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) => new { c.Quantity, p.Price, p.IsInStock });

				if (shoppingCartItems.Where(x => x.IsInStock == true).Count() == 0)
				{
					throw new Exception("Failed to create order: No valid items in shopping cart");
				}

				order.TotalAmount = shoppingCartItems.Sum(cp => cp.Quantity * cp.Price);
			}

			return order;
		}

		[AllowAnonymous]
		[HttpPost("CreateOrder")]
		public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
		{
			try
			{
				// to do: add Transaction
				var tmp = User.Claims.Where(c => orderDTO.CustomerId.ToString() == ClaimTypes.NameIdentifier).ToList();
				string userId = tmp.Count == 0? null : tmp[0].Value;
				if (userId != orderDTO.CustomerId.ToString() && orderDTO.CustomerId != null)
					return BadRequest(new { message = "Failed to create order", error = "You can only create orders for yourself" });

				if (string.IsNullOrWhiteSpace(orderDTO.Town))
					return BadRequest(new { message = "Failed to create order", error = "Town must be provided" });

				// make the local nullable to avoid nullability conversion warnings
				Town? town = _context.Towns.FirstOrDefault(t => t.Name == orderDTO.Town.ToLower());
				if (town == null)
				{
					town = new Town { Name = orderDTO.Town };
					_context.Towns.Add(town);
					_context.SaveChanges();
				}


				Order order = await GenerateOrderItems(orderDTO, town.Id, userId);

				await _context.Orders.AddAsync(order);
				await _context.SaveChangesAsync();

				return Ok(new { message = "Order created successfully", orderId = order.Id });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to create order", error = ex.Message });
				_context.ChangeTracker.Clear();
			}
		}
	}
}
