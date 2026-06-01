using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicShopAPI.Data; // Ścieżka do Twojego wygenerowanego DataContext
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Authorization;
using ElectronicShopAPI.DTO;
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
		[HttpPost("AddProduct")]
		public async Task<ActionResult<IEnumerable<Product>>> AddProduct([FromBody] ProductDTO productDto)
		{
			// Za pomocą LINQ budujemy zapytanie SQL (efekt końcowy to: SELECT ... WHERE Price <= maxPrice)
			try
			{
				var newProduct = new Product
				{
					Name = productDto.Name,
					Description = productDto.Description,
					Price = productDto.Price,
					IsInStock = productDto.IsInStock
				};
				_context.Products.Add(newProduct);
				await _context.SaveChangesAsync();
				return Ok(new { message = "Produkt został dodany pomyślnie", productId = newProduct.Id });

			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Nie udało się dodać produktu", error = ex.Message });
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPost("DeleteProduct")]
		public async Task<ActionResult<IEnumerable<Product>>> DeleteProduct([FromQuery] int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound($"Produkt o podanym ID ({id}) nie istnieje w sklepie.");
			}
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return Ok(new { message = "Produkt został usunięty pomyślnie", id = product.Id });
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPost("ModifyProduct")]
		public async Task<ActionResult<IEnumerable<Product>>> ModifyProduct([FromBody] ProductDTO productDto)
		{
			var product = await _context.Products.FindAsync(productDto.Id);
			if (product == null)
			{
				return NotFound($"Produkt o podanym ID ({productDto.Id}) nie istnieje w sklepie.");
			}
			product.Name = productDto.Name;
			product.Description = productDto.Description;
			product.Price = productDto.Price;
			product.IsInStock = productDto.IsInStock;
			await _context.SaveChangesAsync();
			return Ok(new { message = "Produkt został zaktualizowany pomyślnie", id = product.Id });
		}
	}
}
