using WebApiLMS.Models;

namespace WebApiLMS.DTOs.UserProgramEnrollment
{
    public class UserProgramEnrollmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string Status { get; set; }
        public DateTime EnrolledAt { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }

        // Full navigation models
        public Users User { get; set; }
        public ProgramModel Program { get; set; }
    }
}
