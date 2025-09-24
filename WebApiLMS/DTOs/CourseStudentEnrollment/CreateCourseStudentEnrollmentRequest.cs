namespace WebApiLMS.DTOs.CourseStudentEnrollment
{
    public class CreateCourseStudentEnrollmentRequest
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public double? Progress { get; set; }
    }
}