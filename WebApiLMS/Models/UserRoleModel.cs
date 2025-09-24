using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.Models
{
    // Lookup reference table of roles
    public class UserRoleModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } // Student, Lecturer, Facilitator, Assessor, Moderator, Admin

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}


