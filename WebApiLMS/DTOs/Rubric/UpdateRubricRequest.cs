using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Rubric
{
    /// <summary>
    /// Request DTO for updating existing rubrics
    /// </summary>
    public class UpdateRubricRequest
    {
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public int? AssessmentId { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Max score must be greater than 0")]
        public decimal? MaxScore { get; set; }
        
        public bool? IsActive { get; set; }
    }
    
    /// <summary>
    /// Request DTO for updating rubric criteria
    /// </summary>
    public class UpdateRubricCriterionRequest
    {
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Max score must be greater than 0")]
        public decimal? MaxScore { get; set; }
        
        [Range(0.01, 10.0, ErrorMessage = "Weight must be between 0.01 and 10.0")]
        public decimal? Weight { get; set; }
        
        public int? SortOrder { get; set; }
        
        [StringLength(50)]
        public string? LearningOutcomeId { get; set; }
    }
    
    /// <summary>
    /// Request DTO for updating rubric levels
    /// </summary>
    public class UpdateRubricLevelRequest
    {
        [StringLength(100)]
        public string? Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Score must be non-negative")]
        public decimal? Score { get; set; }
        
        public int? SortOrder { get; set; }
    }
    
    /// <summary>
    /// Request DTO for bulk criterion updates
    /// </summary>
    public class BulkUpdateCriteriaRequest
    {
        [Required]
        public int RubricId { get; set; }
        
        [Required]
        public List<CriterionUpdateItem> Criteria { get; set; } = new();
    }
    
    /// <summary>
    /// Individual criterion update item for bulk operations
    /// </summary>
    public class CriterionUpdateItem
    {
        public int? Id { get; set; } // Null for new criteria
        
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal MaxScore { get; set; }
        
        [Range(0.01, 10.0)]
        public decimal Weight { get; set; } = 1.0m;
        
        public int SortOrder { get; set; }
        
        [StringLength(50)]
        public string? LearningOutcomeId { get; set; }
        
        public List<LevelUpdateItem>? Levels { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }
    
    /// <summary>
    /// Individual level update item for bulk operations
    /// </summary>
    public class LevelUpdateItem
    {
        public int? Id { get; set; } // Null for new levels
        
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Score { get; set; }
        
        public int SortOrder { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }
    
    /// <summary>
    /// Request DTO for reordering criteria
    /// </summary>
    public class ReorderCriteriaRequest
    {
        [Required]
        public int RubricId { get; set; }
        
        [Required]
        public List<CriterionOrderItem> CriteriaOrder { get; set; } = new();
    }
    
    /// <summary>
    /// Criterion order item for reordering
    /// </summary>
    public class CriterionOrderItem
    {
        [Required]
        public int CriterionId { get; set; }
        
        [Required]
        public int SortOrder { get; set; }
    }
    
    /// <summary>
    /// Request DTO for copying rubrics
    /// </summary>
    public class CopyRubricRequest
    {
        [Required]
        public int SourceRubricId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string? NewName { get; set; }
        
        [StringLength(1000)]
        public string? NewDescription { get; set; }
        
        public int? NewAssessmentId { get; set; }
        
        public bool CopyEvaluations { get; set; } = false;
        
        public bool IsActive { get; set; } = true;
    }
}