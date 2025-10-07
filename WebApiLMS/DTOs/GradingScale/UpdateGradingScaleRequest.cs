using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.GradingScale
{
    /// <summary>
    /// Request DTO for updating existing grading scales
    /// </summary>
    public class UpdateGradingScaleRequest
    {
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }
    
    /// <summary>
    /// Request DTO for updating grade boundaries
    /// </summary>
    public class UpdateGradeBoundaryRequest
    {
        [StringLength(10)]
        public string? Grade { get; set; }
        
        [StringLength(100)]
        public string? Description { get; set; }
        
        [Range(0, 100, ErrorMessage = "Min percentage must be between 0 and 100")]
        public decimal? MinPercentage { get; set; }
        
        [Range(0, 100, ErrorMessage = "Max percentage must be between 0 and 100")]
        public decimal? MaxPercentage { get; set; }
        
        [Range(0, 4.0, ErrorMessage = "Grade points must be between 0 and 4.0")]
        public decimal? GradePoints { get; set; }
        
        public bool? IsPassingGrade { get; set; }
        public int? SortOrder { get; set; }
    }
    
    /// <summary>
    /// Request DTO for bulk boundary updates
    /// </summary>
    public class BulkUpdateGradeBoundariesRequest
    {
        [Required]
        public int GradingScaleId { get; set; }
        
        [Required]
        public List<GradeBoundaryUpdateItem> GradeBoundaries { get; set; } = new();
    }
    
    /// <summary>
    /// Individual grade boundary update item
    /// </summary>
    public class GradeBoundaryUpdateItem
    {
        public int? Id { get; set; } // Null for new boundaries
        
        [Required]
        [StringLength(10)]
        public string? Grade { get; set; }
        
        [StringLength(100)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0, 100)]
        public decimal MinPercentage { get; set; }
        
        [Required]
        [Range(0, 100)]
        public decimal MaxPercentage { get; set; }
        
        [Required]
        [Range(0, 4.0)]
        public decimal GradePoints { get; set; }
        
        public bool IsPassingGrade { get; set; } = true;
        public int SortOrder { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }
    
    /// <summary>
    /// Request DTO for setting default grading scale
    /// </summary>
    public class SetDefaultGradingScaleRequest
    {
        [Required]
        public int GradingScaleId { get; set; }
    }
    
    /// <summary>
    /// Request DTO for copying grading scales
    /// </summary>
    public class CopyGradingScaleRequest
    {
        [Required]
        public int SourceGradingScaleId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string? NewName { get; set; }
        
        [StringLength(1000)]
        public string? NewDescription { get; set; }
        
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
        
        public bool CopyBoundaries { get; set; } = true;
    }
    
    /// <summary>
    /// Request DTO for adjusting grade boundaries
    /// </summary>
    public class AdjustGradeBoundariesRequest
    {
        [Required]
        public int GradingScaleId { get; set; }
        
        [Required]
        public string? AdjustmentType { get; set; } // "Curve", "Shift", "Scale", "Custom"
        
        [Required]
        public decimal AdjustmentValue { get; set; }
        
        [StringLength(500)]
        public string? Reason { get; set; }
        
        public bool CreateNewScale { get; set; } = false;
        public string? NewScaleName { get; set; }
    }
    
    /// <summary>
    /// Request DTO for reordering grade boundaries
    /// </summary>
    public class ReorderGradeBoundariesRequest
    {
        [Required]
        public int GradingScaleId { get; set; }
        
        [Required]
        public List<BoundaryOrderItem> BoundaryOrder { get; set; } = new();
    }
    
    /// <summary>
    /// Boundary order item for reordering
    /// </summary>
    public class BoundaryOrderItem
    {
        [Required]
        public int BoundaryId { get; set; }
        
        [Required]
        public int SortOrder { get; set; }
    }
}