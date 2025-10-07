namespace WebApiLMS.DTOs.Submission
{
    public class SubmissionDto
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentTitle { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal MaxMarks { get; set; }
        public decimal Percentage { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Feedback { get; set; }
        public bool IsGraded { get; set; }
        public DateTime? GradedAt { get; set; }
        public int? GradedByUserId { get; set; }
        public string? GradedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    }

    public class AnswerDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string? AnswerText { get; set; }
        public decimal MarksAwarded { get; set; }
        public decimal MaxMarks { get; set; }
        public string? Feedback { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; }
        public List<int> SelectedOptionIds { get; set; } = new();
    }
}