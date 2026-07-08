using Shared.DTO;
using Shared.DTO.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface IProductService
	{
		Task<IEnumerable<ProductDTO>> GetAllProducts(int? pageNumber, int? pageSize);
		Task<ProductDTO?> GetProductById(int id);
		Task<IEnumerable<ProductDTO>> GetCheapProducts(decimal maxPrice);
		Task<IEnumerable<int>> AddProducts(ProductDTO[] productsDto);
		Task<IEnumerable<int>> DeleteProducts(int[] ids);
		Task<IEnumerable<int>> ModifyProducts(ProductDTO[] productsDto);
		Task<IEnumerable<ProductDTO>> FilterProducts(ProductFilterDTO filter);
	}
}
