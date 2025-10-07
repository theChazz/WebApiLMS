using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class SubmissionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [ForeignKey("AssessmentId")]
        public AssessmentModel? Assessment { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Users? Student { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public DateTime? StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? Status { get; set; }

        public decimal? TotalMarks { get; set; }

        public decimal? MaxMarks { get; set; }

        public decimal? Percentage { get; set; }

        public string? Grade { get; set; }

        public DateTime? GradedAt { get; set; }

        public int? GradedBy { get; set; }

        public string? GraderComments { get; set; }

        public string? SubmissionData { get; set; }

        public string? FileUrls { get; set; }

        public int AttemptNumber { get; set; }

        public TimeSpan? TimeTaken { get; set; }

        public bool IsLateSubmission { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<AnswerModel>? Answers { get; set; }
    }
}