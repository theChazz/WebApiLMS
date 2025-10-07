using Microsoft.AspNetCore.Mvc;
using WebApiLMS.DTOs.Submission;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        /// <summary>
        /// Get all submissions
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetAll()
        {
            var submissions = await _submissionService.GetAllAsync();
            return Ok(submissions);
        }

        /// <summary>
        /// Get submissions by assessment ID
        /// </summary>
        [HttpGet("assessment/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetByAssessmentId(int assessmentId)
        {
            var submissions = await _submissionService.GetByAssessmentIdAsync(assessmentId);
            return Ok(submissions);
        }

        /// <summary>
        /// Get submissions by student ID
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetByStudentId(int studentId)
        {
            var submissions = await _submissionService.GetByStudentIdAsync(studentId);
            return Ok(submissions);
        }

        /// <summary>
        /// Get submission by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SubmissionDto>> GetById(int id)
        {
            var submission = await _submissionService.GetByIdAsync(id);
            
            if (submission == null)
            {
                return NotFound($"Submission with ID {id} not found.");
            }

            return Ok(submission);
        }

        /// <summary>
        /// Get submission by assessment and student
        /// </summary>
        [HttpGet("assessment/{assessmentId}/student/{studentId}")]
        public async Task<ActionResult<SubmissionDto>> GetByAssessmentAndStudent(int assessmentId, int studentId)
        {
            var submission = await _submissionService.GetByAssessmentAndStudentAsync(assessmentId, studentId);
            
            if (submission == null)
            {
                return NotFound($"No submission found for assessment {assessmentId} and student {studentId}.");
            }

            return Ok(submission);
        }

        /// <summary>
        /// Create a new submission
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<SubmissionDto>> Create(CreateSubmissionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if student has already submitted
                var hasSubmitted = await _submissionService.HasStudentSubmittedAsync(request.AssessmentId, request.StudentId);
                if (hasSubmitted)
                {
                    return Conflict("Student has already submitted for this assessment.");
                }

                var submission = await _submissionService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = submission.Id }, submission);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating submission: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing submission
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<SubmissionDto>> Update(int id, UpdateSubmissionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var submission = await _submissionService.UpdateAsync(id, request);
                
                if (submission == null)
                {
                    return NotFound($"Submission with ID {id} not found.");
                }

                return Ok(submission);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating submission: {ex.Message}");
            }
        }

        /// <summary>
        /// Grade a submission
        /// </summary>
        [HttpPost("{id}/grade")]
        public async Task<ActionResult<SubmissionDto>> Grade(int id, GradeSubmissionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // In a real application, you would get the grader ID from authentication
                int gradedByUserId = 1; // Default for now

                var submission = await _submissionService.GradeAsync(id, request, gradedByUserId);
                
                if (submission == null)
                {
                    return NotFound($"Submission with ID {id} not found.");
                }

                return Ok(submission);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error grading submission: {ex.Message}");
            }
        }

        /// <summary>
        /// Auto-grade a submission
        /// </summary>
        [HttpPost("{id}/auto-grade")]
        public async Task<ActionResult<SubmissionDto>> AutoGrade(int id)
        {
            try
            {
                var submission = await _submissionService.AutoGradeAsync(id);
                
                if (submission == null)
                {
                    return NotFound($"Submission with ID {id} not found.");
                }

                return Ok(submission);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error auto-grading submission: {ex.Message}");
            }
        }

        /// <summary>
        /// Get submissions pending grading
        /// </summary>
        [HttpGet("pending-grading")]
        public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetPendingGrading()
        {
            var submissions = await _submissionService.GetPendingGradingAsync();
            return Ok(submissions);
        }

        /// <summary>
        /// Check if student has submitted for an assessment
        /// </summary>
        [HttpGet("check-submission")]
        public async Task<ActionResult<bool>> CheckSubmission([FromQuery] int assessmentId, [FromQuery] int studentId)
        {
            var hasSubmitted = await _submissionService.HasStudentSubmittedAsync(assessmentId, studentId);
            return Ok(hasSubmitted);
        }

        /// <summary>
        /// Delete a submission
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await _submissionService.DeleteAsync(id);
                
                if (!success)
                {
                    return NotFound($"Submission with ID {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting submission: {ex.Message}");
            }
        }
    }
}