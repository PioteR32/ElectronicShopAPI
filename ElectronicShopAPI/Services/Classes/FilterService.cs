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
	public class FilterService : ElectronicShopAPI.Services.IFilterService
	{
		private readonly DataContext _context;
		public FilterService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<FilterDTO>> GetAllFilters()
		{
			var list = await _context.Filters.ToListAsync();
			return list.Select(f => ElectronicShopAPI.Extensions.Extensions.ToDto(f));
		}
		public async Task<IEnumerable<FilterDTO>> GetCategoryFilters(int categoryID)
		{
			if (categoryID < 0)
				return _context.Filters.ToListAsync().Result.Select(fo => ElectronicShopAPI.Extensions.Extensions.ToDto(fo));


			var list = await _context.Filters.Include(f => f.CategoriesFilters)
				.Where(fo => fo.CategoriesFilters.Any(cf => cf.CategoryId == categoryID))
				.ToListAsync();

			return list.Select(fo => ElectronicShopAPI.Extensions.Extensions.ToDto(fo));
		}
		public async Task<FilterDTO?> GetFilterById(int id)
		{
			var f = await _context.Filters.FindAsync(id);
			return f == null ? null : ElectronicShopAPI.Extensions.Extensions.ToDto(f);
		}

		public async Task<IEnumerable<int>> AddFilters(FilterDTO[] filtersDto)
		{
			var newItems = filtersDto.Select(d => new Filter
			{
				Name = d.Name
			}).ToList();
			await _context.Filters.AddRangeAsync(newItems);
			await _context.SaveChangesAsync();
			return newItems.Select(n => n.Id);
		}

		public async Task<IEnumerable<int>> DeleteFilters(int[] ids)
		{
			var list = new List<Filter>();
			foreach (var id in ids)
			{
				var f = await _context.Filters.FindAsync(id);
				if (f == null) throw new KeyNotFoundException($"Filter with id {id} not found");
				list.Add(f);
			}
			_context.Filters.RemoveRange(list);
			await _context.SaveChangesAsync();
			return list.Select(f => f.Id);
		}

		public async Task<IEnumerable<int>> ModifyFilters(FilterDTO[] filtersDto)
		{
			var list = new List<Filter>();
			foreach (var dto in filtersDto)
			{
				var f = await _context.Filters.FindAsync(dto.Id);
				if (f == null) throw new KeyNotFoundException($"Filter with id {dto.Id} not found");
				list.Add(f);
			}

			foreach (var f in list)
			{
				var dto = filtersDto.First(x => x.Id == f.Id);
				f.Name = dto.Name;
			}
			await _context.SaveChangesAsync();
			return list.Select(f => f.Id);
		}
	}
}
