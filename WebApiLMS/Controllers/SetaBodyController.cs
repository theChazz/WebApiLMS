using Microsoft.AspNetCore.Mvc;
using WebApiLMS.Models;
using WebApiLMS.Services;
using WebApiLMS.DTOs.SetaBody;

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
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var items = await _service.GetAllAsync();
				return Ok(items);
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while fetching SETA bodies");
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var item = await _service.GetByIdAsync(id);
				if (item == null)
				{
					return NotFound("SETA body not found");
				}
				return Ok(item);
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while fetching the SETA body");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateSetaBodyRequest request)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return ValidationProblem(ModelState);
				}

				var model = new SetaBodyModel
				{
					Code = request.Code,
					Name = request.Name
				};

				var created = await _service.CreateAsync(model);
				return Ok(new { Id = created.Id, Message = "SETA body created successfully" });
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while creating the SETA body");
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UpdateSetaBodyRequest request)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return ValidationProblem(ModelState);
				}

				var model = new SetaBodyModel
				{
					Code = request.Code,
					Name = request.Name
				};

				var ok = await _service.UpdateAsync(id, model);
				if (!ok) return NotFound("SETA body not found");
				return Ok(new { Message = "SETA body updated successfully" });
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while updating the SETA body");
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var ok = await _service.DeleteAsync(id);
				if (!ok) return NotFound("SETA body not found");
				return Ok(new { Message = "SETA body deleted successfully" });
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while deleting the SETA body");
			}
		}
	}
}


