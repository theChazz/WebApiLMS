namespace WebApiLMS.DTOs.Rubric
{
    /// <summary>
    /// DTO for returning rubric information
    /// </summary>
    public class RubricDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        // Assessment reference
        public int? AssessmentId { get; set; }
        public string? AssessmentTitle { get; set; }
        
        public decimal MaxScore { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public List<RubricCriterionDto>? Criteria { get; set; }
    }
    
    /// <summary>
    /// DTO for rubric criteria
    /// </summary>
    public class RubricCriterionDto
    {
        public int Id { get; set; }
        public int RubricId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal MaxScore { get; set; }
        public decimal Weight { get; set; }
        public int SortOrder { get; set; }
        public string? LearningOutcomeId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public List<RubricLevelDto>? Levels { get; set; }
    }
    
    /// <summary>
    /// DTO for rubric performance levels
    /// </summary>
    public class RubricLevelDto
    {
        public int Id { get; set; }
        public int CriterionId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Score { get; set; }
        public int SortOrder { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// Simplified DTO for rubric summaries
    /// </summary>
    public class RubricSummaryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? AssessmentId { get; set; }
        public string? AssessmentTitle { get; set; }
        public decimal MaxScore { get; set; }
        public bool IsActive { get; set; }
        public int CriteriaCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    /// <summary>
    /// DTO for rubric evaluation/scoring
    /// </summary>
    public class RubricEvaluationDto
    {
        public int RubricId { get; set; }
        public string? RubricName { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int? SubmissionId { get; set; }
        
        public List<CriterionEvaluationDto>? CriterionEvaluations { get; set; }
        
        public decimal TotalScore { get; set; }
        public decimal MaxPossibleScore { get; set; }
        public decimal Percentage { get; set; }
        
        public string? OverallComments { get; set; }
        public int? EvaluatedBy { get; set; }
        public string? EvaluatorName { get; set; }
        public DateTime? EvaluatedAt { get; set; }
    }
    
    /// <summary>
    /// DTO for individual criterion evaluation
    /// </summary>
    public class CriterionEvaluationDto
    {
        public int CriterionId { get; set; }
        public string? CriterionName { get; set; }
        public decimal MaxScore { get; set; }
        public decimal Weight { get; set; }
        
        public int? SelectedLevelId { get; set; }
        public string? SelectedLevelName { get; set; }
        public decimal Score { get; set; }
        public decimal WeightedScore { get; set; }
        
        public string? Comments { get; set; }
        public bool IsExempt { get; set; }
    }
}