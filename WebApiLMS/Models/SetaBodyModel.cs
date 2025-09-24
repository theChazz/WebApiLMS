using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.Models
{
    public class SetaBodyModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } // e.g., ETDP-SETA, LGSETA

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}


