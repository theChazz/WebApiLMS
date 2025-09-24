using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.CourseResource
{
    public class CreateCourseResourceRequest
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty; // Document | Video | LiveSession

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1024)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Url]
        [StringLength(2048)]
        public string Url { get; set; } = string.Empty;

        [StringLength(255)]
        public string Provider { get; set; } = string.Empty;

        [StringLength(255)]
        public string MimeType { get; set; } = string.Empty;

        public long? SizeBytes { get; set; }

        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public string Timezone { get; set; } = string.Empty;

        public bool IsPublished { get; set; } = true;
        public string Module { get; set; } = string.Empty;
        public int? SortOrder { get; set; }
    }
}
