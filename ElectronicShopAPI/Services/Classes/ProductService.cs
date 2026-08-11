using ElectronicShopAPI.Data;
using Shared.DTO.Filters;
using Shared.DTO;
using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace ElectronicShopAPI.Services.Classes
{
    public class ProductService : IProductService
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
        public async Task<List<ProductSpecificationDTO>> GetProductSpecsById(int id)
        {
            var productSpecifications = _context.ProductFilterOptions.Where(fo => fo.ProductId == id)
                .Select(fo => fo.FilterOption)
                .Join(_context.Filters,fo=> fo.FilterId,f => f.Id,(fo,f) => new { Id  = fo.Id,Name = fo.Name,FilterId = fo.FilterId,ParentId = fo.ParentId,FilterName = f.Name })
                .Select(fo => new ProductSpecificationDTO
                {
                    Id = fo.Id,
                    Name = fo.Name,
                    FilterId = fo.FilterId,
                    ParentId = fo.ParentId,
                    FilterName = fo.FilterName
                })
                .ToList();

            return productSpecifications;
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
        private static char[] splitChars = new char[] { ' ', ',', '.', ';', ':', '-', '_', '/', '\\', '|', '!', '?', '@', '#', '$', '%', '^', '&', '*', '(', ')', '[', ']', '{', '}', '<', '>', '=', '+', '~' };
        int ConvertFromNullIntToInt(int? value)
        {
            if (value == null)
                return -1;
            return value.Value;
        }
        public async Task<IEnumerable<ProductDTO>> GetFilterProducts(ProductFilterDTO filter)
        {
            bool isInStock = filter.IsInStock.HasValue ? filter.IsInStock.HasValue : true;
            //
            if (filter.FilterOptionsIds.Count > 0)
            {
                var ParentsIds = _context.FilterOptions.Where(fo => filter.FilterOptionsIds.Contains(fo.Id)).Select(fo => ConvertFromNullIntToInt(fo.ParentId)).ToList();
                ParentsIds.RemoveAll(pId => filter.FilterOptionsIds.Contains(pId));
                filter.FilterOptionsIds = ParentsIds;
            }

            var data = _context.Products.AsNoTracking().Where(p => isInStock ? p.NumOnStock > 0 : p.NumOnStock == 0);

            if (filter.MinPrice.HasValue)
            {
                data = data.Where(p => p.Price >= filter.MinPrice);
            }
            if (filter.MaxPrice.HasValue)
            {
                data = data.Where(p => p.Price <= filter.MaxPrice);
            }
            if (filter.SearchPhrase != null && filter.SearchPhrase.Length > 0)
            {
                var splittedSearch = filter.SearchPhrase.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                data = data.Where(p =>
                p.Name.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)
                .Concat(p.Description.Split(splitChars, StringSplitOptions.RemoveEmptyEntries))
                .All(word => splittedSearch.Contains(word)));
            }

            if (filter.CategoryId.HasValue)
            {
                data = data.Include(p => p.Categories).Where(p => p.Categories.Any(c => c.Id == filter.CategoryId));
            }
            if (filter.FilterOptionsIds != null && filter.FilterOptionsIds.Count > 0)
            {
                var filterCount = _context.FilterOptions
                .Where(fo => filter.FilterOptionsIds.Contains(fo.Id))
                .Join(_context.Filters,
                    fo => fo.FilterId,
                    f => f.Id,
                    (fo, f) => new { FilterOption = fo, Filter = f })
                .GroupBy(x => x.Filter.Id)
                .Count();

                data = data
                    .Join(_context.ProductFilterOptions,
                    p => p.Id,
                    f => f.ProductId,
                    (p, f) => new { Product = p, FilterOptionId = f.FilterOptionId })
                    .Join(_context.FilterOptions,
                        x => x.FilterOptionId,
                        fo => fo.Id,
                        (x, fo) => new { x.Product, FilterOption = fo })
                    .Where(x => filter.FilterOptionsIds.Contains(x.FilterOption.Id))
                    .Join(_context.Filters,
                        x => x.FilterOption.FilterId,
                        f => f.Id,
                        (x, f) => new { JoinTable = x, Filter = f })

                    .GroupBy(x => new { x.JoinTable.Product })
                    .Where(x => x.GroupBy(xx => xx.Filter.Id).Count() == filterCount)
                    .Select(x => x.Key.Product);
            }

            if (filter.OrderByOptions != null)
            {
                switch (filter.OrderByOptions)
                {
                    case OrderByOptions.SortByPriceAsc:
                        data = data.OrderBy(p => p.Price);
                        break;
                    case OrderByOptions.SortByPriceDesc:
                        data = data.OrderByDescending(p => p.Price);
                        break;
                    case OrderByOptions.SortByRatingAsc:
                        data = data.Join(_context.Comments,
                            p => p.Id,
                            r => r.ProductId,
                            (p, r) => new { Product = p, Rating = r.Rating })
                            .GroupBy(x => x.Product)
                            .Select(g => new { Product = g.Key, AverageRating = g.Average(x => x.Rating) })
                            .OrderBy(x => x.AverageRating)
                            .Select(x => x.Product);
                        break;
                    case OrderByOptions.SortByRatingDesc:
                        data = data.Join(_context.Comments,
                            p => p.Id,
                            r => r.ProductId,
                            (p, r) => new { Product = p, Rating = r.Rating })
                            .GroupBy(x => x.Product)
                            .Select(g => new { Product = g.Key, AverageRating = g.Average(x => x.Rating) })
                            .OrderByDescending(x => x.AverageRating)
                            .Select(x => x.Product);
                        break;
                    default:
                        break;
                }
            }

            data = data.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
            var products = await data.ToListAsync();
            return products.Select(p => Extensions.Extensions.ToDto(p));
        }
    }
}
