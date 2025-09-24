using Microsoft.AspNetCore.Mvc;
using WebApiLMS.Models;
using WebApiLMS.DTOs.Program;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController : ControllerBase
    {
        private readonly ProgramService _programService;

        public ProgramController(ProgramService programService)
        {
            _programService = programService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPrograms()
        {
            try
            {
                var programs = await _programService.GetAllProgramsAsync();
                return Ok(programs);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching programs");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgram(int id)
        {
            try
            {
                var program = await _programService.GetProgramByIdAsync(id);
                if (program == null)
                {
                    return NotFound("Program not found");
                }
                return Ok(program);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the program");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] CreateProgramRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }

                var model = new ProgramModel
                {
                    Name = request.Name,
                    Code = request.Code,
                    Level = request.Level,
                    Department = request.Department,
                    Status = request.Status,
                    Description = request.Description,
                    ProgramTypeId = request.ProgramTypeId,
                    DurationMonths = request.DurationMonths
                };

                var program = await _programService.CreateProgramAsync(model);
                return Ok(new { Id = program.Id, Message = "Program created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the program");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, [FromBody] UpdateProgramRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }

                var program = new ProgramModel
                {
                    Name = request.Name,
                    Code = request.Code,
                    Level = request.Level,
                    Department = request.Department,
                    Status = request.Status,
                    Description = request.Description,
                    ProgramTypeId = request.ProgramTypeId,
                    DurationMonths = request.DurationMonths
                };

                var success = await _programService.UpdateProgramAsync(id, program);
                if (!success)
                {
                    return NotFound("Program not found");
                }
                return Ok(new { Message = "Program updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the program");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            try
            {
                var success = await _programService.DeleteProgramAsync(id);
                if (!success)
                {
                    return NotFound("Program not found");
                }
                return Ok(new { Message = "Program deleted successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the program");
            }
        }
    }
} 