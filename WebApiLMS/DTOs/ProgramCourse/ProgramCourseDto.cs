using System;

namespace WebApiLMS.DTOs.ProgramCourse
{
    // DTO for responses, potentially including nested details
    public class ProgramCourseDto
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public bool IsCompulsory { get; set; }
        public DateTime CreatedAt { get; set; }

        // Add nested DTOs if you want to return full Program/Course details
        // public ProgramDto Program { get; set; } 
        // public CourseDto Course { get; set; } 
    }
} 