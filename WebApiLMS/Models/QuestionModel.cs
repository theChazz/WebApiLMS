using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class QuestionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [ForeignKey("AssessmentId")]
        public AssessmentModel? Assessment { get; set; }

        [Required]
        public int QuestionTypeId { get; set; }

        [ForeignKey("QuestionTypeId")]
        public QuestionTypeModel? QuestionType { get; set; }

        public string? QuestionText { get; set; }

        public int Marks { get; set; }

        public int SortOrder { get; set; }

        public string? LearningOutcomeId { get; set; }

        public string? Instructions { get; set; }

        public string? ReferenceText { get; set; }

        public string? MediaUrls { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}