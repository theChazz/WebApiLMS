using System;

namespace WebApiLMS.DTOs.UserCredential
{
	public class UserCredentialDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string CredentialType { get; set; }
		public string RegistrationNumber { get; set; }
		public int? SetaBodyId { get; set; }
		public string Scope { get; set; }
		public DateTime ValidFrom { get; set; }
		public DateTime ValidTo { get; set; }
		public string Status { get; set; }
		public string EvidenceUrl { get; set; }
	}
}


