using ElectronicShopAPI.Models;
using ElectronicShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Filters;

namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FiltersController : ControllerBase
	{
		private readonly IFilterService _filterService;
		public FiltersController(IFilterService filterService)
		{
			_filterService = filterService;
		}
		public const string urlGetFilters = "filters";
		[AllowAnonymous]
		[HttpGet(urlGetFilters)]
		public async Task<ActionResult<IEnumerable<FilterDTO>>> GetFilters()
		{
			var items = await _filterService.GetAllFilters();
			return Ok(items);
		}


		[AllowAnonymous]
		[HttpGet("filters/by-category/{categoryID}")]
		public async Task<ActionResult<IEnumerable<FilterDTO>>> GetFiltersByCategory(int categoryID)
		{
			var items = await _filterService.GetCategoryFilters(categoryID);
			return Ok(items);
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<ActionResult<FilterDTO>> GetById(int id)
		{
			var item = await _filterService.GetFilterById(id);
			if (item == null) return NotFound();
			return Ok(item);
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPost("AddFilters")]
		public async Task<ActionResult<IEnumerable<int>>> AddFilters([FromBody] FilterDTO[] dtos)
		{
			try
			{
				var ids = await _filterService.AddFilters(dtos);
				return Ok(ids);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add filters", error = ex.Message });
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpDelete("DeleteFilters")]
		public async Task<ActionResult<IEnumerable<int>>> DeleteFilters([FromQuery] int[] id)
		{
			try
			{
				var ids = await _filterService.DeleteFilters(id);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPut("ModifyFilters")]
		public async Task<ActionResult<IEnumerable<int>>> ModifyFilters([FromBody] FilterDTO[] dtos)
		{
			try
			{
				var ids = await _filterService.ModifyFilters(dtos);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
		}
	}
}
