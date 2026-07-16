using ElectronicShopAPI.Data;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.DTO.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services.Classes
{
	public class FilterOptionService : ElectronicShopAPI.Services.IFilterOptionService
	{
		private readonly DataContext _context;
		public FilterOptionService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<FilterOptionDTO>> GetAllFilterOptions()
		{
			var list = await _context.FilterOptions.ToListAsync();
			return list.Select(fo => ElectronicShopAPI.Extensions.Extensions.ToDto(fo)
			.SetHaveOption(CheckOptionHasChildrens(fo.Id)));
		}

		public async Task<IEnumerable<FilterOptionDTO>> GetOptionsForCategory(int categoryID)
		{
			if(categoryID < 0) 
				return _context.FilterOptions.ToListAsync().Result.Select(fo => ElectronicShopAPI.Extensions.Extensions.ToDto(fo));


			var list = await _context.FilterOptions.Include(f => f.Filter)
				.Include(c => c.Filter.CategoriesFilters)
				.Where(fo => fo.Filter.CategoriesFilters.Any(cf => cf.CategoryId == categoryID))
				.ToListAsync();

			return list.Select(fo => ElectronicShopAPI.Extensions.Extensions.ToDto(fo));
		}
		bool CheckOptionHasChildrens(int filterOptionId)
		{
			return _context.FilterOptions.Any(fo => fo.ParentId == filterOptionId);
		}
		public async Task<IEnumerable<FilterOptionDTO>> GetOptionsForFilter(int filterId)
		{
			var list = await _context.FilterOptions.Where(fo => fo.FilterId == filterId && fo.ParentId == null).ToListAsync();
			return list.Select(fo => ElectronicShopAPI.Extensions.Extensions.ToDto(fo)
			.SetHaveOption(CheckOptionHasChildrens(fo.Id)));
		}
		public async Task<IEnumerable<FilterOptionDTO>> GetOptionsForFilterOption(int parentFilterOptionId)
		{
			var list = await _context.FilterOptions.Where(fo => fo.ParentId == parentFilterOptionId).ToListAsync();
			return list.Select(fo => ElectronicShopAPI.Extensions.Extensions.ToDto(fo)
			.SetHaveOption(CheckOptionHasChildrens(fo.Id)));
		}
		public async Task<FilterOptionDTO?> GetFilterOptionById(int id)
		{
			var fo = await _context.FilterOptions.FindAsync(id);
			return fo == null ? null : ElectronicShopAPI.Extensions.Extensions.ToDto(fo)
				.SetHaveOption(CheckOptionHasChildrens(id));
		}

		public async Task<IEnumerable<int>> AddFilterOptions(FilterOptionDTO[] optionsDto)
		{
			var newItems = optionsDto.Select(d => new FilterOption
			{
				Name = d.Name,
				FilterId = d.FilterId,
				ParentId = d.ParentId
			}).ToList();
			await _context.FilterOptions.AddRangeAsync(newItems);
			await _context.SaveChangesAsync();
			return newItems.Select(n => n.Id);
		}

		public async Task<IEnumerable<int>> DeleteFilterOptions(int[] ids)
		{
			var list = new List<FilterOption>();
			foreach (var id in ids)
			{
				var f = await _context.FilterOptions.FindAsync(id);
				if (f == null) throw new KeyNotFoundException($"FilterOption with id {id} not found");
				list.Add(f);
			}
			_context.FilterOptions.RemoveRange(list);
			await _context.SaveChangesAsync();
			return list.Select(f => f.Id);
		}

		public async Task<IEnumerable<int>> ModifyFilterOptions(FilterOptionDTO[] optionsDto)
		{
			var list = new List<FilterOption>();
			foreach (var dto in optionsDto)
			{
				var f = await _context.FilterOptions.FindAsync(dto.Id);
				if (f == null) throw new KeyNotFoundException($"FilterOption with id {dto.Id} not found");
				list.Add(f);
			}

			foreach (var f in list)
			{
				var dto = optionsDto.First(x => x.Id == f.Id);
				f.Name = dto.Name;
				f.FilterId = dto.FilterId;
				f.ParentId = dto.ParentId;
			}

			await _context.SaveChangesAsync();
			return list.Select(f => f.Id);
		}


	}
}
