using Microsoft.AspNetCore.Mvc;
using WebApiLMS.Models;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserRoleController : ControllerBase
	{
		private readonly UserRoleService _service;
		public UserRoleController(UserRoleService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<List<UserRoleModel>>> GetAll()
		{
			return Ok(await _service.GetAllAsync());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserRoleModel>> GetById(int id)
		{
			var item = await _service.GetByIdAsync(id);
			if (item == null) return NotFound();
			return Ok(item);
		}

		[HttpPost]
		public async Task<ActionResult<UserRoleModel>> Create([FromBody] UserRoleModel request)
		{
			var created = await _service.CreateAsync(request);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UserRoleModel request)
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


