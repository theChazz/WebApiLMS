using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class RubricLevelModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CriterionId { get; set; }

        [ForeignKey("CriterionId")]
        public RubricCriterionModel? Criterion { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Score { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}