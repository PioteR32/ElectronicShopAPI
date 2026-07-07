using ElectronicShopAPI.Data;
using Shared.DTO.Filters;
using Shared.DTO;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ElectronicShopAPI.Services
{
	public interface IProductService
	{
		Task<IEnumerable<ProductDTO>> GetAllProducts(int? pageNumber, int? pageSize);
		Task<ProductDTO?> GetProductById(int id);
		Task<IEnumerable<ProductDTO>> GetCheapProducts(decimal maxPrice);
		Task<IEnumerable<int>> AddProducts(ProductDTO[] productsDto);
		Task<IEnumerable<int>> DeleteProducts(int[] ids);
		Task<IEnumerable<int>> ModifyProducts(ProductDTO[] productsDto);
		Task<IEnumerable<ProductDTO>> FilterProducts(ProductFilterDTO filter);
	}

	public class ProductService: IProductService
	{
		private readonly DataContext _context;
		public ProductService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ProductDTO>> GetAllProducts(int? pageNumber, int? pageSize)
		{
			var query = _context.Products.AsQueryable();

			if (pageNumber.HasValue && pageSize.HasValue)
			{
				query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
			}

			var products = await query.ToListAsync();
			return products.Select(p => Extensions.Extensions.ToDto(p));
		}

		public async Task<ProductDTO?> GetProductById(int id)
		{
			var p = await _context.Products.FindAsync(id);
			return p == null ? null : Extensions.Extensions.ToDto(p);
		}

		public async Task<IEnumerable<ProductDTO>> GetCheapProducts(decimal maxPrice)
		{
			var products = await _context.Products.Where(p => p.Price <= maxPrice).ToListAsync();
			return products.Select(p => Extensions.Extensions.ToDto(p));
		}

		public async Task<IEnumerable<int>> AddProducts(ProductDTO[] productsDto)
		{
			var newProducts = new List<Product>();
			foreach (var productDto in productsDto)
			{
				var newProduct = new Product
				{
					Name = productDto.Name,
					Description = productDto.Description,
					Price = productDto.Price,
					NumOnStock = productDto.NumOnStock
				};

				newProducts.Add(newProduct);
			}

			await _context.Products.AddRangeAsync(newProducts);
			await _context.SaveChangesAsync();

			return newProducts.Select(p => p.Id);
		}

		public async Task<IEnumerable<int>> DeleteProducts(int[] ids)
		{
			var products = new List<Product>();
			foreach (var productId in ids)
			{
				var product = await _context.Products.FindAsync(productId);
				if (product == null)
				{
					throw new KeyNotFoundException($"Product with id {productId} not found");
				}
				products.Add(product);
			}

			_context.Products.RemoveRange(products);
			await _context.SaveChangesAsync();
			return products.Select(p => p.Id);
		}

		public async Task<IEnumerable<int>> ModifyProducts(ProductDTO[] productsDto)
		{
			var products = new List<Product>();
			foreach (var productDto in productsDto)
			{
				var product = await _context.Products.FindAsync(productDto.Id);
				if (product == null)
				{
					throw new KeyNotFoundException($"Product with id {productDto.Id} not found");
				}
				products.Add(product);
			}

			foreach (var product in products)
			{
				var productDto = productsDto.FirstOrDefault(dto => dto.Id == product.Id);
				if (productDto != null)
				{
					product.Name = productDto.Name;
					product.Description = productDto.Description;
					product.Price = productDto.Price;
					product.NumOnStock = productDto.NumOnStock;
				}
			}

			await _context.SaveChangesAsync();
			return products.Select(p => p.Id);
		}

		public async Task<IEnumerable<ProductDTO>> FilterProducts(ProductFilterDTO filter)
		{
			var data =  _context.Products.Where(p => !filter.Id.HasValue || p.Id == filter.Id);
			if (!string.IsNullOrEmpty(filter.Name))
			{
				data = data.Where(p => p.Name.Contains(filter.Name));
			}
			if (!string.IsNullOrEmpty(filter.Description))
			{
				data = data.Where(p => p.Description.Contains(filter.Description));
			}
			if (filter.IsInStock.HasValue)
			{
				data = data.Where(p => p.NumOnStock > 0);
			}
			if (filter.MinPrice.HasValue)
			{
				data = data.Where(p => p.Price >= filter.MinPrice);
			}
			if (filter.MaxPrice.HasValue)
			{
				data = data.Where(p => p.Price <= filter.MaxPrice);
			}
			data = data.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

			var products = await data.ToListAsync();
			return products.Select(p => Extensions.Extensions.ToDto(p));
		}
	}
}
