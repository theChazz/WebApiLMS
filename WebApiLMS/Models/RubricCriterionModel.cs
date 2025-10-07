using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class RubricCriterionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RubricId { get; set; }

        [ForeignKey("RubricId")]
        public RubricModel? Rubric { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal MaxScore { get; set; }

        public decimal Weight { get; set; }

        public int SortOrder { get; set; }

        public string? LearningOutcomeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<RubricLevelModel>? Levels { get; set; }
    }
}