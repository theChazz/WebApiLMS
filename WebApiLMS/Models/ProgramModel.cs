using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class ProgramModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public string Code { get; set; }

        public string Level { get; set; }

        public string Department { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        [Required]
        public int ProgramTypeId { get; set; }

        [ForeignKey("ProgramTypeId")]
        public ProgramTypeModel? ProgramType { get; set; }

        public int DurationMonths { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 