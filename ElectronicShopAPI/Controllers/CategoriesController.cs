using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.DTO;
using ElectronicShopAPI.Services;

namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
		{
			var cats = await _categoryService.GetAllCategories();
			return Ok(cats);
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryDTO>> GetById(int id)
		{
			var c = await _categoryService.GetCategoryById(id);
			if (c == null) return NotFound();
			return Ok(c);
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPost("AddCategories")]
		public async Task<ActionResult<IEnumerable<int>>> AddCategories([FromBody] CategoryDTO[] dtos)
		{
			try
			{
				var ids = await _categoryService.AddCategories(dtos);
				return Ok(ids);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add categories", error = ex.Message });
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpDelete("DeleteCategories")]
		public async Task<ActionResult<IEnumerable<int>>> DeleteCategories([FromQuery] int[] id)
		{
			try
			{
				var ids = await _categoryService.DeleteCategories(id);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPut("ModifyCategories")]
		public async Task<ActionResult<IEnumerable<int>>> ModifyCategories([FromBody] CategoryDTO[] dtos)
		{
			try
			{
				var ids = await _categoryService.ModifyCategories(dtos);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
		}
	}
}
