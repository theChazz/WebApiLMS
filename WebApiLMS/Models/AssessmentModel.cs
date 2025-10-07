using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class AssessmentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public CourseModel? Course { get; set; }

        [StringLength(255)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public int AssessmentTypeId { get; set; }

        [ForeignKey("AssessmentTypeId")]
        public AssessmentTypeModel? AssessmentType { get; set; }

        [Required]
        public int AssessmentCategoryId { get; set; }

        [ForeignKey("AssessmentCategoryId")]
        public AssessmentCategoryModel? AssessmentCategory { get; set; }

        public int MaxMarks { get; set; }

        public int Duration { get; set; }

        public int AttemptsAllowed { get; set; }

        public double WeightingPercentage { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public bool RequiresModeration { get; set; }

        public bool RequiresExternalModeration { get; set; }

        public int? ModerationPercentage { get; set; }

        public bool IsPublished { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}