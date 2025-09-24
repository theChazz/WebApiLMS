
using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.UserProgramEnrollment
{
    public class UserProgramEnrollmentRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid User ID.")]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid Program ID.")]
        public int ProgramId { get; set; }

        [StringLength(50)]
        public string Status { get; set; } // e.g., active, completed, withdrawn

        // EnrolledAt will likely be set by the server, so not needed from client
        public DateTime EnrolledAt { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }
    }
} 