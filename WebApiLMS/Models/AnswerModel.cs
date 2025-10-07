using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class AnswerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SubmissionId { get; set; }

        [ForeignKey("SubmissionId")]
        public SubmissionModel? Submission { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public QuestionModel? Question { get; set; }

        public string? AnswerText { get; set; }

        public string? SelectedOptionIds { get; set; }

        public string? FileUrls { get; set; }

        public decimal? MarksAwarded { get; set; }

        public decimal? MaxMarks { get; set; }

        public bool? IsCorrect { get; set; }

        public string? Feedback { get; set; }

        public DateTime? GradedAt { get; set; }

        public int? GradedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}