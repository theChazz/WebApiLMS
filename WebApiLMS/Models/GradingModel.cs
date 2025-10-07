using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class GradingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Users? Student { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [ForeignKey("AssessmentId")]
        public AssessmentModel? Assessment { get; set; }

        public int? SubmissionId { get; set; }

        [ForeignKey("SubmissionId")]
        public SubmissionModel? Submission { get; set; }

        public decimal Score { get; set; }

        public decimal MaxScore { get; set; }

        public decimal Percentage { get; set; }

        public string? LetterGrade { get; set; }

        public decimal? GradePoints { get; set; }

        public int? GradingScaleId { get; set; }

        [ForeignKey("GradingScaleId")]
        public GradingScaleModel? GradingScale { get; set; }

        public string? GradingMethod { get; set; }

        public string? Comments { get; set; }

        public string? Feedback { get; set; }

        public int? GradedBy { get; set; }

        [ForeignKey("GradedBy")]
        public Users? Grader { get; set; }

        public DateTime? GradedAt { get; set; }

        public string? Status { get; set; }

        public bool IsPublished { get; set; }

        public int Version { get; set; }

        public int? PreviousGradeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
        
        // Additional properties for detailed grading
        public string? RubricScores { get; set; } // JSON string for rubric-based scoring
        public bool IsExempt { get; set; } = false;
        public string? ExemptionReason { get; set; }
        public bool IsLate { get; set; } = false;
        public decimal? LatePenalty { get; set; }
        public string? Notes { get; set; }
    }
}