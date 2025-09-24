namespace WebApiLMS.DTOs.ProgramCourse
{
    public class UpdateProgramCourseRequest
    {
        // Typically, only IsCompulsory might be updatable for this relationship
        public bool IsCompulsory { get; set; }
    }
} 