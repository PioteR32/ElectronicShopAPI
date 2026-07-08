using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface IProductsService
	{
		Task<IEnumerable<ProductDTO>> GetProducts(int pageNumber, int pageSize);
	}
}
