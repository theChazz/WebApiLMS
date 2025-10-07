using Microsoft.AspNetCore.Mvc;
using WebApiLMS.DTOs.Question;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        /// <summary>
        /// Get all questions
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAll()
        {
            var questions = await _questionService.GetAllAsync();
            return Ok(questions);
        }

        /// <summary>
        /// Get questions by assessment ID
        /// </summary>
        [HttpGet("assessment/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetByAssessmentId(int assessmentId)
        {
            var questions = await _questionService.GetByAssessmentIdAsync(assessmentId);
            return Ok(questions);
        }

        /// <summary>
        /// Get question by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetById(int id)
        {
            var question = await _questionService.GetByIdAsync(id);
            
            if (question == null)
            {
                return NotFound($"Question with ID {id} not found.");
            }

            return Ok(question);
        }

        /// <summary>
        /// Create a new question
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create(CreateQuestionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var question = await _questionService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = question.Id }, question);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating question: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing question
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDto>> Update(int id, UpdateQuestionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var question = await _questionService.UpdateAsync(id, request);
                
                if (question == null)
                {
                    return NotFound($"Question with ID {id} not found.");
                }

                return Ok(question);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating question: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a question
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await _questionService.DeleteAsync(id);
                
                if (!success)
                {
                    return NotFound($"Question with ID {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting question: {ex.Message}");
            }
        }

        /// <summary>
        /// Reorder questions within an assessment
        /// </summary>
        [HttpPost("reorder")]
        public async Task<ActionResult> ReorderQuestions(ReorderQuestionsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var success = await _questionService.ReorderQuestionsAsync(request.AssessmentId, request.QuestionOrders);
                
                if (!success)
                {
                    return BadRequest("Failed to reorder questions. Ensure all question IDs are valid.");
                }

                return Ok(new { message = "Questions reordered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reordering questions: {ex.Message}");
            }
        }
    }

    public class ReorderQuestionsRequest
    {
        public int AssessmentId { get; set; }
        public Dictionary<int, int> QuestionOrders { get; set; } = new();
    }
}