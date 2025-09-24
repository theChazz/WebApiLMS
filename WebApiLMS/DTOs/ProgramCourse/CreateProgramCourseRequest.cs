using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.ProgramCourse
{
    public class CreateProgramCourseRequest
    {
        [Required]
        public int ProgramId { get; set; }

        [Required]
        public int CourseId { get; set; }

        public bool IsCompulsory { get; set; } = true;
    }
}