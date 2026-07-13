using Shared.DTO.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface IFilterOptionService
	{
		Task<IEnumerable<FilterOptionDTO>> GetOptions();
		Task<IEnumerable<FilterOptionDTO>> GetOptionsForFilter(int filterId);
		Task<IEnumerable<FilterOptionDTO>> GetCategoryFilterOptions(int categoryId);
		Task<FilterOptionDTO?> GetById(int id);
		Task<IEnumerable<int>> AddOptions(FilterOptionDTO[] options);
		Task<IEnumerable<int>> DeleteOptions(int[] ids);
		Task<IEnumerable<int>> ModifyOptions(FilterOptionDTO[] options);
	}
}
