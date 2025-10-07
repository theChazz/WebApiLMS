using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class RubricModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? AssessmentId { get; set; }

        [ForeignKey("AssessmentId")]
        public AssessmentModel? Assessment { get; set; }

        public decimal MaxScore { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<RubricCriterionModel>? Criteria { get; set; }
    }
}