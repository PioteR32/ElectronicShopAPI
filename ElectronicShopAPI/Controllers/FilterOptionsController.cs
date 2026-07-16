using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.DTO.Filters;
using ElectronicShopAPI.Services;

namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FilterOptionsController : ControllerBase
	{
		private readonly IFilterOptionService _service;
		public FilterOptionsController(IFilterOptionService service)
		{
			_service = service;
		}

		[AllowAnonymous]
		[HttpGet("filterOptions")]
		public async Task<ActionResult<IEnumerable<FilterOptionDTO>>> GetOptions()
		{
			var items = await _service.GetAllFilterOptions();
			return Ok(items);
		}

		[AllowAnonymous]
		[HttpGet("ByCategory/{categoryId}")]
		public async Task<ActionResult<IEnumerable<FilterOptionDTO>>> GetOptionsByCategory(int categoryId)
		{
			var items = await _service.GetOptionsForCategory(categoryId);
			return Ok(items);
		}

		[AllowAnonymous]
		[HttpGet("ByFilter/{filterId}")]
		public async Task<ActionResult<IEnumerable<FilterOptionDTO>>> GetByFilter(int filterId)
		{
			var items = await _service.GetOptionsForFilter(filterId);
			return Ok(items);
		}

		[AllowAnonymous]
		[HttpGet("ByFilterOption/{parentFilterOptionId}")]
		public async Task<ActionResult<IEnumerable<FilterOptionDTO>>> GetByFilterOption(int parentFilterOptionId)
		{
			var items = await _service.GetOptionsForFilterOption(parentFilterOptionId);
			return Ok(items);
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<ActionResult<FilterOptionDTO>> GetById(int id)
		{
			var item = await _service.GetFilterOptionById(id);
			if (item == null) return NotFound();
			return Ok(item);
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPost("AddFilterOptions")]
		public async Task<ActionResult<IEnumerable<int>>> AddOptions([FromBody] FilterOptionDTO[] dtos)
		{
			try
			{
				var ids = await _service.AddFilterOptions(dtos);
				return Ok(ids);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to add filter options", error = ex.Message });
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpDelete("DeleteFilterOptions")]
		public async Task<ActionResult<IEnumerable<int>>> DeleteOptions([FromQuery] int[] id)
		{
			try
			{
				var ids = await _service.DeleteFilterOptions(id);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
		}

		[Authorize(Policy = "ModifyProducts")]
		[HttpPut("ModifyFilterOptions")]
		public async Task<ActionResult<IEnumerable<int>>> ModifyOptions([FromBody] FilterOptionDTO[] dtos)
		{
			try
			{
				var ids = await _service.ModifyFilterOptions(dtos);
				return Ok(ids);
			}
			catch (KeyNotFoundException knf)
			{
				return NotFound(knf.Message);
			}
		}
	}
}
