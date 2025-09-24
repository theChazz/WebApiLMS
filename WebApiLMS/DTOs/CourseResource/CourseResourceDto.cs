using WebApiLMS.Models;

namespace WebApiLMS.DTOs.CourseResource
{
    public class CourseResourceDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Provider { get; set; }
        public string MimeType { get; set; }
        public long? SizeBytes { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public string Timezone { get; set; }
        public bool IsPublished { get; set; }
        public string Module { get; set; }
        public int? SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Full Course model for complete information
        public CourseModel Course { get; set; }
    }
}
