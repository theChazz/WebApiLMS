using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class ProgramCourseModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Program")]
        public int ProgramId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public bool IsCompulsory { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ProgramModel Program { get; set; }
        public virtual CourseModel Course { get; set; }
    }
}