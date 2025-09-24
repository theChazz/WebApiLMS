using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public enum CourseResourceType
    {
        Document = 1,
        Video = 2,
        LiveSession = 3
    }

    public class CourseResourceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }

        public CourseModel Course { get; set; }

        [Required]
        public CourseResourceType Type { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1024)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Url]
        [StringLength(2048)]
        public string Url { get; set; } = string.Empty;

        // Optional metadata
        [StringLength(255)]
        public string Provider { get; set; } = string.Empty; // teams | zoom | youtube | vimeo | file | other

        [StringLength(255)]
        public string MimeType { get; set; } = string.Empty;

        public long? SizeBytes { get; set; }

        // Scheduling for LiveSession
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }

        [StringLength(100)]
        public string Timezone { get; set; } = string.Empty;

        // Visibility & ordering
        public bool IsPublished { get; set; } = true;
        [StringLength(100)]
        public string Module { get; set; } = string.Empty; // e.g., Week 1, Module A
        public int? SortOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
