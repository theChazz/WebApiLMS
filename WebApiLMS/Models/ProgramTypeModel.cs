using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.Models
{
    public class ProgramTypeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } // Degree, Learnership, SkillsProgramme, Apprenticeship

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}


