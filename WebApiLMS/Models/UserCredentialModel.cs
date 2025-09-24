using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiLMS.Models
{
    public class UserCredentialModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }

        // "Assessor", "Moderator", "Facilitator" (credentialed)
        [Required]
        [StringLength(50)]
        public string CredentialType { get; set; }

        // e.g., ETDP registration number / scope number
        [Required]
        [StringLength(100)]
        public string RegistrationNumber { get; set; }

        // Accrediting SETA/QCTO body
        public int? SetaBodyId { get; set; }

        [ForeignKey("SetaBodyId")]
        public SetaBodyModel SetaBody { get; set; }

        // Scope of practice, e.g., unit standards, qualification codes
        [StringLength(512)]
        public string Scope { get; set; }

        // Validity period and status tracking
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active / Expired / PendingVerification / Revoked

        [StringLength(2048)]
        public string EvidenceUrl { get; set; } // Link to uploaded certificate or portfolio evidence
    }
}


