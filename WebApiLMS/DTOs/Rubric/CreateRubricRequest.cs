using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Rubric
{
    /// <summary>
    /// Request DTO for creating new rubrics
    /// </summary>
    public class CreateRubricRequest
    {
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public int? AssessmentId { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Max score must be greater than 0")]
        public decimal MaxScore { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Nested criteria creation
        public List<CreateRubricCriterionRequest>? Criteria { get; set; }
    }
    
    /// <summary>
    /// Request DTO for creating rubric criteria
    /// </summary>
    public class CreateRubricCriterionRequest
    {
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Max score must be greater than 0")]
        public decimal MaxScore { get; set; }
        
        [Range(0.01, 10.0, ErrorMessage = "Weight must be between 0.01 and 10.0")]
        public decimal Weight { get; set; } = 1.0m;
        
        public int SortOrder { get; set; }
        
        [StringLength(50)]
        public string? LearningOutcomeId { get; set; }
        
        // Nested levels creation
        public List<CreateRubricLevelRequest>? Levels { get; set; }
    }
    
    /// <summary>
    /// Request DTO for creating rubric levels
    /// </summary>
    public class CreateRubricLevelRequest
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Score must be non-negative")]
        public decimal Score { get; set; }
        
        public int SortOrder { get; set; }
    }
    
    /// <summary>
    /// Request DTO for bulk rubric creation with template
    /// </summary>
    public class CreateRubricFromTemplateRequest
    {
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public int? AssessmentId { get; set; }
        
        [Required]
        public string? TemplateType { get; set; } // "Standard", "Holistic", "Analytic", "SinglePoint"
        
        [Required]
        [Range(2, 10, ErrorMessage = "Number of levels must be between 2 and 10")]
        public int NumberOfLevels { get; set; } = 4;
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal MaxScore { get; set; }
        
        public List<string>? CriterionNames { get; set; }
        public List<string>? LevelNames { get; set; }
    }
    
    /// <summary>
    /// Request DTO for rubric evaluation/scoring
    /// </summary>
    public class RubricEvaluationRequest
    {
        [Required]
        public int RubricId { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        
        public int? SubmissionId { get; set; }
        
        [Required]
        public List<CriterionScoreRequest> CriterionScores { get; set; } = new();
        
        [StringLength(2000)]
        public string? OverallComments { get; set; }
        
        public bool SaveAsDraft { get; set; } = false;
    }
    
    /// <summary>
    /// Request DTO for individual criterion scoring
    /// </summary>
    public class CriterionScoreRequest
    {
        [Required]
        public int CriterionId { get; set; }
        
        public int? SelectedLevelId { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? CustomScore { get; set; }
        
        [StringLength(500)]
        public string? Comments { get; set; }
        
        public bool IsExempt { get; set; } = false;
    }
}