using Microsoft.AspNetCore.Mvc;
using WebApiLMS.DTOs.Grading;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
    /// <summary>
    /// Controller for managing grades and grading operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GradingController : ControllerBase
    {
        private readonly IGradingService _gradingService;

        public GradingController(IGradingService gradingService)
        {
            _gradingService = gradingService;
        }

        #region Basic CRUD Operations

        /// <summary>
        /// Get all grades
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradingDto>>> GetAllGrades()
        {
            try
            {
                var grades = await _gradingService.GetAllGradesAsync();
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving grades.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get grade by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<GradingDto>> GetGradeById(int id)
        {
            try
            {
                var grade = await _gradingService.GetGradeByIdAsync(id);
                if (grade == null)
                    return NotFound(new { message = $"Grade with ID {id} not found." });

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the grade.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new grade
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<GradingDto>> CreateGrade([FromBody] CreateGradingRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var isValid = await _gradingService.ValidateGradeAsync(request);
                if (!isValid)
                    return BadRequest(new { message = "Invalid grade data provided." });

                var grade = await _gradingService.CreateGradeAsync(request);
                return CreatedAtAction(nameof(GetGradeById), new { id = grade.Id }, grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the grade.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing grade
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<GradingDto>> UpdateGrade(int id, [FromBody] UpdateGradingRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var grade = await _gradingService.UpdateGradeAsync(id, request);
                if (grade == null)
                    return NotFound(new { message = $"Grade with ID {id} not found." });

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the grade.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a grade
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGrade(int id)
        {
            try
            {
                var result = await _gradingService.DeleteGradeAsync(id);
                if (!result)
                    return NotFound(new { message = $"Grade with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the grade.", error = ex.Message });
            }
        }

        #endregion

        #region Grading-Specific Operations

        /// <summary>
        /// Get grades by student ID
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<GradingDto>>> GetGradesByStudent(int studentId)
        {
            try
            {
                var grades = await _gradingService.GetGradesByStudentAsync(studentId);
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving student grades.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get grades by assessment ID
        /// </summary>
        [HttpGet("assessment/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<GradingDto>>> GetGradesByAssessment(int assessmentId)
        {
            try
            {
                var grades = await _gradingService.GetGradesByAssessmentAsync(assessmentId);
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving assessment grades.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get grade by submission ID
        /// </summary>
        [HttpGet("submission/{submissionId}")]
        public async Task<ActionResult<GradingDto>> GetGradeBySubmission(int submissionId)
        {
            try
            {
                var grade = await _gradingService.GetGradeBySubmissionAsync(submissionId);
                if (grade == null)
                    return NotFound(new { message = $"No grade found for submission ID {submissionId}." });

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the submission grade.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get grade summaries with optional filtering
        /// </summary>
        [HttpGet("summaries")]
        public async Task<ActionResult<IEnumerable<GradeSummaryDto>>> GetGradeSummaries(
            [FromQuery] int? studentId = null, 
            [FromQuery] int? assessmentId = null)
        {
            try
            {
                var summaries = await _gradingService.GetGradeSummariesAsync(studentId, assessmentId);
                return Ok(summaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving grade summaries.", error = ex.Message });
            }
        }

        #endregion

        #region Bulk Operations

        /// <summary>
        /// Create grades in bulk
        /// </summary>
        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<GradingDto>>> CreateBulkGrades([FromBody] BulkGradingRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var grades = await _gradingService.CreateBulkGradesAsync(request);
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating bulk grades.", error = ex.Message });
            }
        }

        /// <summary>
        /// Publish grades
        /// </summary>
        [HttpPost("publish")]
        public async Task<ActionResult> PublishGrades([FromBody] PublishGradesRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _gradingService.PublishGradesAsync(request);
                if (!result)
                    return BadRequest(new { message = "Failed to publish grades." });

                return Ok(new { message = "Grades published successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while publishing grades.", error = ex.Message });
            }
        }

        /// <summary>
        /// Unpublish grades
        /// </summary>
        [HttpPost("unpublish")]
        public async Task<ActionResult> UnpublishGrades([FromBody] List<int> gradeIds)
        {
            try
            {
                var result = await _gradingService.UnpublishGradesAsync(gradeIds);
                if (!result)
                    return BadRequest(new { message = "Failed to unpublish grades." });

                return Ok(new { message = "Grades unpublished successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while unpublishing grades.", error = ex.Message });
            }
        }

        #endregion

        #region Grade Calculations and Adjustments

        /// <summary>
        /// Calculate/recalculate grade with optional grading scale
        /// </summary>
        [HttpPost("{id}/calculate")]
        public async Task<ActionResult<GradingDto>> CalculateGrade(int id, [FromQuery] int? gradingScaleId = null)
        {
            try
            {
                var grade = await _gradingService.CalculateGradeAsync(id, gradingScaleId);
                if (grade == null)
                    return NotFound(new { message = $"Grade with ID {id} not found." });

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating the grade.", error = ex.Message });
            }
        }

        /// <summary>
        /// Apply grade adjustment
        /// </summary>
        [HttpPost("adjust")]
        public async Task<ActionResult<GradingDto>> ApplyGradeAdjustment([FromBody] GradeAdjustmentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var grade = await _gradingService.ApplyGradeAdjustmentAsync(request);
                if (grade == null)
                    return NotFound(new { message = $"Grade with ID {request.GradeId} not found." });

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while applying grade adjustment.", error = ex.Message });
            }
        }

        /// <summary>
        /// Apply grade curve to all grades in an assessment
        /// </summary>
        [HttpPost("assessment/{assessmentId}/curve")]
        public async Task<ActionResult<IEnumerable<GradingDto>>> ApplyGradeCurve(int assessmentId, [FromQuery] decimal curvePercentage)
        {
            try
            {
                if (curvePercentage < 0 || curvePercentage > 50)
                    return BadRequest(new { message = "Curve percentage must be between 0 and 50." });

                var grades = await _gradingService.ApplyGradeCurveAsync(assessmentId, curvePercentage);
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while applying grade curve.", error = ex.Message });
            }
        }

        #endregion

        #region Statistics and Analytics

        /// <summary>
        /// Get grade statistics for an assessment
        /// </summary>
        [HttpGet("assessment/{assessmentId}/statistics")]
        public async Task<ActionResult> GetGradeStatistics(int assessmentId)
        {
            try
            {
                var statistics = await _gradingService.GetGradeStatisticsAsync(assessmentId);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving grade statistics.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get student grade analytics
        /// </summary>
        [HttpGet("student/{studentId}/analytics")]
        public async Task<ActionResult> GetStudentGradeAnalytics(int studentId)
        {
            try
            {
                var analytics = await _gradingService.GetStudentGradeAnalyticsAsync(studentId);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving student analytics.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get top performers for an assessment
        /// </summary>
        [HttpGet("assessment/{assessmentId}/top-performers")]
        public async Task<ActionResult<IEnumerable<GradingDto>>> GetTopPerformers(int assessmentId, [FromQuery] int count = 10)
        {
            try
            {
                if (count < 1 || count > 100)
                    return BadRequest(new { message = "Count must be between 1 and 100." });

                var topPerformers = await _gradingService.GetTopPerformersAsync(assessmentId, count);
                return Ok(topPerformers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving top performers.", error = ex.Message });
            }
        }

        /// <summary>
        /// Calculate student GPA
        /// </summary>
        [HttpGet("student/{studentId}/gpa")]
        public async Task<ActionResult> CalculateStudentGpa(int studentId, [FromQuery] int? termId = null)
        {
            try
            {
                var gpa = await _gradingService.CalculateGpaAsync(studentId, termId);
                return Ok(new { StudentId = studentId, GPA = gpa, TermId = termId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating GPA.", error = ex.Message });
            }
        }

        #endregion

        #region Grade History and Versioning

        /// <summary>
        /// Get grade history for a student and assessment
        /// </summary>
        [HttpGet("history/student/{studentId}/assessment/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<GradingDto>>> GetGradeHistory(int studentId, int assessmentId)
        {
            try
            {
                var history = await _gradingService.GetGradeHistoryAsync(studentId, assessmentId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving grade history.", error = ex.Message });
            }
        }

        /// <summary>
        /// Revert to a specific grade version
        /// </summary>
        [HttpPost("{id}/revert/{version}")]
        public async Task<ActionResult<GradingDto>> RevertToGradeVersion(int id, int version)
        {
            try
            {
                var grade = await _gradingService.RevertToGradeVersionAsync(id, version);
                if (grade == null)
                    return NotFound(new { message = $"Grade with ID {id} or version {version} not found." });

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while reverting grade version.", error = ex.Message });
            }
        }

        #endregion

        #region Import/Export

        /// <summary>
        /// Import grades from CSV
        /// </summary>
        [HttpPost("assessment/{assessmentId}/import")]
        public async Task<ActionResult<IEnumerable<GradingDto>>> ImportGradesFromCsv(int assessmentId, IFormFile csvFile)
        {
            try
            {
                if (csvFile == null || csvFile.Length == 0)
                    return BadRequest(new { message = "No CSV file provided." });

                using var stream = csvFile.OpenReadStream();
                var grades = await _gradingService.ImportGradesFromCsvAsync(assessmentId, stream);
                return Ok(grades);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "CSV import functionality is not yet implemented." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while importing grades.", error = ex.Message });
            }
        }

        /// <summary>
        /// Export grades to CSV
        /// </summary>
        [HttpGet("assessment/{assessmentId}/export")]
        public async Task<ActionResult> ExportGradesToCsv(int assessmentId)
        {
            try
            {
                var csvStream = await _gradingService.ExportGradesToCsvAsync(assessmentId);
                return File(csvStream, "text/csv", $"grades_assessment_{assessmentId}.csv");
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "CSV export functionality is not yet implemented." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while exporting grades.", error = ex.Message });
            }
        }

        #endregion

        #region Validation and Authorization

        /// <summary>
        /// Check if a grader can grade a specific assessment
        /// </summary>
        [HttpGet("assessment/{assessmentId}/can-grade/{graderId}")]
        public async Task<ActionResult> CanGradeAssessment(int assessmentId, int graderId)
        {
            try
            {
                var canGrade = await _gradingService.CanGradeAssessmentAsync(assessmentId, graderId);
                return Ok(new { CanGrade = canGrade });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while checking grading permissions.", error = ex.Message });
            }
        }

        #endregion
    }
}