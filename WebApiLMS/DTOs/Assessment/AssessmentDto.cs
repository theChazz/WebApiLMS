namespace WebApiLMS.DTOs.Assessment
{
    /// <summary>
    /// DTO for assessment response data
    /// </summary>
    public class AssessmentDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        
        public int AssessmentTypeId { get; set; }
        public string? AssessmentTypeName { get; set; }
        
        public int AssessmentCategoryId { get; set; }
        public string? AssessmentCategoryName { get; set; }
        
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
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        
        // Statistics
        public int QuestionCount { get; set; }
        public int SubmissionCount { get; set; }
    }
}