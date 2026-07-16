using Shared.DTO;
using Shared.DTO.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface IFilterService
	{
		Task<IEnumerable<FilterDTO>> GetAllFilters();
		Task<FilterDTO?> GetFilterById(int id);
		Task<IEnumerable<FilterDTO>> GetCategoryFilters(int categoryID);
		Task<IEnumerable<int>> AddFilters(FilterDTO[] filtersDto);
		Task<IEnumerable<int>> DeleteFilters(int[] ids);
		Task<IEnumerable<int>> ModifyFilters(FilterDTO[] filtersDto);
	}
}
