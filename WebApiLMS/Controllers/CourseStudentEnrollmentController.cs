using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiLMS.DTOs.CourseStudentEnrollment;
using WebApiLMS.Services;
using WebApiLMS.Models;

namespace WebApiLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseStudentEnrollmentController : ControllerBase
    {
        private readonly CourseStudentEnrollmentService _service;
        public CourseStudentEnrollmentController(CourseStudentEnrollmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseStudentEnrollmentModel>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseStudentEnrollmentDto>> GetById(int id)
        {
            var enrollment = await _service.GetByIdAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            var enrollmentDto = new CourseStudentEnrollmentDto
            {
                Id = enrollment.Id,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId,
                EnrolledAt = enrollment.EnrolledAt,
                Progress = enrollment.Progress
            };
            return Ok(enrollmentDto);
        }

        [HttpPost]
        public async Task<ActionResult<CourseStudentEnrollmentDto>> Create([FromBody] CreateCourseStudentEnrollmentRequest request)
        {
            var createdEnrollment = await _service.CreateAsync(request);
            
            var createdDto = new CourseStudentEnrollmentDto 
            { 
                 Id = createdEnrollment.Id, 
                 CourseId = createdEnrollment.CourseId, 
                 StudentId = createdEnrollment.StudentId, 
                 EnrolledAt = createdEnrollment.EnrolledAt,
                 Progress = createdEnrollment.Progress
            }; 

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseStudentEnrollmentRequest request)
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