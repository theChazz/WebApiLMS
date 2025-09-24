// File: WebApiLMS/Models/UserProgramEnrollment.cs
using System;
using System.ComponentModel.DataAnnotations.Schema; // Added for ForeignKey attribute

namespace WebApiLMS.Models
{
    public class UserProgramEnrollmentModel
    {
        public int Id { get; set; }

        [ForeignKey("User")] // Link to the User navigation property
        public int UserId { get; set; }

        [ForeignKey("Program")] // Link to the Program navigation property
        public int ProgramId { get; set; }

        public string Status { get; set; } // e.g., active, completed, withdrawn
        public DateTime EnrolledAt { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }

        // Navigation properties to link to the actual User and Program objects
        public virtual Users User { get; set; }
        public virtual ProgramModel Program { get; set; }
    }
}