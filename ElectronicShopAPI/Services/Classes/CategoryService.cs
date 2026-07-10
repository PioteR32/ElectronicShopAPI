using ElectronicShopAPI.Data;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services.Classes
{
	public class CategoryService : ElectronicShopAPI.Services.ICategoryService
	{
		private readonly DataContext _context;
		public CategoryService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
		{
			var query = _context.Categories;
			var cats = await query.ToListAsync();
			return cats.Select(c => ElectronicShopAPI.Extensions.Extensions.ToDto(c));
		}

		public async Task<CategoryDTO?> GetCategoryById(int id)
		{
			var c = await _context.Categories.FindAsync(id);
			return c == null ? null : ElectronicShopAPI.Extensions.Extensions.ToDto(c);
		}

		public async Task<IEnumerable<int>> AddCategories(CategoryDTO[] categoriesDto)
		{
			var newCategories = categoriesDto.Select(d => new Category
			{
				Name = d.Name,
				ParentId = d.ParentId
			}).ToList();

			await _context.Categories.AddRangeAsync(newCategories);
			await _context.SaveChangesAsync();
			return newCategories.Select(c => c.Id);
		}

		public async Task<IEnumerable<int>> DeleteCategories(int[] ids)
		{
			var list = new List<Category>();
			foreach (var id in ids)
			{
				var c = await _context.Categories.FindAsync(id);
				if (c == null) throw new KeyNotFoundException($"Category with id {id} not found");
				list.Add(c);
			}
			_context.Categories.RemoveRange(list);
			await _context.SaveChangesAsync();
			return list.Select(c => c.Id);
		}

		public async Task<IEnumerable<int>> ModifyCategories(CategoryDTO[] categoriesDto)
		{
			var list = new List<Category>();
			foreach (var dto in categoriesDto)
			{
				var c = await _context.Categories.FindAsync(dto.Id);
				if (c == null) throw new KeyNotFoundException($"Category with id {dto.Id} not found");
				list.Add(c);
			}

			foreach (var c in list)
			{
				var dto = categoriesDto.First(x => x.Id == c.Id);
				c.Name = dto.Name;
				c.ParentId = dto.ParentId;
			}

			await _context.SaveChangesAsync();
			return list.Select(c => c.Id);
		}
	}
}
