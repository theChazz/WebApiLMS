using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiLMS.DTOs.CourseLecturerAssignment;
using WebApiLMS.Services;
using WebApiLMS.Models;

namespace WebApiLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseLecturerAssignmentController : ControllerBase
    {
        private readonly CourseLecturerAssignmentService _service;
        public CourseLecturerAssignmentController(CourseLecturerAssignmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseLecturerAssignmentModel>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseLecturerAssignmentDto>> GetById(int id)
        {
            var assignment = await _service.GetByIdAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            var assignmentDto = new CourseLecturerAssignmentDto
            {
                Id = assignment.Id,
                CourseId = assignment.CourseId,
                UserId = assignment.UserId,
                AssignedAt = assignment.AssignedAt
            };
            return Ok(assignmentDto);
        }

        [HttpPost]
        public async Task<ActionResult<CourseLecturerAssignmentDto>> Create([FromBody] CreateCourseLecturerAssignmentRequest request)
        {
            var createdAssignment = await _service.CreateAsync(request);
            
            var createdDto = new CourseLecturerAssignmentDto 
            { 
                 Id = createdAssignment.Id, 
                 CourseId = createdAssignment.CourseId, 
                 UserId = createdAssignment.UserId, 
                 AssignedAt = createdAssignment.AssignedAt
            }; 

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseLecturerAssignmentRequest request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}