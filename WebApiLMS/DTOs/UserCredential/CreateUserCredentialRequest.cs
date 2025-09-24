using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiLMS.DTOs.UserCredential
{
	public class CreateUserCredentialRequest
	{
		[Required]
		public int UserId { get; set; }

		[Required]
		[StringLength(50)]
		public string CredentialType { get; set; }

		[Required]
		[StringLength(100)]
		public string RegistrationNumber { get; set; }

		public int? SetaBodyId { get; set; }
		[StringLength(512)]
		public string Scope { get; set; }
		public DateTime ValidFrom { get; set; }
		public DateTime ValidTo { get; set; }
		[StringLength(50)]
		public string Status { get; set; }
		[StringLength(2048)]
		public string EvidenceUrl { get; set; }
	}
}


