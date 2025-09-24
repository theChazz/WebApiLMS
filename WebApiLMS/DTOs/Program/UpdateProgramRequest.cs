using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.Program
{
    public class UpdateProgramRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string Level { get; set; }

        public string Department { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        [Required]
        public int ProgramTypeId { get; set; }

        [Range(1, int.MaxValue)]
        public int DurationMonths { get; set; }
    }
}


