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
        
        // Assessment System DbSets
        public DbSet<AssessmentModel> Assessments { get; set; }
        public DbSet<AssessmentTypeModel> AssessmentTypes { get; set; }
        public DbSet<AssessmentCategoryModel> AssessmentCategories { get; set; }
        public DbSet<QuestionModel> Questions { get; set; }
        public DbSet<QuestionTypeModel> QuestionTypes { get; set; }
        public DbSet<AnswerOptionModel> AnswerOptions { get; set; }
        public DbSet<SubmissionModel> Submissions { get; set; }
        public DbSet<AnswerModel> Answers { get; set; }
        public DbSet<RubricModel> Rubrics { get; set; }
        public DbSet<RubricCriterionModel> RubricCriteria { get; set; }
        public DbSet<RubricLevelModel> RubricLevels { get; set; }
        public DbSet<GradingScaleModel> GradingScales { get; set; }
        public DbSet<GradeBoundaryModel> GradeBoundaries { get; set; }
        public DbSet<GradingModel> Gradings { get; set; }

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

            // Assessment System unique constraints
            modelBuilder.Entity<QuestionModel>()
                .HasIndex(q => new { q.AssessmentId, q.SortOrder })
                .IsUnique();

            modelBuilder.Entity<SubmissionModel>()
                .HasIndex(s => new { s.AssessmentId, s.StudentId, s.AttemptNumber })
                .IsUnique();

            modelBuilder.Entity<AnswerOptionModel>()
                .HasIndex(ao => new { ao.QuestionId, ao.SortOrder })
                .IsUnique();

            modelBuilder.Entity<RubricCriterionModel>()
                .HasIndex(rc => new { rc.RubricId, rc.SortOrder })
                .IsUnique();

            modelBuilder.Entity<GradeBoundaryModel>()
                .HasIndex(gb => new { gb.GradingScaleId, gb.SortOrder })
                .IsUnique();

            // Decimal precision configurations for Assessment System
            modelBuilder.Entity<AnswerModel>()
                .Property(a => a.MarksAwarded)
                .HasPrecision(10, 2);

            modelBuilder.Entity<AnswerModel>()
                .Property(a => a.MaxMarks)
                .HasPrecision(10, 2);

            modelBuilder.Entity<AnswerOptionModel>()
                .Property(ao => ao.Points)
                .HasPrecision(10, 2);

            modelBuilder.Entity<GradeBoundaryModel>()
                .Property(gb => gb.GradePoints)
                .HasPrecision(5, 2);

            modelBuilder.Entity<GradeBoundaryModel>()
                .Property(gb => gb.MinPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<GradeBoundaryModel>()
                .Property(gb => gb.MaxPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<GradingModel>()
                .Property(g => g.Score)
                .HasPrecision(10, 2);

            modelBuilder.Entity<GradingModel>()
                .Property(g => g.MaxScore)
                .HasPrecision(10, 2);

            modelBuilder.Entity<GradingModel>()
                .Property(g => g.Percentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<GradingModel>()
                .Property(g => g.GradePoints)
                .HasPrecision(5, 2);

            modelBuilder.Entity<GradingModel>()
                .Property(g => g.LatePenalty)
                .HasPrecision(5, 2);

            modelBuilder.Entity<RubricCriterionModel>()
                .Property(rc => rc.MaxScore)
                .HasPrecision(10, 2);

            modelBuilder.Entity<RubricCriterionModel>()
                .Property(rc => rc.Weight)
                .HasPrecision(5, 4);

            modelBuilder.Entity<RubricLevelModel>()
                .Property(rl => rl.Score)
                .HasPrecision(10, 2);

            modelBuilder.Entity<RubricModel>()
                .Property(r => r.MaxScore)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SubmissionModel>()
                .Property(s => s.MaxMarks)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SubmissionModel>()
                .Property(s => s.TotalMarks)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SubmissionModel>()
                .Property(s => s.Percentage)
                .HasPrecision(5, 2);

            // Assessment System - Disable ALL cascade delete behavior
            
            // Assessment relationships
            modelBuilder.Entity<AssessmentModel>()
                .HasOne(a => a.Course)
                .WithMany()
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssessmentModel>()
                .HasOne(a => a.AssessmentType)
                .WithMany()
                .HasForeignKey(a => a.AssessmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssessmentModel>()
                .HasOne(a => a.AssessmentCategory)
                .WithMany()
                .HasForeignKey(a => a.AssessmentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Question relationships
            modelBuilder.Entity<QuestionModel>()
                .HasOne(q => q.Assessment)
                .WithMany()
                .HasForeignKey(q => q.AssessmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionModel>()
                .HasOne(q => q.QuestionType)
                .WithMany()
                .HasForeignKey(q => q.QuestionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Answer Option relationships
            modelBuilder.Entity<AnswerOptionModel>()
                .HasOne(ao => ao.Question)
                .WithMany()
                .HasForeignKey(ao => ao.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Submission relationships
            modelBuilder.Entity<SubmissionModel>()
                .HasOne(s => s.Assessment)
                .WithMany()
                .HasForeignKey(s => s.AssessmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubmissionModel>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Answer relationships
            modelBuilder.Entity<AnswerModel>()
                .HasOne(a => a.Submission)
                .WithMany()
                .HasForeignKey(a => a.SubmissionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AnswerModel>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rubric relationships
            modelBuilder.Entity<RubricModel>()
                .HasOne(r => r.Assessment)
                .WithMany()
                .HasForeignKey(r => r.AssessmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RubricCriterionModel>()
                .HasOne(rc => rc.Rubric)
                .WithMany()
                .HasForeignKey(rc => rc.RubricId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RubricLevelModel>()
                .HasOne(rl => rl.Criterion)
                .WithMany()
                .HasForeignKey(rl => rl.CriterionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Grading Scale relationships
            modelBuilder.Entity<GradeBoundaryModel>()
                .HasOne(gb => gb.GradingScale)
                .WithMany()
                .HasForeignKey(gb => gb.GradingScaleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Grading relationships
            modelBuilder.Entity<GradingModel>()
                .HasOne(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GradingModel>()
                .HasOne(g => g.Assessment)
                .WithMany()
                .HasForeignKey(g => g.AssessmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GradingModel>()
                .HasOne(g => g.Submission)
                .WithMany()
                .HasForeignKey(g => g.SubmissionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GradingModel>()
                .HasOne(g => g.Grader)
                .WithMany()
                .HasForeignKey(g => g.GradedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GradingModel>()
                .HasOne(g => g.GradingScale)
                .WithMany()
                .HasForeignKey(g => g.GradingScaleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
