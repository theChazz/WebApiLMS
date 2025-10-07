using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.Models
{
    /// <summary>
    /// Question types (e.g., Multiple Choice, Short Answer, Essay, etc.)
    /// </summary>
    public class QuestionTypeModel
    {
        [Key]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        // Configuration options
        public bool AllowsMultipleAnswers { get; set; }
        public bool RequiresTextInput { get; set; }
        public bool AllowsFileUpload { get; set; }
        public bool SupportsAutoGrading { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<QuestionModel>? Questions { get; set; }
    }
}