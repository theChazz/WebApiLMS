using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Submission
{
    public class UpdateSubmissionRequest
    {
        public List<UpdateAnswerRequest> Answers { get; set; } = new();
    }

    public class UpdateAnswerRequest
    {
        [Required]
        public int QuestionId { get; set; }

        [StringLength(5000)]
        public string? AnswerText { get; set; }

        public List<int> SelectedOptionIds { get; set; } = new();
    }

    public class GradeSubmissionRequest
    {
        [StringLength(2000)]
        public string? Feedback { get; set; }

        public List<GradeAnswerRequest> Answers { get; set; } = new();
    }

    public class GradeAnswerRequest
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal MarksAwarded { get; set; }

        [StringLength(1000)]
        public string? Feedback { get; set; }
    }
}