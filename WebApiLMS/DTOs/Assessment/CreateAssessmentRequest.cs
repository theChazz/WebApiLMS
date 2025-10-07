using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Assessment
{
    /// <summary>
    /// DTO for creating new assessments
    /// </summary>
    public class CreateAssessmentRequest
    {
        public int CourseId { get; set; }
        
        [StringLength(255)]
        public string? Title { get; set; }
        
        public string? Description { get; set; }
        
        public int AssessmentTypeId { get; set; }
        public int AssessmentCategoryId { get; set; }
        
        // Assessment Configuration
        public int MaxMarks { get; set; } = 100;
        public int Duration { get; set; } // minutes
        public int AttemptsAllowed { get; set; } = 1;
        public double WeightingPercentage { get; set; }
        
        // Scheduling
        public DateTime? OpenDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CloseDate { get; set; }
        
        // Moderation
        public bool RequiresModeration { get; set; }
        public bool RequiresExternalModeration { get; set; }
        public int? ModerationPercentage { get; set; } = 25;
        
        // Audit
        public int CreatedBy { get; set; }
    }
}