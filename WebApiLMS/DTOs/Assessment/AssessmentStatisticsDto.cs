namespace WebApiLMS.DTOs.Assessment
{
    /// <summary>
    /// DTO for assessment statistics
    /// </summary>
    public class AssessmentStatisticsDto
    {
        public int AssessmentId { get; set; }
        public string? AssessmentTitle { get; set; }
        
        // Question Statistics
        public int TotalQuestions { get; set; }
        public int TotalMarks { get; set; }
        
        // Submission Statistics
        public int TotalSubmissions { get; set; }
        public int CompletedSubmissions { get; set; }
        public int PendingSubmissions { get; set; }
        public int DraftSubmissions { get; set; }
        
        // Performance Statistics
        public double? AverageScore { get; set; }
        public double? HighestScore { get; set; }
        public double? LowestScore { get; set; }
        public double? PassRate { get; set; }
        
        // Timing Statistics
        public TimeSpan? AverageCompletionTime { get; set; }
        public TimeSpan? FastestCompletionTime { get; set; }
        public TimeSpan? SlowestCompletionTime { get; set; }
        
        // Grade Distribution
        public Dictionary<string, int> GradeDistribution { get; set; } = new Dictionary<string, int>();
        
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}