using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLMS.Models;
using WebApiLMS.DTOs.UserProgramEnrollment;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProgramEnrollmentController : ControllerBase
    {
        private readonly UserProgramEnrollmentService _service;

        public UserProgramEnrollmentController(UserProgramEnrollmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserProgramEnrollmentDto>>> GetEnrollments()
        {
            var enrollments = await _service.GetAllEnrollmentsAsync();
            var dtos = enrollments.Select(e => new UserProgramEnrollmentDto
            {
                Id = e.Id,
                UserId = e.UserId,
                UserName = e.User?.FullName ?? "N/A",
                ProgramId = e.ProgramId,
                ProgramName = e.Program?.Name ?? "N/A",
                Status = e.Status,
                EnrolledAt = e.EnrolledAt,
                ExpectedCompletionDate = e.ExpectedCompletionDate,
                User = e.User,
                Program = e.Program
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProgramEnrollmentDto>> GetEnrollment(int id)
        {
            var enrollment = await _service.GetEnrollmentByIdAsync(id);
            if (enrollment == null) return NotFound();

            var dto = new UserProgramEnrollmentDto
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                UserName = enrollment.User?.FullName ?? "N/A",
                ProgramId = enrollment.ProgramId,
                ProgramName = enrollment.Program?.Name ?? "N/A",
                Status = enrollment.Status,
                EnrolledAt = enrollment.EnrolledAt,
                ExpectedCompletionDate = enrollment.ExpectedCompletionDate,
                User = enrollment.User,
                Program = enrollment.Program
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<UserProgramEnrollmentDto>> CreateEnrollment([FromBody] UserProgramEnrollmentRequest requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var enrollment = new UserProgramEnrollmentModel
            {
                UserId = requestDto.UserId,
                ProgramId = requestDto.ProgramId,
                Status = requestDto.Status,
                EnrolledAt = DateTime.UtcNow,
                ExpectedCompletionDate = requestDto.ExpectedCompletionDate
            };

            var createdEnrollment = await _service.AddEnrollmentAsync(enrollment);

            var dto = new UserProgramEnrollmentDto
            {
                Id = createdEnrollment.Id,
                UserId = createdEnrollment.UserId,
                UserName = createdEnrollment.User?.FullName ?? "N/A",
                ProgramId = createdEnrollment.ProgramId,
                ProgramName = createdEnrollment.Program?.Name ?? "N/A",
                Status = createdEnrollment.Status,
                EnrolledAt = createdEnrollment.EnrolledAt,
                ExpectedCompletionDate = createdEnrollment.ExpectedCompletionDate,
                User = createdEnrollment.User,
                Program = createdEnrollment.Program
            };

            return CreatedAtAction(nameof(GetEnrollment), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] UpdateUserProgramEnrollmentRequest requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEnrollment = await _service.GetEnrollmentByIdAsync(id);

            if (existingEnrollment == null)
            {
                return NotFound($"Enrollment with ID {id} not found.");
            }

            existingEnrollment.Status = requestDto.Status;
            existingEnrollment.ExpectedCompletionDate = requestDto.ExpectedCompletionDate;

            var updatedResult = await _service.UpdateEnrollmentAsync(id, existingEnrollment);

            if (updatedResult == null)
            {
                return NotFound($"Failed to update enrollment with ID {id}. It might have been deleted.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var result = await _service.DeleteEnrollmentAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
