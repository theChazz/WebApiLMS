using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.Models
{
    /// <summary>
    /// Reference table for assessment categories (Formative, Summative, Competency, etc.)
    /// </summary>
    public class AssessmentCategoryModel
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(50)]
        public string? Code { get; set; } // FORMATIVE, SUMMATIVE, COMPETENCY, etc.
        
        [StringLength(100)]
        public string? Name { get; set; } // Formative, Summative, Competency, etc.
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; }
        
        // Assessment rules
        public double MinWeightingPercentage { get; set; } = 0;
        public double MaxWeightingPercentage { get; set; } = 100;
        public bool RequiresExternalModeration { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}