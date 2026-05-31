using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicShopAPI.Data; // Ścieżka do Twojego wygenerowanego DataContext
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli

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
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
		{
			var products = await _context.Products.ToListAsync();
			return Ok(products);
		}

		// 2. NOWA FUNKCJA: Pobieranie jednego konkretnego produktu po ID
		// URL: api/products/5
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
		[HttpGet("filter")]
		public async Task<ActionResult<IEnumerable<Product>>> GetCheapProducts([FromQuery] decimal maxPrice)
		{
			// Za pomocą LINQ budujemy zapytanie SQL (efekt końcowy to: SELECT ... WHERE Price <= maxPrice)
			var filteredProducts = await _context.Products
				.Where(p => p.Price <= maxPrice)
				.ToListAsync();

			return Ok(filteredProducts);
		}
	}
}
