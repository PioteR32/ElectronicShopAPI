using Microsoft.AspNetCore.Mvc;
using ElectronicShopAPI.Services.Interfaces;
using Shared.DTO;

namespace ElectronicShopAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CommentController : ControllerBase
	{
		private readonly ICommentService _service;
		public CommentController(ICommentService service)
		{
			_service = service;
		}

		[HttpGet("product/{productId}")]
		public async Task<IActionResult> GetForProduct(int productId)
		{
			var list = await _service.GetCommentsForProduct(productId);
			return Ok(list);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var c = await _service.GetById(id);
			if (c == null) return NotFound();
			return Ok(c);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CommentDTO dto)
		{
			var id = await _service.Create(dto);
			return CreatedAtAction(nameof(Get), new { id }, dto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, CommentDTO dto)
		{
			var ok = await _service.Update(id, dto);
			if (!ok) return NotFound();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var ok = await _service.Delete(id);
			if (!ok) return NotFound();
			return NoContent();
		}
	}
}
