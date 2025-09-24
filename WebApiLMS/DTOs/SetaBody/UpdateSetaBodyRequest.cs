using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.SetaBody
{
	public class UpdateSetaBodyRequest
	{
		[Required]
		[StringLength(50)]
		public string Code { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }
	}
}


