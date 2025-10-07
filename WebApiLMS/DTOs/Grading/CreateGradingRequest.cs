using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Grading
{
    /// <summary>
    /// Request DTO for creating new grade records
    /// </summary>
    public class CreateGradingRequest
    {
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public int AssessmentId { get; set; }
        
        public int? SubmissionId { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Score must be non-negative")]
        public decimal Score { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Max score must be greater than 0")]
        public decimal MaxScore { get; set; }
        
        public int? GradingScaleId { get; set; }
        
        public string? GradingMethod { get; set; } = "Manual";
        
        [StringLength(2000)]
        public string? Comments { get; set; }
        
        [StringLength(5000)]
        public string? Feedback { get; set; }
        
        public string? Status { get; set; } = "Draft";
        
        // Additional grading options
        public bool IsExempt { get; set; } = false;
        
        [StringLength(500)]
        public string? ExemptionReason { get; set; }
        
        public bool IsLate { get; set; } = false;
        
        [Range(0, 100, ErrorMessage = "Late penalty must be between 0 and 100")]
        public decimal? LatePenalty { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        // Rubric scores as key-value pairs (criterion ID -> score)
        public Dictionary<string, decimal>? RubricScores { get; set; }
    }
    
    /// <summary>
    /// Request DTO for bulk grading operations
    /// </summary>
    public class BulkGradingRequest
    {
        [Required]
        public int AssessmentId { get; set; }
        
        [Required]
        public List<StudentGradeRequest> Grades { get; set; } = new();
        
        public string? GradingMethod { get; set; } = "Bulk";
        public string? Comments { get; set; }
        public bool PublishGrades { get; set; } = false;
    }
    
    /// <summary>
    /// Individual student grade in bulk operations
    /// </summary>
    public class StudentGradeRequest
    {
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Score { get; set; }
        
        public string? Comments { get; set; }
        public string? Feedback { get; set; }
        public bool IsExempt { get; set; } = false;
        public string? ExemptionReason { get; set; }
    }
}