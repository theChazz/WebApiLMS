using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Submission
{
    public class CreateSubmissionRequest
    {
        [Required]
        public int AssessmentId { get; set; }

        [Required]
        public int StudentId { get; set; }

        public List<CreateAnswerRequest> Answers { get; set; } = new();
    }

    public class CreateAnswerRequest
    {
        [Required]
        public int QuestionId { get; set; }

        [StringLength(5000)]
        public string? AnswerText { get; set; }

        public List<int> SelectedOptionIds { get; set; } = new();
    }
}