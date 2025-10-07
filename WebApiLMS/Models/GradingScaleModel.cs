using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    /// <summary>
    /// Grading scale definitions for grade calculation
    /// </summary>
    public class GradingScaleModel
    {
        [Key]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<GradeBoundaryModel>? GradeBoundaries { get; set; }
        public ICollection<AssessmentModel>? Assessments { get; set; }
    }
}