using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface IShoppingCartService
	{
		Task<IEnumerable<ShoppingCartItemDTO>> GetCart(int customerId);
	}
}
