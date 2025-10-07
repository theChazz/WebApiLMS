using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Assessment
{
    /// <summary>
    /// DTO for updating existing assessments
    /// </summary>
    public class UpdateAssessmentRequest
    {
        [StringLength(255)]
        public string? Title { get; set; }
        
        public string? Description { get; set; }
        
        public int AssessmentTypeId { get; set; }
        public int AssessmentCategoryId { get; set; }
        
        // Assessment Configuration
        public int MaxMarks { get; set; }
        public int Duration { get; set; } // minutes
        public int AttemptsAllowed { get; set; }
        public double WeightingPercentage { get; set; }
        
        // Scheduling
        public DateTime? OpenDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CloseDate { get; set; }
        
        // Moderation
        public bool RequiresModeration { get; set; }
        public bool RequiresExternalModeration { get; set; }
        public int? ModerationPercentage { get; set; }
        
        // Status
        public bool IsPublished { get; set; }
        public bool IsActive { get; set; }
        
        // Audit
        public int? ModifiedBy { get; set; }
    }
}