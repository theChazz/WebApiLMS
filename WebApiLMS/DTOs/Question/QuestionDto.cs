namespace WebApiLMS.DTOs.Question
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentTitle { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public decimal MaxMarks { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsRequired { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public string? HelpText { get; set; }
        public string? CorrectAnswer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<AnswerOptionDto> AnswerOptions { get; set; } = new();
    }

    public class AnswerOptionDto
    {
        public int Id { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public decimal Points { get; set; }
        public string? Explanation { get; set; }
        public int DisplayOrder { get; set; }
    }
}