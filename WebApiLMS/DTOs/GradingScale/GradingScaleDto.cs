namespace WebApiLMS.DTOs.GradingScale
{
    /// <summary>
    /// DTO for returning grading scale information
    /// </summary>
    public class GradingScaleDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public List<GradeBoundaryDto>? GradeBoundaries { get; set; }
        public List<AssessmentSummaryDto>? Assessments { get; set; }
    }
    
    /// <summary>
    /// DTO for grade boundaries within grading scales
    /// </summary>
    public class GradeBoundaryDto
    {
        public int Id { get; set; }
        public int GradingScaleId { get; set; }
        public string? GradingScaleName { get; set; }
        
        public string? Grade { get; set; }
        public string? Description { get; set; }
        public decimal MinPercentage { get; set; }
        public decimal MaxPercentage { get; set; }
        public decimal GradePoints { get; set; }
        
        public bool IsPassingGrade { get; set; }
        public int SortOrder { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// DTO for assessment summary in grading scale context
    /// </summary>
    public class AssessmentSummaryDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal MaxScore { get; set; }
        public string? Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
    
    /// <summary>
    /// Simplified DTO for grading scale summaries
    /// </summary>
    public class GradingScaleSummaryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public int BoundariesCount { get; set; }
        public int AssessmentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    /// <summary>
    /// DTO for grade calculation results
    /// </summary>
    public class GradeCalculationDto
    {
        public decimal Percentage { get; set; }
        public string? LetterGrade { get; set; }
        public string? GradeDescription { get; set; }
        public decimal GradePoints { get; set; }
        public bool IsPassingGrade { get; set; }
        public int GradingScaleId { get; set; }
        public string? GradingScaleName { get; set; }
        public GradeBoundaryDto? AppliedBoundary { get; set; }
    }
    
    /// <summary>
    /// DTO for grading scale statistics
    /// </summary>
    public class GradingScaleStatsDto
    {
        public int GradingScaleId { get; set; }
        public string? Name { get; set; }
        public int TotalBoundaries { get; set; }
        public int ActiveAssessments { get; set; }
        public int TotalGradesAssigned { get; set; }
        public decimal AveragePercentage { get; set; }
        public Dictionary<string, int>? GradeDistribution { get; set; }
        public decimal PassingRate { get; set; }
    }
}