using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Question
{
    public class CreateQuestionRequest
    {
        [Required]
        public int AssessmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 1000)]
        public decimal MaxMarks { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        public bool IsRequired { get; set; } = true;

        [Required]
        [StringLength(20)]
        public string Difficulty { get; set; } = string.Empty;

        [StringLength(500)]
        public string? HelpText { get; set; }

        [StringLength(1000)]
        public string? CorrectAnswer { get; set; }

        public List<CreateAnswerOptionRequest> AnswerOptions { get; set; } = new();
    }

    public class CreateAnswerOptionRequest
    {
        [Required]
        [StringLength(500)]
        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        [Range(0, 1000)]
        public decimal Points { get; set; }

        [StringLength(500)]
        public string? Explanation { get; set; }

        [Required]
        public int DisplayOrder { get; set; }
    }
}