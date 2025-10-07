using WebApiLMS.DTOs.Rubric;

namespace WebApiLMS.Services
{
    /// <summary>
    /// Service interface for rubric operations and rubric-based assessment
    /// </summary>
    public interface IRubricService
    {
        // Basic CRUD operations for Rubrics
        Task<IEnumerable<RubricDto>> GetAllRubricsAsync();
        Task<RubricDto?> GetRubricByIdAsync(int id);
        Task<RubricDto> CreateRubricAsync(CreateRubricRequest request);
        Task<RubricDto?> UpdateRubricAsync(int id, UpdateRubricRequest request);
        Task<bool> DeleteRubricAsync(int id);
        
        // Rubric-specific operations
        Task<IEnumerable<RubricSummaryDto>> GetRubricSummariesAsync();
        Task<IEnumerable<RubricDto>> GetRubricsByAssessmentAsync(int assessmentId);
        Task<RubricDto> CreateRubricFromTemplateAsync(CreateRubricFromTemplateRequest request);
        Task<RubricDto> CopyRubricAsync(CopyRubricRequest request);
        
        // Criterion operations
        Task<RubricCriterionDto?> GetCriterionByIdAsync(int criterionId);
        Task<RubricCriterionDto> CreateCriterionAsync(int rubricId, CreateRubricCriterionRequest request);
        Task<RubricCriterionDto?> UpdateCriterionAsync(int criterionId, UpdateRubricCriterionRequest request);
        Task<bool> DeleteCriterionAsync(int criterionId);
        Task<bool> ReorderCriteriaAsync(ReorderCriteriaRequest request);
        Task<IEnumerable<RubricCriterionDto>> BulkUpdateCriteriaAsync(BulkUpdateCriteriaRequest request);
        
        // Level operations  
        Task<RubricLevelDto?> GetLevelByIdAsync(int levelId);
        Task<RubricLevelDto> CreateLevelAsync(int criterionId, CreateRubricLevelRequest request);
        Task<RubricLevelDto?> UpdateLevelAsync(int levelId, UpdateRubricLevelRequest request);
        Task<bool> DeleteLevelAsync(int levelId);
        
        // Rubric evaluation and scoring
        Task<RubricEvaluationDto> EvaluateWithRubricAsync(RubricEvaluationRequest request);
        Task<RubricEvaluationDto?> GetRubricEvaluationAsync(int rubricId, int studentId, int? submissionId = null);
        Task<IEnumerable<RubricEvaluationDto>> GetRubricEvaluationsByAssessmentAsync(int assessmentId);
        Task<RubricEvaluationDto?> UpdateRubricEvaluationAsync(int evaluationId, RubricEvaluationRequest request);
        
        // Analytics and statistics
        Task<object> GetRubricStatisticsAsync(int rubricId);
        Task<object> GetCriterionAnalyticsAsync(int criterionId);
        Task<object> GetRubricUsageReportAsync(int rubricId);
        
        // Validation and utilities
        Task<bool> ValidateRubricAsync(int rubricId);
        Task<bool> CanUseRubricAsync(int rubricId, int userId);
        Task<decimal> CalculateRubricScoreAsync(int rubricId, List<CriterionScoreRequest> criterionScores);
        
        // Import/Export
        Task<RubricDto> ImportRubricAsync(Stream jsonStream);
        Task<Stream> ExportRubricAsync(int rubricId);
        Task<Stream> ExportRubricEvaluationsAsync(int rubricId);
    }
}