namespace WebApiLMS.DTOs.CourseStudentEnrollment
{
    public class CourseStudentEnrollmentDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public DateTime EnrolledAt { get; set; }
        public double? Progress { get; set; }
    }
}