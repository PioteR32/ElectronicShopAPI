using Shared.DTO;
using Shared.DTO.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface IProductsService
	{
		Task<IEnumerable<ProductDTO>> GetProducts(int pageNumber, int pageSize);
		Task<IEnumerable<ProductDTO>> GetFilteredProducts(ProductFilterDTO filter);
		Task<List<ProductSpecificationDTO>> GetProductSpecsById(int id);
	}
}
