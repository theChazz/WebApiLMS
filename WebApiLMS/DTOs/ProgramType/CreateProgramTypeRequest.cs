using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.ProgramType
{
	public class CreateProgramTypeRequest
	{
		[Required]
		[StringLength(50)]
		public string Code { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }
	}
}


