using Microsoft.AspNetCore.Mvc;
using WebApiLMS.Services;
using WebApiLMS.DTOs.Assessment;

namespace WebApiLMS.Controllers
{
    /// <summary>
    /// Controller for Assessment operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentController : ControllerBase
    {
        private readonly AssessmentService _assessmentService;

        public AssessmentController(AssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        /// <summary>
        /// Get all assessments
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<AssessmentDto>>> GetAll()
        {
            try
            {
                var assessments = await _assessmentService.GetAllAsync();
                var assessmentDtos = assessments.Select(a => new AssessmentDto
                {
                    Id = a.Id,
                    CourseId = a.CourseId,
                    CourseName = a.Course?.CourseName,
                    Title = a.Title,
                    Description = a.Description,
                    AssessmentTypeId = a.AssessmentTypeId,
                    AssessmentTypeName = a.AssessmentType?.Name,
                    AssessmentCategoryId = a.AssessmentCategoryId,
                    AssessmentCategoryName = a.AssessmentCategory?.Name,
                    MaxMarks = a.MaxMarks,
                    Duration = a.Duration,
                    AttemptsAllowed = a.AttemptsAllowed,
                    WeightingPercentage = a.WeightingPercentage,
                    OpenDate = a.OpenDate,
                    DueDate = a.DueDate,
                    CloseDate = a.CloseDate,
                    RequiresModeration = a.RequiresModeration,
                    RequiresExternalModeration = a.RequiresExternalModeration,
                    ModerationPercentage = a.ModerationPercentage,
                    IsPublished = a.IsPublished,
                    IsActive = a.IsActive,
                    CreatedBy = a.CreatedBy,
                    CreatedAt = a.CreatedAt,
                    ModifiedBy = a.ModifiedBy,
                    ModifiedAt = a.ModifiedAt
                }).ToList();

                return Ok(assessmentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving assessments", details = ex.Message });
            }
        }

        /// <summary>
        /// Get assessments by course ID
        /// </summary>
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<List<AssessmentDto>>> GetByCourseId(int courseId)
        {
            try
            {
                var assessments = await _assessmentService.GetByCourseIdAsync(courseId);
                var assessmentDtos = assessments.Select(a => new AssessmentDto
                {
                    Id = a.Id,
                    CourseId = a.CourseId,
                    CourseName = a.Course?.CourseName,
                    Title = a.Title,
                    Description = a.Description,
                    AssessmentTypeId = a.AssessmentTypeId,
                    AssessmentTypeName = a.AssessmentType?.Name,
                    AssessmentCategoryId = a.AssessmentCategoryId,
                    AssessmentCategoryName = a.AssessmentCategory?.Name,
                    MaxMarks = a.MaxMarks,
                    Duration = a.Duration,
                    AttemptsAllowed = a.AttemptsAllowed,
                    WeightingPercentage = a.WeightingPercentage,
                    OpenDate = a.OpenDate,
                    DueDate = a.DueDate,
                    CloseDate = a.CloseDate,
                    RequiresModeration = a.RequiresModeration,
                    RequiresExternalModeration = a.RequiresExternalModeration,
                    ModerationPercentage = a.ModerationPercentage,
                    IsPublished = a.IsPublished,
                    IsActive = a.IsActive,
                    CreatedBy = a.CreatedBy,
                    CreatedAt = a.CreatedAt,
                    ModifiedBy = a.ModifiedBy,
                    ModifiedAt = a.ModifiedAt
                }).ToList();

                return Ok(assessmentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving course assessments", details = ex.Message });
            }
        }

        /// <summary>
        /// Get assessment by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentDto>> GetById(int id)
        {
            try
            {
                var assessment = await _assessmentService.GetByIdAsync(id);
                if (assessment == null)
                {
                    return NotFound(new { message = "Assessment not found" });
                }

                var assessmentDto = new AssessmentDto
                {
                    Id = assessment.Id,
                    CourseId = assessment.CourseId,
                    CourseName = assessment.Course?.CourseName,
                    Title = assessment.Title,
                    Description = assessment.Description,
                    AssessmentTypeId = assessment.AssessmentTypeId,
                    AssessmentTypeName = assessment.AssessmentType?.Name,
                    AssessmentCategoryId = assessment.AssessmentCategoryId,
                    AssessmentCategoryName = assessment.AssessmentCategory?.Name,
                    MaxMarks = assessment.MaxMarks,
                    Duration = assessment.Duration,
                    AttemptsAllowed = assessment.AttemptsAllowed,
                    WeightingPercentage = assessment.WeightingPercentage,
                    OpenDate = assessment.OpenDate,
                    DueDate = assessment.DueDate,
                    CloseDate = assessment.CloseDate,
                    RequiresModeration = assessment.RequiresModeration,
                    RequiresExternalModeration = assessment.RequiresExternalModeration,
                    ModerationPercentage = assessment.ModerationPercentage,
                    IsPublished = assessment.IsPublished,
                    IsActive = assessment.IsActive,
                    CreatedBy = assessment.CreatedBy,
                    CreatedAt = assessment.CreatedAt,
                    ModifiedBy = assessment.ModifiedBy,
                    ModifiedAt = assessment.ModifiedAt
                };

                return Ok(assessmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the assessment", details = ex.Message });
            }
        }

        /// <summary>
        /// Test validation - diagnostic endpoint
        /// </summary>
        [HttpGet("validate/{courseId}/{assessmentTypeId}/{assessmentCategoryId}/{userId}")]
        public async Task<ActionResult> TestValidation(int courseId, int assessmentTypeId, int assessmentCategoryId, int userId)
        {
            try
            {
                var results = new
                {
                    CourseExists = await _assessmentService.TestCourseExists(courseId),
                    AssessmentTypeExists = await _assessmentService.TestAssessmentTypeExists(assessmentTypeId),
                    AssessmentCategoryExists = await _assessmentService.TestAssessmentCategoryExists(assessmentCategoryId),
                    UserExists = await _assessmentService.TestUserExists(userId)
                };
                
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Validation test failed", details = ex.Message });
            }
        }

        /// <summary>
        /// Create new assessment
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AssessmentDto>> Create([FromBody] CreateAssessmentRequest request)
        {
            try
            {
                var assessment = await _assessmentService.CreateAsync(request);
                
                var assessmentDto = new AssessmentDto
                {
                    Id = assessment.Id,
                    CourseId = assessment.CourseId,
                    CourseName = assessment.Course?.CourseName,
                    Title = assessment.Title,
                    Description = assessment.Description,
                    AssessmentTypeId = assessment.AssessmentTypeId,
                    AssessmentTypeName = assessment.AssessmentType?.Name,
                    AssessmentCategoryId = assessment.AssessmentCategoryId,
                    AssessmentCategoryName = assessment.AssessmentCategory?.Name,
                    MaxMarks = assessment.MaxMarks,
                    Duration = assessment.Duration,
                    AttemptsAllowed = assessment.AttemptsAllowed,
                    WeightingPercentage = assessment.WeightingPercentage,
                    OpenDate = assessment.OpenDate,
                    DueDate = assessment.DueDate,
                    CloseDate = assessment.CloseDate,
                    RequiresModeration = assessment.RequiresModeration,
                    RequiresExternalModeration = assessment.RequiresExternalModeration,
                    ModerationPercentage = assessment.ModerationPercentage,
                    IsPublished = assessment.IsPublished,
                    IsActive = assessment.IsActive,
                    CreatedBy = assessment.CreatedBy,
                    CreatedAt = assessment.CreatedAt,
                    ModifiedBy = assessment.ModifiedBy,
                    ModifiedAt = assessment.ModifiedAt
                };

                return CreatedAtAction(nameof(GetById), new { id = assessment.Id }, assessmentDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Validation error", details = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = "Database operation failed", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred while creating the assessment", details = ex.Message });
            }
        }

        /// <summary>
        /// Update assessment
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateAssessmentRequest request)
        {
            try
            {
                var success = await _assessmentService.UpdateAsync(id, request);
                if (!success)
                {
                    return NotFound(new { message = "Assessment not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the assessment", details = ex.Message });
            }
        }

        /// <summary>
        /// Delete assessment
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await _assessmentService.DeleteAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Assessment not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the assessment", details = ex.Message });
            }
        }

        /// <summary>
        /// Publish assessment
        /// </summary>
        [HttpPost("{id}/publish")]
        public async Task<ActionResult> Publish(int id, [FromBody] PublishAssessmentRequest request)
        {
            try
            {
                var success = await _assessmentService.PublishAsync(id, request.PublishedBy);
                if (!success)
                {
                    return NotFound(new { message = "Assessment not found" });
                }

                return Ok(new { message = "Assessment published successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while publishing the assessment", details = ex.Message });
            }
        }

        /// <summary>
        /// Get assessment statistics
        /// </summary>
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<AssessmentStatisticsDto>> GetStatistics(int id)
        {
            try
            {
                var statistics = await _assessmentService.GetStatisticsAsync(id);
                return Ok(statistics);
            }
            catch (ArgumentException)
            {
                return NotFound(new { message = "Assessment not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving assessment statistics", details = ex.Message });
            }
        }
    }
}