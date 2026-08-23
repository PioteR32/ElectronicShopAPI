using Shared.DTO.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface IFilterService
	{
		Task<IEnumerable<FilterDTO>> GetFilters();
		Task<IEnumerable<FilterDTO>> GetFiltersForCategory(int categoryId);
		Task<FilterDTO?> GetById(int id);
		Task<IEnumerable<int>> AddFilters(FilterDTO[] filters);
		Task<IEnumerable<int>> DeleteFilters(int[] ids);
		Task<IEnumerable<int>> ModifyFilters(FilterDTO[] filters);
	}
}
