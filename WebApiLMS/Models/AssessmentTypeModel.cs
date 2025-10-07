using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.Models
{
    /// <summary>
    /// Reference table for assessment types (Quiz, Assignment, Examination, etc.)
    /// </summary>
    public class AssessmentTypeModel
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(50)]
        public string? Code { get; set; } // QUIZ, ASSIGNMENT, EXAMINATION, etc.
        
        [StringLength(100)]
        public string? Name { get; set; } // Quiz, Assignment, Examination, etc.
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; }
        
        // Configuration
        public bool AllowMultipleAttempts { get; set; } = false;
        public bool RequiresModeration { get; set; } = false;
        public bool SupportsAutoMarking { get; set; } = false;
        public int DefaultDurationMinutes { get; set; } = 60;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}