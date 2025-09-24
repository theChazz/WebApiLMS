using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class CourseLecturerAssignmentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Required]
        public int UserId { get; set; } // Assigned user id (Lecturer/Facilitator/Assessor/Moderator)
        [ForeignKey("UserId")]
        public Users User { get; set; }

        // No Role field here; the user's role is determined via Users.UserRole

        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}