using Shared.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface IOrdersService
	{
		Task<int?> CreateOrder(OrderDTO dto);
	}
}
