using Microsoft.AspNetCore.Mvc;
using WebApiLMS.DTOs.Rubric;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
    /// <summary>
    /// Controller for managing rubrics and rubric-based assessment
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RubricController : ControllerBase
    {
        private readonly IRubricService _rubricService;

        public RubricController(IRubricService rubricService)
        {
            _rubricService = rubricService;
        }

        #region Basic CRUD Operations for Rubrics

        /// <summary>
        /// Get all rubrics
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RubricDto>>> GetAllRubrics()
        {
            try
            {
                var rubrics = await _rubricService.GetAllRubricsAsync();
                return Ok(rubrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving rubrics.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get rubric by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RubricDto>> GetRubricById(int id)
        {
            try
            {
                var rubric = await _rubricService.GetRubricByIdAsync(id);
                if (rubric == null)
                    return NotFound(new { message = $"Rubric with ID {id} not found." });

                return Ok(rubric);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the rubric.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new rubric
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RubricDto>> CreateRubric([FromBody] CreateRubricRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var rubric = await _rubricService.CreateRubricAsync(request);
                return CreatedAtAction(nameof(GetRubricById), new { id = rubric.Id }, rubric);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the rubric.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing rubric
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RubricDto>> UpdateRubric(int id, [FromBody] UpdateRubricRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var rubric = await _rubricService.UpdateRubricAsync(id, request);
                if (rubric == null)
                    return NotFound(new { message = $"Rubric with ID {id} not found." });

                return Ok(rubric);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the rubric.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a rubric
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRubric(int id)
        {
            try
            {
                var result = await _rubricService.DeleteRubricAsync(id);
                if (!result)
                    return NotFound(new { message = $"Rubric with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the rubric.", error = ex.Message });
            }
        }

        #endregion

        #region Rubric-Specific Operations

        /// <summary>
        /// Get rubric summaries
        /// </summary>
        [HttpGet("summaries")]
        public async Task<ActionResult<IEnumerable<RubricSummaryDto>>> GetRubricSummaries()
        {
            try
            {
                var summaries = await _rubricService.GetRubricSummariesAsync();
                return Ok(summaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving rubric summaries.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get rubrics by assessment ID
        /// </summary>
        [HttpGet("assessment/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<RubricDto>>> GetRubricsByAssessment(int assessmentId)
        {
            try
            {
                var rubrics = await _rubricService.GetRubricsByAssessmentAsync(assessmentId);
                return Ok(rubrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving assessment rubrics.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create rubric from template
        /// </summary>
        [HttpPost("template")]
        public async Task<ActionResult<RubricDto>> CreateRubricFromTemplate([FromBody] CreateRubricFromTemplateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var rubric = await _rubricService.CreateRubricFromTemplateAsync(request);
                return CreatedAtAction(nameof(GetRubricById), new { id = rubric.Id }, rubric);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating rubric from template.", error = ex.Message });
            }
        }

        /// <summary>
        /// Copy an existing rubric
        /// </summary>
        [HttpPost("copy")]
        public async Task<ActionResult<RubricDto>> CopyRubric([FromBody] CopyRubricRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var rubric = await _rubricService.CopyRubricAsync(request);
                return CreatedAtAction(nameof(GetRubricById), new { id = rubric.Id }, rubric);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while copying the rubric.", error = ex.Message });
            }
        }

        #endregion

        #region Criterion Operations

        /// <summary>
        /// Get criterion by ID
        /// </summary>
        [HttpGet("criteria/{criterionId}")]
        public async Task<ActionResult<RubricCriterionDto>> GetCriterionById(int criterionId)
        {
            try
            {
                var criterion = await _rubricService.GetCriterionByIdAsync(criterionId);
                if (criterion == null)
                    return NotFound(new { message = $"Criterion with ID {criterionId} not found." });

                return Ok(criterion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the criterion.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new criterion for a rubric
        /// </summary>
        [HttpPost("{rubricId}/criteria")]
        public async Task<ActionResult<RubricCriterionDto>> CreateCriterion(int rubricId, [FromBody] CreateRubricCriterionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var criterion = await _rubricService.CreateCriterionAsync(rubricId, request);
                return CreatedAtAction(nameof(GetCriterionById), new { criterionId = criterion.Id }, criterion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the criterion.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing criterion
        /// </summary>
        [HttpPut("criteria/{criterionId}")]
        public async Task<ActionResult<RubricCriterionDto>> UpdateCriterion(int criterionId, [FromBody] UpdateRubricCriterionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var criterion = await _rubricService.UpdateCriterionAsync(criterionId, request);
                if (criterion == null)
                    return NotFound(new { message = $"Criterion with ID {criterionId} not found." });

                return Ok(criterion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the criterion.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a criterion
        /// </summary>
        [HttpDelete("criteria/{criterionId}")]
        public async Task<ActionResult> DeleteCriterion(int criterionId)
        {
            try
            {
                var result = await _rubricService.DeleteCriterionAsync(criterionId);
                if (!result)
                    return NotFound(new { message = $"Criterion with ID {criterionId} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the criterion.", error = ex.Message });
            }
        }

        /// <summary>
        /// Reorder criteria within a rubric
        /// </summary>
        [HttpPost("criteria/reorder")]
        public async Task<ActionResult> ReorderCriteria([FromBody] ReorderCriteriaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _rubricService.ReorderCriteriaAsync(request);
                if (!result)
                    return BadRequest(new { message = "Failed to reorder criteria." });

                return Ok(new { message = "Criteria reordered successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while reordering criteria.", error = ex.Message });
            }
        }

        /// <summary>
        /// Bulk update criteria
        /// </summary>
        [HttpPut("criteria/bulk")]
        public async Task<ActionResult<IEnumerable<RubricCriterionDto>>> BulkUpdateCriteria([FromBody] BulkUpdateCriteriaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var criteria = await _rubricService.BulkUpdateCriteriaAsync(request);
                return Ok(criteria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while bulk updating criteria.", error = ex.Message });
            }
        }

        #endregion

        #region Level Operations

        /// <summary>
        /// Get level by ID
        /// </summary>
        [HttpGet("levels/{levelId}")]
        public async Task<ActionResult<RubricLevelDto>> GetLevelById(int levelId)
        {
            try
            {
                var level = await _rubricService.GetLevelByIdAsync(levelId);
                if (level == null)
                    return NotFound(new { message = $"Level with ID {levelId} not found." });

                return Ok(level);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the level.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new level for a criterion
        /// </summary>
        [HttpPost("criteria/{criterionId}/levels")]
        public async Task<ActionResult<RubricLevelDto>> CreateLevel(int criterionId, [FromBody] CreateRubricLevelRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var level = await _rubricService.CreateLevelAsync(criterionId, request);
                return CreatedAtAction(nameof(GetLevelById), new { levelId = level.Id }, level);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the level.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing level
        /// </summary>
        [HttpPut("levels/{levelId}")]
        public async Task<ActionResult<RubricLevelDto>> UpdateLevel(int levelId, [FromBody] UpdateRubricLevelRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var level = await _rubricService.UpdateLevelAsync(levelId, request);
                if (level == null)
                    return NotFound(new { message = $"Level with ID {levelId} not found." });

                return Ok(level);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the level.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a level
        /// </summary>
        [HttpDelete("levels/{levelId}")]
        public async Task<ActionResult> DeleteLevel(int levelId)
        {
            try
            {
                var result = await _rubricService.DeleteLevelAsync(levelId);
                if (!result)
                    return NotFound(new { message = $"Level with ID {levelId} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the level.", error = ex.Message });
            }
        }

        #endregion

        #region Rubric Evaluation and Scoring

        /// <summary>
        /// Evaluate a student using a rubric
        /// </summary>
        [HttpPost("evaluate")]
        public async Task<ActionResult<RubricEvaluationDto>> EvaluateWithRubric([FromBody] RubricEvaluationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var evaluation = await _rubricService.EvaluateWithRubricAsync(request);
                return Ok(evaluation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while evaluating with rubric.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get rubric evaluation for a student
        /// </summary>
        [HttpGet("{rubricId}/evaluation/student/{studentId}")]
        public async Task<ActionResult<RubricEvaluationDto>> GetRubricEvaluation(int rubricId, int studentId, [FromQuery] int? submissionId = null)
        {
            try
            {
                var evaluation = await _rubricService.GetRubricEvaluationAsync(rubricId, studentId, submissionId);
                if (evaluation == null)
                    return NotFound(new { message = "Rubric evaluation not found." });

                return Ok(evaluation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving rubric evaluation.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get all rubric evaluations for an assessment
        /// </summary>
        [HttpGet("evaluations/assessment/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<RubricEvaluationDto>>> GetRubricEvaluationsByAssessment(int assessmentId)
        {
            try
            {
                var evaluations = await _rubricService.GetRubricEvaluationsByAssessmentAsync(assessmentId);
                return Ok(evaluations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving assessment evaluations.", error = ex.Message });
            }
        }

        /// <summary>
        /// Calculate rubric score
        /// </summary>
        [HttpPost("{rubricId}/calculate-score")]
        public async Task<ActionResult> CalculateRubricScore(int rubricId, [FromBody] List<CriterionScoreRequest> criterionScores)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var score = await _rubricService.CalculateRubricScoreAsync(rubricId, criterionScores);
                return Ok(new { RubricId = rubricId, TotalScore = score });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating rubric score.", error = ex.Message });
            }
        }

        #endregion

        #region Analytics and Statistics

        /// <summary>
        /// Get rubric statistics
        /// </summary>
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult> GetRubricStatistics(int id)
        {
            try
            {
                var statistics = await _rubricService.GetRubricStatisticsAsync(id);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving rubric statistics.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get criterion analytics
        /// </summary>
        [HttpGet("criteria/{criterionId}/analytics")]
        public async Task<ActionResult> GetCriterionAnalytics(int criterionId)
        {
            try
            {
                var analytics = await _rubricService.GetCriterionAnalyticsAsync(criterionId);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving criterion analytics.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get rubric usage report
        /// </summary>
        [HttpGet("{id}/usage-report")]
        public async Task<ActionResult> GetRubricUsageReport(int id)
        {
            try
            {
                var report = await _rubricService.GetRubricUsageReportAsync(id);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving usage report.", error = ex.Message });
            }
        }

        #endregion

        #region Validation and Utilities

        /// <summary>
        /// Validate a rubric
        /// </summary>
        [HttpGet("{id}/validate")]
        public async Task<ActionResult> ValidateRubric(int id)
        {
            try
            {
                var isValid = await _rubricService.ValidateRubricAsync(id);
                return Ok(new { RubricId = id, IsValid = isValid });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while validating the rubric.", error = ex.Message });
            }
        }

        /// <summary>
        /// Check if user can use a rubric
        /// </summary>
        [HttpGet("{rubricId}/can-use/{userId}")]
        public async Task<ActionResult> CanUseRubric(int rubricId, int userId)
        {
            try
            {
                var canUse = await _rubricService.CanUseRubricAsync(rubricId, userId);
                return Ok(new { CanUse = canUse });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while checking rubric permissions.", error = ex.Message });
            }
        }

        #endregion

        #region Import/Export

        /// <summary>
        /// Import rubric from JSON
        /// </summary>
        [HttpPost("import")]
        public async Task<ActionResult<RubricDto>> ImportRubric(IFormFile jsonFile)
        {
            try
            {
                if (jsonFile == null || jsonFile.Length == 0)
                    return BadRequest(new { message = "No JSON file provided." });

                using var stream = jsonFile.OpenReadStream();
                var rubric = await _rubricService.ImportRubricAsync(stream);
                return Ok(rubric);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "Rubric import functionality is not yet implemented." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while importing the rubric.", error = ex.Message });
            }
        }

        /// <summary>
        /// Export rubric to JSON
        /// </summary>
        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportRubric(int id)
        {
            try
            {
                var jsonStream = await _rubricService.ExportRubricAsync(id);
                return File(jsonStream, "application/json", $"rubric_{id}.json");
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "Rubric export functionality is not yet implemented." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while exporting the rubric.", error = ex.Message });
            }
        }

        /// <summary>
        /// Export rubric evaluations
        /// </summary>
        [HttpGet("{id}/export-evaluations")]
        public async Task<ActionResult> ExportRubricEvaluations(int id)
        {
            try
            {
                var csvStream = await _rubricService.ExportRubricEvaluationsAsync(id);
                return File(csvStream, "text/csv", $"rubric_{id}_evaluations.csv");
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "Rubric evaluation export functionality is not yet implemented." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while exporting rubric evaluations.", error = ex.Message });
            }
        }

        #endregion
    }
}