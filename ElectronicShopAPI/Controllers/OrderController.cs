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
			var productIds = orderDTO.OrderItemDTO.Select(oi => oi.ProductId).ToList();
			var products = _context.Products
					.Where(p => productIds.Contains(p.Id)).ToList();
			if (userId == null)
			{
				// collect product ids from DTO in-memory
				

				// materialize product info from database (no assignments inside expression)
				if(products.Where(p => p.NumOnStock == 0 ).Count() != 0)
				{
					throw new Exception("Failed to create order: Some products not in stock");
				}

				var orderItems = products.Select(p => new OrderItem { ProductId = p.Id, Price = p.Price,  Quantity = 0 });
				OrderItemDTO tmpOrderItemDTO;

				foreach(var orderItem in orderItems)
				{
					tmpOrderItemDTO = orderDTO.OrderItemDTO.Where(oi => oi.ProductId == orderItem.ProductId).FirstOrDefault();

					if(tmpOrderItemDTO == null)
						throw new Exception("Failed to create order");

					orderItem.Quantity = tmpOrderItemDTO?.Quantity ?? 0;

					if(orderItem.Quantity < products.Find(p => p.Id == orderItem.ProductId)?.NumOnStock || orderItem.Quantity == null) 
					{
						throw new Exception("Failed to create order: Quantity must be smaller than available stock");
					}

					order.OrderItems.Add(orderItem);
				}

				order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.Price);
			}
			else
			{
				int IuserId = int.Parse(userId);
				var shoppingCartItems = _context.ShoppingCarts.Where(c => c.CustomerId == IuserId)
					.Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) => new { c.Quantity, p.Price, p.NumOnStock });

				if (shoppingCartItems.Where(x => x.NumOnStock >= x.Quantity).Count() == 0)
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
