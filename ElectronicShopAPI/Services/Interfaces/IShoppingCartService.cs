using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface IShoppingCartService
	{
		Task<IEnumerable<object>> GetUserShoppingCart(int userId);
		Task AddItemsToShoppingCart(int userId, OrderItemDTO[] items);
		Task RemoveItemsFromShoppingCart(int userId, OrderItemDTO[] items);
		Task ModifyQuantityItemsInShoppingCart(int userId, OrderItemDTO[] items);
		Task<IEnumerable<ShoppingCartItemDTO>> FindItemsInShoppingCart(int userId, string find);
	}
}
