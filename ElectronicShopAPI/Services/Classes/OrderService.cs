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
        private static readonly SemaphoreSlim OrderLock = new(1, 1);
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

            if (orderDTO.OrderItemDTO == null || orderDTO.OrderItemDTO.Count == 0)
                throw new ArgumentException("Order must contain products");

            if (orderDTO.OrderItemDTO == null || orderDTO.OrderItemDTO.Count == 0)
                throw new ArgumentException("Order must contain products");

            if (orderDTO.OrderItemDTO.Any(item => item.Quantity <= 0))
                throw new ArgumentException("Quantity must be greater than zero");

            var productIds = orderDTO.OrderItemDTO.Select(oi => oi.ProductId).ToList();


            var items = orderDTO.OrderItemDTO
                .GroupBy(item => item.ProductId)
                .Select(group => new OrderItemDTO
                {
                    ProductId = group.Key,
                    Quantity = group.Sum(item => item.Quantity)
                })
                .ToList();


            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            foreach (var dto in items)
            {
                var prod = products.FirstOrDefault(p => p.Id == dto.ProductId) ?? throw new Exception("Failed to create order");
                if (dto.Quantity > prod.NumOnStock)
                    throw new Exception("Failed to create order: Quantity must be smaller than available stock");

                var orderItem = new OrderItem { ProductId = prod.Id, Price = prod.Price, Quantity = dto.Quantity };
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.Price);

            foreach (var oi in orderDTO.OrderItemDTO)
            {
                var product = await _context.Products.FindAsync(oi.ProductId);
                if (product != null && product.NumOnStock >= oi.Quantity)
                {
                    product.NumOnStock -= oi.Quantity;
                    _context.Products.Update(product);
                }
                else
                {
                    throw new Exception("Failed to create order: Quantity must be smaller than available stock");
                }
            }

            return order;
        }

        public async Task<int> CreateOrder(OrderDTO orderDTO, string? userId)
        {
            ArgumentNullException.ThrowIfNull(orderDTO);

            if (string.IsNullOrWhiteSpace(orderDTO.Town))
                throw new ArgumentException("Town must be provided");

            
            bool lockTaken = false;

            try
            {
                var townName = orderDTO.Town.Trim();
                var normalizedTown = townName.ToLower();

                var town = await _context.Towns
                    .FirstOrDefaultAsync(t => t.Name.ToLower() == normalizedTown);

                if (town == null)
                {
                    town = new Town { Name = townName };
                    _context.Towns.Add(town);
                    await _context.SaveChangesAsync();
                }

                lockTaken = true;
                await OrderLock.WaitAsync();
                var order = await GenerateOrderItems(orderDTO, town.Id, userId);

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return order.Id;
            }
            finally
            {
                if (lockTaken)
                    OrderLock.Release();
            }
        }
    }
}
