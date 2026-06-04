using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Authorization;
using ElectronicShopAPI.DTO;
using ElectronicShopAPI.Data;
namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly DataContext _context;

		// Konstruktor wstrzykujący bazę danych
		public ProductsController(DataContext context)
		{
			_context = context;
		}

		// 1. STANDARDOWY GET (Pobiera wszystko)
		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
		{
			var products = await _context.Products.ToListAsync();
			return Ok(products);
		}

		// 2. NOWA FUNKCJA: Pobieranie jednego konkretnego produktu po ID
		// URL: api/products/5
		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProductById(int id)
		{
			// FindAsync szuka rekordu bezpośrednio po kluczu głównym (Primary Key)
			var product = await _context.Products.FindAsync(id);

			if (product == null)
			{
				// Jeśli produkt nie istnieje, zwracamy czysty HTTP 404 NotFound
				return NotFound($"Produkt o podanym ID ({id}) nie istnieje w sklepie.");
			}

			return Ok(product);
		}

		// 3. NOWA FUNKCJA: Filtrowanie produktów po cenie maksymalnej
		// URL: api/products/filter?maxPrice=150
		[AllowAnonymous]
		[HttpGet("filter")]
		public async Task<ActionResult<IEnumerable<Product>>> GetCheapProducts([FromQuery] decimal maxPrice)
		{
			// Za pomocą LINQ budujemy zapytanie SQL (efekt końcowy to: SELECT ... WHERE Price <= maxPrice)
			var filteredProducts = await _context.Products
				.Where(p => p.Price <= maxPrice)
				.ToListAsync();

			return Ok(filteredProducts);
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPost("AddProducts")]
		public async Task<ActionResult<IEnumerable<Product>>> AddProducts([FromBody] ProductDTO[] productsDto)
		{
			try
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
				return Ok(new { message = "Produkty zostały dodane pomyślnie", productIds = newProducts.Select(p => p.Id) });

			}
			catch (Exception ex)
			{
				_context.ChangeTracker.Clear(); 
				return BadRequest(new { message = "Nie udało się dodać produktu", error = ex.Message });
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpDelete("DeleteProducts")]
		public async Task<ActionResult<IEnumerable<Product>>> DeleteProducts([FromQuery] int[] id)
		{
			var products = new List<Product>();
			foreach (var productId in id)
			{
				var product = await _context.Products.FindAsync(productId);
				if (product == null)
				{
					return NotFound($"Produkt o podanym ID ({productId}) nie istnieje w sklepie.");
				}
				products.Add(product);
			}

			_context.Products.RemoveRange(products);
			await _context.SaveChangesAsync();
			return Ok(new { message = "Produkty zostały usunięte pomyślnie", id = products.Select(p => p.Id) });
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPut("ModifyProducts")]
		public async Task<ActionResult<IEnumerable<Product>>> ModifyProducts([FromBody] ProductDTO[] productsDto)
		{
			var products = new List<Product>();
			foreach (var productDto in productsDto)
			{
				var product = await _context.Products.FindAsync(productDto.Id);
				if (product == null)
				{
					return NotFound($"Produkt o podanym ID ({productDto.Id}) nie istnieje w sklepie.");
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
					product.NumOnStock = productDto.NumOnStock ;
				}
			}

			await _context.SaveChangesAsync();
			return Ok(new { message = "Produkty zostały zaktualizowane pomyślnie", ids = products.Select(p => p.Id) });
		}
	}
}
