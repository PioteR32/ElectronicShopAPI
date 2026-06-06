using Microsoft.AspNetCore.Mvc;
using ElectronicShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using ElectronicShopAPI.DTO;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ElectronicShopAPI.Data;
using ElectronicShopAPI.Services; // Ścieżka do Twoich modeli




namespace ElectronicShopAPI.Controllers
{
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[AllowAnonymous]
		[HttpPost("CreateOrder")]
		public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
		{
			try
			{
				var tmp = User.Claims.Where(c => orderDTO.CustomerId.ToString() == ClaimTypes.NameIdentifier).ToList();
				string? userId = tmp.Count == 0 ? null : tmp[0].Value;
				if (userId != orderDTO.CustomerId.ToString() && orderDTO.CustomerId != null)
					return BadRequest(new { message = "Failed to create order", error = "You can only create orders for yourself" });

				var orderId = await _orderService.CreateOrder(orderDTO, userId);
				return Ok(new { message = "Order created successfully", orderId = orderId });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to create order", error = ex.Message });
			}
		}
	}
}
