using Microsoft.AspNetCore.Mvc;
using WebApiLMS.Models;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SetaBodyController : ControllerBase
	{
		private readonly SetaBodyService _service;
		public SetaBodyController(SetaBodyService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<List<SetaBodyModel>>> GetAll()
		{
			return Ok(await _service.GetAllAsync());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<SetaBodyModel>> GetById(int id)
		{
			var item = await _service.GetByIdAsync(id);
			if (item == null) return NotFound();
			return Ok(item);
		}

		[HttpPost]
		public async Task<ActionResult<SetaBodyModel>> Create([FromBody] SetaBodyModel request)
		{
			var created = await _service.CreateAsync(request);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] SetaBodyModel request)
		{
			var ok = await _service.UpdateAsync(id, request);
			if (!ok) return NotFound();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var ok = await _service.DeleteAsync(id);
			if (!ok) return NotFound();
			return NoContent();
		}
	}
}


