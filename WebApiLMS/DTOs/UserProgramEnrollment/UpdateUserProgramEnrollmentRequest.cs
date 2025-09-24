using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.UserProgramEnrollment
{
    public class UpdateUserProgramEnrollmentRequest
    {
        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }
    }
}
