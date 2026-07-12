using Shared.DTO.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface IFilterOptionService
	{
		Task<IEnumerable<FilterOptionDTO>> GetAllFilterOptions(int? pageNumber, int? pageSize);
		Task<FilterOptionDTO?> GetFilterOptionById(int id);
		Task<IEnumerable<int>> AddFilterOptions(FilterOptionDTO[] optionsDto);
		Task<IEnumerable<int>> DeleteFilterOptions(int[] ids);
		Task<IEnumerable<int>> ModifyFilterOptions(FilterOptionDTO[] optionsDto);
		Task<IEnumerable<FilterOptionDTO>> GetOptionsForFilter(int filterId);
	}
}
