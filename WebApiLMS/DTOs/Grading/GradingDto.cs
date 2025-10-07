namespace WebApiLMS.DTOs.Grading
{
    /// <summary>
    /// DTO for returning grading information
    /// </summary>
    public class GradingDto
    {
        public int Id { get; set; }
        
        // Student information
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        
        // Assessment information
        public int AssessmentId { get; set; }
        public string? AssessmentTitle { get; set; }
        
        // Submission reference
        public int? SubmissionId { get; set; }
        
        // Grade details
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public decimal Percentage { get; set; }
        public string? LetterGrade { get; set; }
        public decimal? GradePoints { get; set; }
        
        // Grading scale
        public int? GradingScaleId { get; set; }
        public string? GradingScaleName { get; set; }
        
        // Grading metadata
        public string? GradingMethod { get; set; }
        public string? Comments { get; set; }
        public string? Feedback { get; set; }
        
        // Grader information
        public int? GradedBy { get; set; }
        public string? GraderName { get; set; }
        public DateTime? GradedAt { get; set; }
        
        // Status
        public string? Status { get; set; }
        public bool IsPublished { get; set; }
        public int Version { get; set; }
        
        // Additional details
        public bool IsExempt { get; set; }
        public string? ExemptionReason { get; set; }
        public bool IsLate { get; set; }
        public decimal? LatePenalty { get; set; }
        public string? Notes { get; set; }
        
        // Rubric scores (parsed from JSON)
        public Dictionary<string, decimal>? RubricScores { get; set; }
        
        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// Simplified DTO for grade summaries
    /// </summary>
    public class GradeSummaryDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int AssessmentId { get; set; }
        public string? AssessmentTitle { get; set; }
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public decimal Percentage { get; set; }
        public string? LetterGrade { get; set; }
        public string? Status { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? GradedAt { get; set; }
    }
}