using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class CourseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string CourseName { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Difficulty { get; set; }

        public string Syllabus { get; set; }

        public string Prerequisites { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 