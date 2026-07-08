using Shared.DTO;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface IOrderService
	{
		Task<int> CreateOrder(OrderDTO orderDTO, string? userId);
	}
}
