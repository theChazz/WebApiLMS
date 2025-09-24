using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using WebApiLMS.Models;

namespace WebApiLMS.Data
{
    public class WebApiLMSDbContext : DbContext
    {
        public WebApiLMSDbContext(DbContextOptions<WebApiLMSDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; } // this will become the database name "Users"
        public DbSet<CourseModel> Courses { get; set; } // this will become the database name "Courses"
        public DbSet<ProgramModel> Programs { get; set; } // this will become the database name "Programs"
        public DbSet<ProgramCourseModel> ProgramCourses { get; set; }
        public DbSet<UserProgramEnrollmentModel> UserProgramEnrollments { get; set; } // New DbSet for UserProgramEnrollments
        public DbSet<CourseStudentEnrollmentModel> CourseStudentEnrollments { get; set; } // New DbSet for CourseStudentEnrollments
        public DbSet<CourseLecturerAssignmentModel> CourseLecturerAssignments { get; set; } // New DbSet for CourseLecturerAssignments
        public DbSet<CourseResourceModel> CourseResources { get; set; }
        public DbSet<UserRoleModel> UserRoles { get; set; }
        public DbSet<UserCredentialModel> UserCredentials { get; set; }
        public DbSet<SetaBodyModel> SetaBodies { get; set; }
        public DbSet<ProgramTypeModel> ProgramTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRoleModel>()
                .HasIndex(r => r.Code)
                .IsUnique();

            modelBuilder.Entity<UserCredentialModel>()
                .HasIndex(uc => new { uc.UserId, uc.CredentialType, uc.RegistrationNumber })
                .IsUnique();

            modelBuilder.Entity<SetaBodyModel>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<CourseLecturerAssignmentModel>()
                .HasIndex(a => new { a.CourseId, a.UserId })
                .IsUnique();

            modelBuilder.Entity<ProgramTypeModel>()
                .HasIndex(p => p.Code)
                .IsUnique();
        }
    }

}
