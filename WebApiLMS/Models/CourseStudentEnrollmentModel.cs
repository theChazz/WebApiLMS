using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class CourseStudentEnrollmentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Required]
        public int StudentId { get; set; } // Same as UserId, but must have Role = "Student"
        [ForeignKey("StudentId")]
        public Users Student { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.Now;
        public double? Progress { get; set; } // optional
    }
}