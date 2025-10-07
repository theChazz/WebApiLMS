using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    /// <summary>
    /// Grade boundaries within grading scales
    /// </summary>
    public class GradeBoundaryModel
    {
        [Key]
        public int Id { get; set; }
        
        public int GradingScaleId { get; set; }
        [ForeignKey("GradingScaleId")]
        public GradingScaleModel? GradingScale { get; set; }
        
        public string? Grade { get; set; } // A, B, C, D, F, etc.
        public string? Description { get; set; } // Distinction, Credit, Pass, Fail
        public decimal MinPercentage { get; set; }
        public decimal MaxPercentage { get; set; }
        public decimal GradePoints { get; set; } // For GPA calculation
        
        public bool IsPassingGrade { get; set; }
        public int SortOrder { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}