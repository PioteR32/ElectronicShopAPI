using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicShopAPI.Models; // Ścieżka do Twoich modeli
using Microsoft.AspNetCore.Authorization;
using ElectronicShopAPI.DTO;
using ElectronicShopAPI.Data;
using ElectronicShopAPI.Services;
namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		// Konstruktor wstrzykujący serwis produktów
		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		// 1. STANDARDOWY GET (Pobiera wszystko)
		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
		{
			var products = await _productService.GetAllProducts(pageNumber, pageSize);
			return Ok(products);
		}

		// 2. NOWA FUNKCJA: Pobieranie jednego konkretnego produktu po ID
		// URL: api/products/5
		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDTO>> GetProductById(int id)
		{
			// FindAsync szuka rekordu bezpośrednio po kluczu głównym (Primary Key)
			var product = await _productService.GetProductById(id);
			if (product == null)
			{
				return NotFound($"Produkt o podanym ID ({id}) nie istnieje w sklepie.");
			}
			return Ok(product);
		}

		// 3. NOWA FUNKCJA: Filtrowanie produktów po cenie maksymalnej
		// URL: api/products/filter?maxPrice=150
		[AllowAnonymous]
		[HttpGet("filter")]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetCheapProducts([FromQuery] decimal maxPrice)
		{
			// Za pomocą LINQ budujemy zapytanie SQL (efekt końcowy to: SELECT ... WHERE Price <= maxPrice)
			var filteredProducts = await _productService.GetCheapProducts(maxPrice);
			return Ok(filteredProducts);
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPost("AddProducts")]
		public async Task<ActionResult<IEnumerable<int>>> AddProducts([FromBody] ProductDTO[] productsDto)
		{
			try
			{
				var ids = await _productService.AddProducts(productsDto);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Nie udało się dodać produktu", error = ex.Message });
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpDelete("DeleteProducts")]
		public async Task<ActionResult<IEnumerable<int>>> DeleteProducts([FromQuery] int[] id)
		{
			try
			{
				var ids = await _productService.DeleteProducts(id);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPut("ModifyProducts")]
		public async Task<ActionResult<IEnumerable<int>>> ModifyProducts([FromBody] ProductDTO[] productsDto)
		{
			try
			{
				var ids = await _productService.ModifyProducts(productsDto);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
		}
	}
}
