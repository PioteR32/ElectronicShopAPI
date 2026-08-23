using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopApp.Services.Interfaces
{
	public interface ICategoriesService
	{
		Task<IEnumerable<CategoryDTO>> GetCategories();
		Task<CategoryDTO?> GetCategoryById(int id);
		Task<IEnumerable<int>> AddCategories(CategoryDTO[] categories);
		Task<IEnumerable<int>> DeleteCategories(int[] ids);
		Task<IEnumerable<int>> ModifyCategories(CategoryDTO[] categories);
	}
}
