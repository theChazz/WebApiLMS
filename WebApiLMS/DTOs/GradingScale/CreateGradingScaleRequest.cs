using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.GradingScale
{
    /// <summary>
    /// Request DTO for creating new grading scales
    /// </summary>
    public class CreateGradingScaleRequest
    {
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
        
        // Nested grade boundaries creation
        public List<CreateGradeBoundaryRequest>? GradeBoundaries { get; set; }
    }
    
    /// <summary>
    /// Request DTO for creating grade boundaries
    /// </summary>
    public class CreateGradeBoundaryRequest
    {
        [Required]
        [StringLength(10)]
        public string? Grade { get; set; }
        
        [StringLength(100)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0, 100, ErrorMessage = "Min percentage must be between 0 and 100")]
        public decimal MinPercentage { get; set; }
        
        [Required]
        [Range(0, 100, ErrorMessage = "Max percentage must be between 0 and 100")]
        public decimal MaxPercentage { get; set; }
        
        [Required]
        [Range(0, 4.0, ErrorMessage = "Grade points must be between 0 and 4.0")]
        public decimal GradePoints { get; set; }
        
        public bool IsPassingGrade { get; set; } = true;
        public int SortOrder { get; set; }
    }
    
    /// <summary>
    /// Request DTO for creating grading scale from template
    /// </summary>
    public class CreateGradingScaleFromTemplateRequest
    {
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        public string? TemplateType { get; set; } // "Standard", "Plus/Minus", "Pass/Fail", "Percentage", "Custom"
        
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
        
        // Custom template parameters
        public decimal? PassingThreshold { get; set; } = 50.0m;
        public bool IncludePlusMinusGrades { get; set; } = false;
        public List<string>? CustomGrades { get; set; }
        public List<decimal>? CustomPercentages { get; set; }
        public List<decimal>? CustomGradePoints { get; set; }
    }
    
    /// <summary>
    /// Request DTO for bulk grade boundary creation
    /// </summary>
    public class BulkCreateGradeBoundariesRequest
    {
        [Required]
        public int GradingScaleId { get; set; }
        
        [Required]
        public List<CreateGradeBoundaryRequest> GradeBoundaries { get; set; } = new();
        
        public bool ReplaceExisting { get; set; } = false;
    }
    
    /// <summary>
    /// Request DTO for grade calculation
    /// </summary>
    public class CalculateGradeRequest
    {
        [Required]
        public int GradingScaleId { get; set; }
        
        [Required]
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100")]
        public decimal Percentage { get; set; }
    }
    
    /// <summary>
    /// Request DTO for batch grade calculations
    /// </summary>
    public class BatchCalculateGradesRequest
    {
        [Required]
        public int GradingScaleId { get; set; }
        
        [Required]
        public List<PercentageItem> Percentages { get; set; } = new();
    }
    
    /// <summary>
    /// Individual percentage item for batch calculations
    /// </summary>
    public class PercentageItem
    {
        public int Id { get; set; } // Reference ID (student ID, submission ID, etc.)
        
        [Required]
        [Range(0, 100)]
        public decimal Percentage { get; set; }
        
        public string? Context { get; set; } // Additional context information
    }
}