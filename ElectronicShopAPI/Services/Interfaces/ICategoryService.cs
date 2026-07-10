using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryDTO>> GetAllCategories();
		Task<CategoryDTO?> GetCategoryById(int id);
		Task<IEnumerable<int>> AddCategories(CategoryDTO[] categoriesDto);
		Task<IEnumerable<int>> DeleteCategories(int[] ids);
		Task<IEnumerable<int>> ModifyCategories(CategoryDTO[] categoriesDto);
	}
}
