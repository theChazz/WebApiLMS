using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Grading
{
    /// <summary>
    /// Request DTO for updating existing grade records
    /// </summary>
    public class UpdateGradingRequest
    {
        [Range(0, double.MaxValue, ErrorMessage = "Score must be non-negative")]
        public decimal? Score { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Max score must be greater than 0")]
        public decimal? MaxScore { get; set; }
        
        public int? GradingScaleId { get; set; }
        
        public string? GradingMethod { get; set; }
        
        [StringLength(2000)]
        public string? Comments { get; set; }
        
        [StringLength(5000)]
        public string? Feedback { get; set; }
        
        public string? Status { get; set; }
        
        // Additional grading options
        public bool? IsExempt { get; set; }
        
        [StringLength(500)]
        public string? ExemptionReason { get; set; }
        
        public bool? IsLate { get; set; }
        
        [Range(0, 100, ErrorMessage = "Late penalty must be between 0 and 100")]
        public decimal? LatePenalty { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        // Rubric scores as key-value pairs (criterion ID -> score)
        public Dictionary<string, decimal>? RubricScores { get; set; }
    }
    
    /// <summary>
    /// Request DTO for publishing grades
    /// </summary>
    public class PublishGradesRequest
    {
        [Required]
        public List<int> GradeIds { get; set; } = new();
        
        public bool SendNotifications { get; set; } = true;
        
        [StringLength(1000)]
        public string? PublishMessage { get; set; }
    }
    
    /// <summary>
    /// Request DTO for grade adjustments
    /// </summary>
    public class GradeAdjustmentRequest
    {
        [Required]
        public int GradeId { get; set; }
        
        [Required]
        public string? AdjustmentType { get; set; } // "Curve", "Bonus", "Penalty", "Override"
        
        [Required]
        public decimal AdjustmentValue { get; set; }
        
        [StringLength(500)]
        public string? Reason { get; set; }
        
        public bool CreateNewVersion { get; set; } = true;
    }
}