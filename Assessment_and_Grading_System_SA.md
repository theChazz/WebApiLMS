# Assessment and Grading System for South African LMS

## Overview

This document outlines the comprehensive assessment and grading system designed specifically for South African educational institutions, ensuring compliance with **SAQA (South African Qualifications Authority)**, **NQF (National Qualifications Framework)**, and **SETA (Sector Education and Training Authority)** requirements.

## Table of Contents

1. [South African Assessment Standards](#south-african-assessment-standards)
2. [Assessment Types](#assessment-types)
3. [Data Models](#data-models)
4. [Course-Assessment Integration](#course-assessment-integration)
5. [End-to-End Workflow](#end-to-end-workflow)
6. [Grading Scheme](#grading-scheme)
7. [Compliance and Reporting](#compliance-and-reporting)
8. [API Endpoints](#api-endpoints)
9. [Implementation Guide](#implementation-guide)

---

## South African Assessment Standards

### NQF Levels and Requirements

The system supports all **10 NQF levels** as defined by SAQA:

| NQF Level | Qualification Type | Assessment Requirements |
|-----------|-------------------|------------------------|
| 1-4 | General Education | Continuous assessment + Summative |
| 5-6 | FET/TVET | Portfolio of Evidence (PoE) + Workplace assessment |
| 7 | Bachelor's Degree | Formative + Summative + Research project |
| 8 | Honours/PGDip | Advanced research + Dissertation |
| 9 | Master's Degree | Thesis + Research methodology |
| 10 | Doctoral Degree | Original research + Doctoral thesis |

### SETA Compliance

- **Recognition of Prior Learning (RPL)** assessment support
- **Workplace-based Assessment (WBA)** integration
- **Portfolio of Evidence (PoE)** management
- **Competency-based Assessment** for unit standards
- **Moderation** and **Verification** processes

---

## Assessment Types

### 1. Formative Assessments
**Purpose**: Continuous monitoring and feedback during learning

#### Types:
- **Quizzes**: Short knowledge checks (5-20 questions)
- **Discussion Forums**: Participation and quality of contributions
- **Assignments**: Short written tasks and exercises
- **Self-assessments**: Learner reflection activities
- **Peer assessments**: Student-to-student evaluation

#### Characteristics:
- Low stakes (5-15% of final grade)
- Frequent (weekly/bi-weekly)
- Immediate feedback
- Multiple attempts allowed

### 2. Summative Assessments
**Purpose**: Evaluate learning achievement at course/module completion

#### Types:
- **Written Examinations**: Formal testing (2-4 hours)
- **Practical Examinations**: Hands-on skills testing
- **Projects**: Comprehensive research or practical work
- **Case Studies**: Real-world problem solving
- **Portfolio Assessments**: Collection of work over time

#### Characteristics:
- High stakes (60-80% of final grade)
- Limited attempts (usually 1-2)
- Formal moderation required
- Detailed rubrics

### 3. Competency-Based Assessments (SETA)
**Purpose**: Evaluate against specific unit standards

#### Types:
- **Knowledge Assessment**: Written/oral testing of theory
- **Practical Assessment**: Skills demonstration
- **Workplace Assessment**: On-the-job evaluation
- **Integrated Assessment**: Combined knowledge + practical
- **Recognition of Prior Learning (RPL)**: Credit for experience

#### Characteristics:
- **Competent/Not Yet Competent** grading
- Must meet all assessment criteria
- Workplace assessor involvement
- Evidence requirements documentation

### 4. Authentic Assessments
**Purpose**: Real-world application of knowledge and skills

#### Types:
- **Simulations**: Controlled real-world scenarios
- **Work-Integrated Learning (WIL)**: Industry placement
- **Capstone Projects**: Final year comprehensive projects
- **Research Projects**: Independent investigation
- **Community Engagement**: Service learning

---

## Data Models

### Core Assessment Models

```csharp
public class AssessmentModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public CourseModel Course { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    public int AssessmentTypeId { get; set; }
    [ForeignKey("AssessmentTypeId")]
    public AssessmentTypeModel AssessmentType { get; set; }
    
    [Required]
    public int AssessmentCategoryId { get; set; }
    [ForeignKey("AssessmentCategoryId")]
    public AssessmentCategoryModel AssessmentCategory { get; set; }
    
    // NQF Compliance
    public int NQFLevel { get; set; }
    public string UnitStandardId { get; set; } // SAQA unit standard
    
    // Assessment Configuration
    public int MaxMarks { get; set; } = 100;
    public int Duration { get; set; } // minutes
    public int AttemptsAllowed { get; set; } = 1;
    public double WeightingPercentage { get; set; }
    
    // Scheduling
    public DateTime? OpenDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CloseDate { get; set; }
    
    // Moderation
    public bool RequiresModeration { get; set; }
    public bool RequiresExternalModeration { get; set; }
    public int? ModerationPercentage { get; set; } = 25; // % of papers to moderate
    
    // Status
    public bool IsPublished { get; set; }
    public bool IsActive { get; set; }
    
    // Audit
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    // Navigation Properties
    public List<QuestionModel> Questions { get; set; }
    public List<SubmissionModel> Submissions { get; set; }
    public List<AssessmentModerationModel> Moderations { get; set; }
    public List<AssessmentRubricModel> Rubrics { get; set; }
}

// Reference Tables for Assessment Configuration

public class AssessmentTypeModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Code { get; set; } // QUIZ, ASSIGNMENT, EXAMINATION, etc.
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // Quiz, Assignment, Examination, etc.
    
    [StringLength(500)]
    public string Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    
    // Configuration
    public bool AllowMultipleAttempts { get; set; } = false;
    public bool RequiresModeration { get; set; } = false;
    public bool SupportsAutoMarking { get; set; } = false;
    public int DefaultDurationMinutes { get; set; } = 60;
    
    // Localization support
    [StringLength(100)]
    public string DisplayNameEn { get; set; }
    [StringLength(100)]
    public string DisplayNameAf { get; set; }
    [StringLength(100)]
    public string DisplayNameZu { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public class AssessmentCategoryModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Code { get; set; } // FORMATIVE, SUMMATIVE, COMPETENCY, etc.
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // Formative, Summative, Competency, etc.
    
    [StringLength(500)]
    public string Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    
    // Assessment rules
    public double MinWeightingPercentage { get; set; } = 0;
    public double MaxWeightingPercentage { get; set; } = 100;
    public bool RequiresExternalModeration { get; set; } = false;
    
    // SETA/NQF compliance
    public bool IsSETACompliant { get; set; } = false;
    public string NQFLevelsApplicable { get; set; } // JSON array: [1,2,3,4,5,6,7,8,9,10]
    
    // Localization support
    [StringLength(100)]
    public string DisplayNameEn { get; set; }
    [StringLength(100)]
    public string DisplayNameAf { get; set; }
    [StringLength(100)]
    public string DisplayNameZu { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
```

### Question and Answer Models

```csharp
public class QuestionModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int AssessmentId { get; set; }
    [ForeignKey("AssessmentId")]
    public AssessmentModel Assessment { get; set; }
    
    [Required]
    public int QuestionTypeId { get; set; }
    [ForeignKey("QuestionTypeId")]
    public QuestionTypeModel QuestionType { get; set; }
    
    [Required]
    public string QuestionText { get; set; }
    
    public int Marks { get; set; }
    public int SortOrder { get; set; }
    
    // Cognitive Level (Bloom's Taxonomy)
    public int CognitiveLevelId { get; set; }
    [ForeignKey("CognitiveLevelId")]
    public CognitiveLevelModel CognitiveLevel { get; set; }
    
    // Learning Outcome mapping
    public string LearningOutcomeId { get; set; }
    
    // Question metadata
    public string Instructions { get; set; }
    public string ReferenceText { get; set; }
    public List<string> MediaUrls { get; set; }
    
    // Navigation Properties
    public List<QuestionOptionModel> Options { get; set; }
    public List<AnswerModel> Answers { get; set; }
}

// Question Types Reference Table
public class QuestionTypeModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Code { get; set; } // MULTIPLE_CHOICE, TRUE_FALSE, SHORT_ANSWER, etc.
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // Multiple Choice, True/False, Short Answer, etc.
    
    [StringLength(500)]
    public string Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    
    // Technical configuration
    public bool SupportsAutoMarking { get; set; } = false;
    public bool RequiresFileUpload { get; set; } = false;
    public bool SupportsMultipleAnswers { get; set; } = false;
    public bool RequiresManualMarking { get; set; } = true;
    
    // UI Configuration
    public string InputType { get; set; } // text, radio, checkbox, file, textarea
    public int MaxCharacterLimit { get; set; } = 0; // 0 = no limit
    public string ValidationRules { get; set; } // JSON configuration
    
    // Localization support
    [StringLength(100)]
    public string DisplayNameEn { get; set; }
    [StringLength(100)]
    public string DisplayNameAf { get; set; }
    [StringLength(100)]
    public string DisplayNameZu { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

// Cognitive Levels Reference Table (Bloom's Taxonomy)
public class CognitiveLevelModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Code { get; set; } // REMEMBER, UNDERSTAND, APPLY, etc.
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // Remember, Understand, Apply, etc.
    
    [StringLength(500)]
    public string Description { get; set; }
    
    public int BloomLevel { get; set; } // 1-6 for Bloom's taxonomy
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    
    // Educational metadata
    public string LearningVerbs { get; set; } // JSON array of action verbs
    public string ExampleActivities { get; set; } // JSON array of example activities
    public string AssessmentTips { get; set; } // Guidance for question writing
    
    // Localization support
    [StringLength(100)]
    public string DisplayNameEn { get; set; }
    [StringLength(100)]
    public string DisplayNameAf { get; set; }
    [StringLength(100)]
    public string DisplayNameZu { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
```

### Submission and Grading Models

```csharp
public class SubmissionModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int AssessmentId { get; set; }
    [ForeignKey("AssessmentId")]
    public AssessmentModel Assessment { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    [ForeignKey("StudentId")]
    public Users Student { get; set; }
    
    public int AttemptNumber { get; set; } = 1;
    
    // Submission Data
    public string ResponseData { get; set; } // JSON serialized answers
    public List<string> AttachmentUrls { get; set; }
    
    // Timing
    public DateTime StartedAt { get; set; }
    public DateTime SubmittedAt { get; set; }
    public int TimeSpentMinutes { get; set; }
    
    // Grading
    public int SubmissionStatusId { get; set; }
    [ForeignKey("SubmissionStatusId")]
    public SubmissionStatusModel Status { get; set; }
    public double? RawScore { get; set; }
    public double? PercentageScore { get; set; }
    public string Grade { get; set; } // A+, B, C, etc.
    public bool? IsPass { get; set; }
    
    // Feedback
    public string Feedback { get; set; }
    public DateTime? FeedbackDate { get; set; }
    
    // Marking
    public int? MarkerId { get; set; }
    [ForeignKey("MarkerId")]
    public Users Marker { get; set; }
    public DateTime? MarkedAt { get; set; }
    
    // Moderation
    public int? ModeratorId { get; set; }
    [ForeignKey("ModeratorId")]
    public Users Moderator { get; set; }
    public DateTime? ModeratedAt { get; set; }
    public bool RequiresModeration { get; set; }
    public bool IsModerated { get; set; }
    
    // Appeals
    public bool HasAppeal { get; set; }
    public DateTime? AppealDate { get; set; }
    public string AppealReason { get; set; }
    public int? AppealStatusId { get; set; }
    [ForeignKey("AppealStatusId")]
    public AppealStatusModel AppealStatus { get; set; }
    
    // Navigation Properties
    public List<AnswerModel> Answers { get; set; }
    public List<ModerationRecordModel> ModerationRecords { get; set; }
}

// Submission Status Reference Table
public class SubmissionStatusModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Code { get; set; } // DRAFT, SUBMITTED, MARKING, etc.
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // Draft, Submitted, Marking, etc.
    
    [StringLength(500)]
    public string Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    
    // Workflow configuration
    public bool IsInitialStatus { get; set; } = false;
    public bool IsFinalStatus { get; set; } = false;
    public bool AllowsEditing { get; set; } = true;
    public bool RequiresApproval { get; set; } = false;
    public bool IsVisibleToStudent { get; set; } = true;
    
    // Status color for UI
    [StringLength(20)]
    public string StatusColor { get; set; } // success, warning, danger, info
    
    // Localization support
    [StringLength(100)]
    public string DisplayNameEn { get; set; }
    [StringLength(100)]
    public string DisplayNameAf { get; set; }
    [StringLength(100)]
    public string DisplayNameZu { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

// Appeal Status Reference Table
public class AppealStatusModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Code { get; set; } // PENDING, APPROVED, REJECTED, etc.
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // Pending, Approved, Rejected, etc.
    
    [StringLength(500)]
    public string Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    
    // Appeal workflow
    public bool IsInitialStatus { get; set; } = false;
    public bool IsFinalStatus { get; set; } = false;
    public bool RequiresAction { get; set; } = true;
    public int TimeoutDays { get; set; } = 0; // Auto-timeout after days
    
    // Status color for UI
    [StringLength(20)]
    public string StatusColor { get; set; } // success, warning, danger, info
    
    // Localization support
    [StringLength(100)]
    public string DisplayNameEn { get; set; }
    [StringLength(100)]
    public string DisplayNameAf { get; set; }
    [StringLength(100)]
    public string DisplayNameZu { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
```

### Moderation Models

```csharp
public class ModerationRecordModel
{
    [Key]
    public int Id { get; set; }
    
    public int SubmissionId { get; set; }
    [ForeignKey("SubmissionId")]
    public SubmissionModel Submission { get; set; }
    
    public int ModeratorId { get; set; }
    [ForeignKey("ModeratorId")]
    public Users Moderator { get; set; }
    
    public int ModerationTypeId { get; set; }
    [ForeignKey("ModerationTypeId")]
    public ModerationTypeModel Type { get; set; }
    
    // Original vs Moderated Scores
    public double OriginalScore { get; set; }
    public double ModeratedScore { get; set; }
    public double ScoreDifference { get; set; }
    
    // Moderation Feedback
    public string Comments { get; set; }
    public bool AgreesWithMarking { get; set; }
    public string RecommendedAction { get; set; }
    
    public DateTime ModeratedAt { get; set; }
}

// Moderation Type Reference Table
public class ModerationTypeModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Code { get; set; } // INTERNAL, EXTERNAL, SETA, BLIND
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // Internal, External, SETA, Blind
    
    [StringLength(500)]
    public string Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    
    // Moderation rules
    public bool RequiresExternalModerator { get; set; } = false;
    public bool RequiresSETAApproval { get; set; } = false;
    public bool IsAnonymous { get; set; } = false;
    public int MinimumSamplePercentage { get; set; } = 25;
    public int MaxDaysToComplete { get; set; } = 10;
    
    // Cost implications
    public decimal? CostPerSubmission { get; set; }
    public bool RequiresBudgetApproval { get; set; } = false;
    
    // Localization support
    [StringLength(100)]
    public string DisplayNameEn { get; set; }
    [StringLength(100)]
    public string DisplayNameAf { get; set; }
    [StringLength(100)]
    public string DisplayNameZu { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
```

### Database Context Updates

```csharp
public class WebApiLMSDbContext : DbContext
{
    // Existing DbSets...
    public DbSet<Users> Users { get; set; }
    public DbSet<CourseModel> Courses { get; set; }
    
    // New Assessment System DbSets
    public DbSet<AssessmentModel> Assessments { get; set; }
    public DbSet<QuestionModel> Questions { get; set; }
    public DbSet<SubmissionModel> Submissions { get; set; }
    public DbSet<AnswerModel> Answers { get; set; }
    public DbSet<ModerationRecordModel> ModerationRecords { get; set; }
    
    // Reference Tables (instead of enums)
    public DbSet<AssessmentTypeModel> AssessmentTypes { get; set; }
    public DbSet<AssessmentCategoryModel> AssessmentCategories { get; set; }
    public DbSet<QuestionTypeModel> QuestionTypes { get; set; }
    public DbSet<CognitiveLevelModel> CognitiveLevels { get; set; }
    public DbSet<SubmissionStatusModel> SubmissionStatuses { get; set; }
    public DbSet<AppealStatusModel> AppealStatuses { get; set; }
    public DbSet<ModerationTypeModel> ModerationTypes { get; set; }
    
    // Assessment Planning
    public DbSet<CourseAssessmentPlanModel> CourseAssessmentPlans { get; set; }
    public DbSet<LearningOutcomeModel> LearningOutcomes { get; set; }
    public DbSet<AssessmentLearningOutcomeModel> AssessmentLearningOutcomes { get; set; }
    
    // Progress and Analytics
    public DbSet<StudentProgressModel> StudentProgress { get; set; }
    public DbSet<AuditTrailModel> AuditTrails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Existing configurations...
        
        // Assessment Type unique constraints
        modelBuilder.Entity<AssessmentTypeModel>()
            .HasIndex(at => at.Code)
            .IsUnique();
            
        modelBuilder.Entity<AssessmentCategoryModel>()
            .HasIndex(ac => ac.Code)
            .IsUnique();
            
        modelBuilder.Entity<QuestionTypeModel>()
            .HasIndex(qt => qt.Code)
            .IsUnique();
            
        modelBuilder.Entity<CognitiveLevelModel>()
            .HasIndex(cl => cl.Code)
            .IsUnique();
            
        modelBuilder.Entity<SubmissionStatusModel>()
            .HasIndex(ss => ss.Code)
            .IsUnique();
            
        modelBuilder.Entity<AppealStatusModel>()
            .HasIndex(aps => aps.Code)
            .IsUnique();
            
        modelBuilder.Entity<ModerationTypeModel>()
            .HasIndex(mt => mt.Code)
            .IsUnique();

        // Assessment relationships
        modelBuilder.Entity<AssessmentModel>()
            .HasIndex(a => new { a.CourseId, a.Title })
            .IsUnique();
            
        modelBuilder.Entity<SubmissionModel>()
            .HasIndex(s => new { s.AssessmentId, s.StudentId, s.AttemptNumber })
            .IsUnique();

        // Performance indexes
        modelBuilder.Entity<AssessmentModel>()
            .HasIndex(a => new { a.CourseId, a.AssessmentTypeId, a.IsPublished });
            
        modelBuilder.Entity<SubmissionModel>()
            .HasIndex(s => new { s.StudentId, s.SubmissionStatusId });
            
        modelBuilder.Entity<QuestionModel>()
            .HasIndex(q => new { q.AssessmentId, q.SortOrder });
    }
}
```

### Initial Data Seeding

```csharp
public static class AssessmentDataSeeder
{
    public static void SeedReferenceData(ModelBuilder modelBuilder)
    {
        // Assessment Types
        modelBuilder.Entity<AssessmentTypeModel>().HasData(
            new AssessmentTypeModel { Id = 1, Code = "QUIZ", Name = "Quiz", Description = "Short knowledge assessment", SortOrder = 1, AllowMultipleAttempts = true, SupportsAutoMarking = true, DefaultDurationMinutes = 30, DisplayNameEn = "Quiz", DisplayNameAf = "Toets", DisplayNameZu = "Ukuhlolwa" },
            new AssessmentTypeModel { Id = 2, Code = "ASSIGNMENT", Name = "Assignment", Description = "Written assignment or project work", SortOrder = 2, RequiresModeration = true, DefaultDurationMinutes = 0, DisplayNameEn = "Assignment", DisplayNameAf = "Opdrag", DisplayNameZu = "Umsebenzi" },
            new AssessmentTypeModel { Id = 3, Code = "EXAMINATION", Name = "Examination", Description = "Formal written examination", SortOrder = 3, RequiresModeration = true, DefaultDurationMinutes = 180, DisplayNameEn = "Examination", DisplayNameAf = "Eksamen", DisplayNameZu = "Isivivinyo" },
            new AssessmentTypeModel { Id = 4, Code = "PROJECT", Name = "Project", Description = "Comprehensive project work", SortOrder = 4, RequiresModeration = true, DefaultDurationMinutes = 0, DisplayNameEn = "Project", DisplayNameAf = "Projek", DisplayNameZu = "Iphrojekthi" },
            new AssessmentTypeModel { Id = 5, Code = "PORTFOLIO", Name = "Portfolio", Description = "Collection of evidence", SortOrder = 5, RequiresModeration = true, DefaultDurationMinutes = 0, DisplayNameEn = "Portfolio", DisplayNameAf = "Portefeulje", DisplayNameZu = "Iphothifoliyo" },
            new AssessmentTypeModel { Id = 6, Code = "PRACTICAL", Name = "Practical", Description = "Hands-on practical assessment", SortOrder = 6, RequiresModeration = true, DefaultDurationMinutes = 120, DisplayNameEn = "Practical", DisplayNameAf = "Prakties", DisplayNameZu = "Ukusebenza" },
            new AssessmentTypeModel { Id = 7, Code = "WORKPLACE_ASSESSMENT", Name = "Workplace Assessment", Description = "Assessment in workplace setting", SortOrder = 7, RequiresModeration = true, DefaultDurationMinutes = 0, DisplayNameEn = "Workplace Assessment", DisplayNameAf = "Werksplek Assessering", DisplayNameZu = "Ukuhlolwa Emsebenzini" },
            new AssessmentTypeModel { Id = 8, Code = "RPL_ASSESSMENT", Name = "RPL Assessment", Description = "Recognition of Prior Learning", SortOrder = 8, RequiresModeration = true, DefaultDurationMinutes = 0, DisplayNameEn = "RPL Assessment", DisplayNameAf = "ELV Assessering", DisplayNameZu = "Ukuhlolwa kwe-RPL" }
        );

        // Assessment Categories
        modelBuilder.Entity<AssessmentCategoryModel>().HasData(
            new AssessmentCategoryModel { Id = 1, Code = "FORMATIVE", Name = "Formative", Description = "Ongoing assessment for learning", SortOrder = 1, MaxWeightingPercentage = 50, IsSETACompliant = true, NQFLevelsApplicable = "[1,2,3,4,5,6,7,8,9,10]", DisplayNameEn = "Formative", DisplayNameAf = "Formatief", DisplayNameZu = "Okwakhayo" },
            new AssessmentCategoryModel { Id = 2, Code = "SUMMATIVE", Name = "Summative", Description = "Assessment of learning achievement", SortOrder = 2, MinWeightingPercentage = 50, RequiresExternalModeration = true, IsSETACompliant = true, NQFLevelsApplicable = "[1,2,3,4,5,6,7,8,9,10]", DisplayNameEn = "Summative", DisplayNameAf = "Summatief", DisplayNameZu = "Okuqoqayo" },
            new AssessmentCategoryModel { Id = 3, Code = "COMPETENCY", Name = "Competency", Description = "Competency-based assessment", SortOrder = 3, IsSETACompliant = true, NQFLevelsApplicable = "[1,2,3,4,5,6]", DisplayNameEn = "Competency", DisplayNameAf = "Bevoegdheid", DisplayNameZu = "Amakhono" },
            new AssessmentCategoryModel { Id = 4, Code = "AUTHENTIC", Name = "Authentic", Description = "Real-world application assessment", SortOrder = 4, IsSETACompliant = true, NQFLevelsApplicable = "[5,6,7,8,9,10]", DisplayNameEn = "Authentic", DisplayNameAf = "Outentiek", DisplayNameZu = "Okwangempela" },
            new AssessmentCategoryModel { Id = 5, Code = "DIAGNOSTIC", Name = "Diagnostic", Description = "Assessment to identify learning needs", SortOrder = 5, MaxWeightingPercentage = 10, IsSETACompliant = false, NQFLevelsApplicable = "[1,2,3,4,5,6,7]", DisplayNameEn = "Diagnostic", DisplayNameAf = "Diagnosties", DisplayNameZu = "Okuhlaziyayo" }
        );

        // Question Types
        modelBuilder.Entity<QuestionTypeModel>().HasData(
            new QuestionTypeModel { Id = 1, Code = "MULTIPLE_CHOICE", Name = "Multiple Choice", Description = "Select one or more correct answers", SortOrder = 1, SupportsAutoMarking = true, SupportsMultipleAnswers = true, InputType = "radio", DisplayNameEn = "Multiple Choice", DisplayNameAf = "Veelvuldige Keuse", DisplayNameZu = "Ukukhetha Okuningi" },
            new QuestionTypeModel { Id = 2, Code = "TRUE_FALSE", Name = "True/False", Description = "Select true or false", SortOrder = 2, SupportsAutoMarking = true, InputType = "radio", DisplayNameEn = "True/False", DisplayNameAf = "Waar/Onwaar", DisplayNameZu = "Iqiniso/Amanga" },
            new QuestionTypeModel { Id = 3, Code = "SHORT_ANSWER", Name = "Short Answer", Description = "Brief written response", SortOrder = 3, RequiresManualMarking = true, InputType = "text", MaxCharacterLimit = 500, DisplayNameEn = "Short Answer", DisplayNameAf = "Kort Antwoord", DisplayNameZu = "Impendulo Emfushane" },
            new QuestionTypeModel { Id = 4, Code = "ESSAY", Name = "Essay", Description = "Extended written response", SortOrder = 4, RequiresManualMarking = true, InputType = "textarea", MaxCharacterLimit = 5000, DisplayNameEn = "Essay", DisplayNameAf = "Opstel", DisplayNameZu = "Isinqumo" },
            new QuestionTypeModel { Id = 5, Code = "FILE_UPLOAD", Name = "File Upload", Description = "Upload files as response", SortOrder = 5, RequiresFileUpload = true, RequiresManualMarking = true, InputType = "file", DisplayNameEn = "File Upload", DisplayNameAf = "Lêer Oplaai", DisplayNameZu = "Ukulayisha Ifayela" },
            new QuestionTypeModel { Id = 6, Code = "NUMERICAL", Name = "Numerical", Description = "Numerical answer", SortOrder = 6, SupportsAutoMarking = true, InputType = "number", DisplayNameEn = "Numerical", DisplayNameAf = "Numeries", DisplayNameZu = "Izinombolo" },
            new QuestionTypeModel { Id = 7, Code = "PRACTICAL", Name = "Practical", Description = "Practical demonstration", SortOrder = 7, RequiresManualMarking = true, InputType = "textarea", DisplayNameEn = "Practical", DisplayNameAf = "Prakties", DisplayNameZu = "Ukusebenza" }
        );

        // Cognitive Levels (Bloom's Taxonomy)
        modelBuilder.Entity<CognitiveLevelModel>().HasData(
            new CognitiveLevelModel { Id = 1, Code = "REMEMBER", Name = "Remember", Description = "Recall facts and basic concepts", BloomLevel = 1, SortOrder = 1, LearningVerbs = "[\"define\",\"list\",\"state\",\"identify\",\"recall\"]", DisplayNameEn = "Remember", DisplayNameAf = "Onthou", DisplayNameZu = "Khumbula" },
            new CognitiveLevelModel { Id = 2, Code = "UNDERSTAND", Name = "Understand", Description = "Explain ideas or concepts", BloomLevel = 2, SortOrder = 2, LearningVerbs = "[\"explain\",\"describe\",\"summarize\",\"interpret\",\"classify\"]", DisplayNameEn = "Understand", DisplayNameAf = "Verstaan", DisplayNameZu = "Qonda" },
            new CognitiveLevelModel { Id = 3, Code = "APPLY", Name = "Apply", Description = "Use information in new situations", BloomLevel = 3, SortOrder = 3, LearningVerbs = "[\"apply\",\"demonstrate\",\"solve\",\"use\",\"implement\"]", DisplayNameEn = "Apply", DisplayNameAf = "Toepas", DisplayNameZu = "Sebenzisa" },
            new CognitiveLevelModel { Id = 4, Code = "ANALYZE", Name = "Analyze", Description = "Draw connections among ideas", BloomLevel = 4, SortOrder = 4, LearningVerbs = "[\"analyze\",\"compare\",\"contrast\",\"examine\",\"categorize\"]", DisplayNameEn = "Analyze", DisplayNameAf = "Analiseer", DisplayNameZu = "Hlaziya" },
            new CognitiveLevelModel { Id = 5, Code = "EVALUATE", Name = "Evaluate", Description = "Justify a stand or decision", BloomLevel = 5, SortOrder = 5, LearningVerbs = "[\"evaluate\",\"judge\",\"critique\",\"assess\",\"defend\"]", DisplayNameEn = "Evaluate", DisplayNameAf = "Evalueer", DisplayNameZu = "Hlolisisa" },
            new CognitiveLevelModel { Id = 6, Code = "CREATE", Name = "Create", Description = "Produce new or original work", BloomLevel = 6, SortOrder = 6, LearningVerbs = "[\"create\",\"design\",\"develop\",\"compose\",\"construct\"]", DisplayNameEn = "Create", DisplayNameAf = "Skep", DisplayNameZu = "Dala" }
        );

        // Submission Statuses
        modelBuilder.Entity<SubmissionStatusModel>().HasData(
            new SubmissionStatusModel { Id = 1, Code = "DRAFT", Name = "Draft", Description = "Work in progress", SortOrder = 1, IsInitialStatus = true, AllowsEditing = true, StatusColor = "info", IsVisibleToStudent = true, DisplayNameEn = "Draft", DisplayNameAf = "Konsep", DisplayNameZu = "Isicwangciso" },
            new SubmissionStatusModel { Id = 2, Code = "SUBMITTED", Name = "Submitted", Description = "Submitted for marking", SortOrder = 2, AllowsEditing = false, StatusColor = "success", IsVisibleToStudent = true, DisplayNameEn = "Submitted", DisplayNameAf = "Ingehandig", DisplayNameZu = "Kuthunyelwe" },
            new SubmissionStatusModel { Id = 3, Code = "MARKING", Name = "Marking", Description = "Currently being marked", SortOrder = 3, AllowsEditing = false, StatusColor = "warning", IsVisibleToStudent = true, DisplayNameEn = "Marking", DisplayNameAf = "Nasien", DisplayNameZu = "Kuphawulwa" },
            new SubmissionStatusModel { Id = 4, Code = "MARKED", Name = "Marked", Description = "Marking completed", SortOrder = 4, AllowsEditing = false, StatusColor = "info", IsVisibleToStudent = false, DisplayNameEn = "Marked", DisplayNameAf = "Nagesien", DisplayNameZu = "Kuphawuliwe" },
            new SubmissionStatusModel { Id = 5, Code = "MODERATION", Name = "Moderation", Description = "Under moderation", SortOrder = 5, AllowsEditing = false, StatusColor = "warning", IsVisibleToStudent = false, DisplayNameEn = "Moderation", DisplayNameAf = "Moderering", DisplayNameZu = "Kulinganiswa" },
            new SubmissionStatusModel { Id = 6, Code = "MODERATED", Name = "Moderated", Description = "Moderation completed", SortOrder = 6, AllowsEditing = false, StatusColor = "info", IsVisibleToStudent = false, DisplayNameEn = "Moderated", DisplayNameAf = "Gemodereer", DisplayNameZu = "Kulinganisiwe" },
            new SubmissionStatusModel { Id = 7, Code = "RELEASED", Name = "Released", Description = "Results released to student", SortOrder = 7, IsFinalStatus = true, AllowsEditing = false, StatusColor = "success", IsVisibleToStudent = true, DisplayNameEn = "Released", DisplayNameAf = "Vrygestel", DisplayNameZu = "Kukhishiwe" },
            new SubmissionStatusModel { Id = 8, Code = "APPEAL", Name = "Appeal", Description = "Under appeal review", SortOrder = 8, AllowsEditing = false, StatusColor = "danger", IsVisibleToStudent = true, DisplayNameEn = "Appeal", DisplayNameAf = "Appèl", DisplayNameZu = "Ukuphakamisa" },
            new SubmissionStatusModel { Id = 9, Code = "RESUBMIT", Name = "Resubmit", Description = "Requires resubmission", SortOrder = 9, AllowsEditing = true, StatusColor = "warning", IsVisibleToStudent = true, DisplayNameEn = "Resubmit", DisplayNameAf = "Heronderwerp", DisplayNameZu = "Phinda Uthumele" }
        );

        // Appeal Statuses
        modelBuilder.Entity<AppealStatusModel>().HasData(
            new AppealStatusModel { Id = 1, Code = "PENDING", Name = "Pending", Description = "Appeal submitted, awaiting review", SortOrder = 1, IsInitialStatus = true, RequiresAction = true, StatusColor = "warning", DisplayNameEn = "Pending", DisplayNameAf = "Hangende", DisplayNameZu = "Kulindile" },
            new AppealStatusModel { Id = 2, Code = "UNDER_REVIEW", Name = "Under Review", Description = "Appeal is being reviewed", SortOrder = 2, RequiresAction = true, StatusColor = "info", DisplayNameEn = "Under Review", DisplayNameAf = "Onder Hersiening", DisplayNameZu = "Kuyabuyekezwa" },
            new AppealStatusModel { Id = 3, Code = "APPROVED", Name = "Approved", Description = "Appeal approved - grade changed", SortOrder = 3, IsFinalStatus = true, RequiresAction = false, StatusColor = "success", DisplayNameEn = "Approved", DisplayNameAf = "Goedgekeur", DisplayNameZu = "Kwamukelwe" },
            new AppealStatusModel { Id = 4, Code = "REJECTED", Name = "Rejected", Description = "Appeal rejected - original grade stands", SortOrder = 4, IsFinalStatus = true, RequiresAction = false, StatusColor = "danger", DisplayNameEn = "Rejected", DisplayNameAf = "Verwerp", DisplayNameZu = "Kwenqatshwa" },
            new AppealStatusModel { Id = 5, Code = "PARTIALLY_APPROVED", Name = "Partially Approved", Description = "Appeal partially approved - grade adjusted", SortOrder = 5, IsFinalStatus = true, RequiresAction = false, StatusColor = "info", DisplayNameEn = "Partially Approved", DisplayNameAf = "Gedeeltelik Goedgekeur", DisplayNameZu = "Kwamukelwe Ngokwengxenye" }
        );

        // Moderation Types
        modelBuilder.Entity<ModerationTypeModel>().HasData(
            new ModerationTypeModel { Id = 1, Code = "INTERNAL", Name = "Internal", Description = "Internal institutional moderation", SortOrder = 1, RequiresExternalModerator = false, MinimumSamplePercentage = 25, MaxDaysToComplete = 5, DisplayNameEn = "Internal", DisplayNameAf = "Intern", DisplayNameZu = "Yangaphakathi" },
            new ModerationTypeModel { Id = 2, Code = "EXTERNAL", Name = "External", Description = "External expert moderation", SortOrder = 2, RequiresExternalModerator = true, MinimumSamplePercentage = 25, MaxDaysToComplete = 10, RequiresBudgetApproval = true, DisplayNameEn = "External", DisplayNameAf = "Ekstern", DisplayNameZu = "Yangaphandle" },
            new ModerationTypeModel { Id = 3, Code = "SETA", Name = "SETA", Description = "SETA compliance moderation", SortOrder = 3, RequiresExternalModerator = true, RequiresSETAApproval = true, MinimumSamplePercentage = 30, MaxDaysToComplete = 15, RequiresBudgetApproval = true, DisplayNameEn = "SETA", DisplayNameAf = "SETA", DisplayNameZu = "I-SETA" },
            new ModerationTypeModel { Id = 4, Code = "BLIND", Name = "Blind", Description = "Anonymous moderation", SortOrder = 4, IsAnonymous = true, MinimumSamplePercentage = 20, MaxDaysToComplete = 7, DisplayNameEn = "Blind", DisplayNameAf = "Blind", DisplayNameZu = "Okufihliwe" }
        );
    }
}
```

### Assessment Planning at Course Level

```csharp
public class CourseAssessmentPlanModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public CourseModel Course { get; set; }
    
    // SETA/NQF Compliance
    public string QualificationId { get; set; } // SAQA qualification ID
    public List<string> UnitStandardIds { get; set; } // Unit standards covered
    public int NQFLevel { get; set; }
    
    // Assessment Distribution
    public double FormativeWeighting { get; set; } = 40.0; // %
    public double SummativeWeighting { get; set; } = 60.0; // %
    
    // Pass Requirements
    public double MinimumPassPercentage { get; set; } = 50.0;
    public double SubminimumThreshold { get; set; } = 40.0; // Can't pass if below this
    public bool RequireAllCompetencies { get; set; } = false; // For competency-based
    
    // Moderation Requirements
    public bool RequiresModeration { get; set; }
    public double ModerationPercentage { get; set; } = 25.0; // % of submissions to moderate
    
    // Assessment Schedule
    public List<AssessmentScheduleModel> Schedule { get; set; }
}

public class AssessmentScheduleModel
{
    [Key]
    public int Id { get; set; }
    
    public int CourseAssessmentPlanId { get; set; }
    
    public string Title { get; set; }
    public int AssessmentTypeId { get; set; }
    [ForeignKey("AssessmentTypeId")]
    public AssessmentTypeModel AssessmentType { get; set; }
    public double WeightingPercentage { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string LearningOutcomes { get; set; }
    public bool IsCompulsory { get; set; } = true;
}
```

### Learning Outcome Mapping

```csharp
public class LearningOutcomeModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public CourseModel Course { get; set; }
    
    public string Code { get; set; } // LO1, LO2, etc.
    public string Description { get; set; }
    
    // NQF/SETA Alignment
    public string UnitStandardId { get; set; }
    public string AssessmentCriteria { get; set; }
    public int CognitiveLevelId { get; set; }
    [ForeignKey("CognitiveLevelId")]
    public CognitiveLevelModel CognitiveLevel { get; set; }
    
    // Assessment Coverage
    public List<AssessmentLearningOutcomeModel> AssessmentMappings { get; set; }
}

public class AssessmentLearningOutcomeModel
{
    [Key]
    public int Id { get; set; }
    
    public int AssessmentId { get; set; }
    [ForeignKey("AssessmentId")]
    public AssessmentModel Assessment { get; set; }
    
    public int LearningOutcomeId { get; set; }
    [ForeignKey("LearningOutcomeId")]
    public LearningOutcomeModel LearningOutcome { get; set; }
    
    public double WeightingPercentage { get; set; }
    public bool IsPrimaryOutcome { get; set; }
}
```

---

## End-to-End Workflow

### 1. Assessment Authoring

**Workflow Steps:**

1. **Planning Phase**
   - Course coordinator creates assessment plan
   - Maps assessments to learning outcomes
   - Sets weighting and pass requirements
   - Defines moderation strategy

2. **Creation Phase**
   - Subject matter expert creates assessment
   - Adds questions with cognitive level mapping
   - Creates marking rubrics
   - Sets technical configuration (timing, attempts)

3. **Review Phase**
   - Internal peer review of content
   - Technical review of settings
   - Accessibility and fairness review
   - SETA compliance check (if applicable)

4. **Approval Phase**
   - Head of Department approval
   - Quality assurance sign-off
   - External moderator review (if required)

5. **Publication**
   - Assessment made available to students
   - Automated notifications sent
   - Availability recorded in audit trail

### 2. Student Submission

**Workflow Steps:**

1. **Pre-Assessment**
   - Student views assessment details
   - Checks prerequisites and attempts remaining
   - Reviews instructions and requirements
   - Confirms understanding of academic integrity

2. **During Assessment**
   - System tracks time and progress
   - Auto-saves responses periodically
   - Prevents navigation away (if configured)
   - Monitors for technical issues

3. **Submission**
   - Final review opportunity
   - Plagiarism checking (for written work)
   - File upload validation
   - Generation of submission receipt

4. **Post-Submission**
   - Immediate acknowledgment
   - Progress tracking update
   - Notification to markers
   - Audit trail creation

### 3. Marking Process

**Marking Workflow:**

1. **Assignment Phase**
   - System assigns submissions to markers
   - Workload balancing algorithms
   - Conflict of interest checking
   - Anonymization (if configured)

2. **Auto-Marking**
   - Objective questions marked automatically
   - Immediate score calculation
   - Pattern recognition for common answers
   - Flagging for manual review

3. **Manual Marking**
   - Marker reviews subjective responses
   - Applies rubric criteria consistently
   - Provides detailed feedback
   - Justifies grade decisions

4. **Quality Assurance**
   - Cross-referencing with rubric
   - Consistency checking across markers
   - Bias detection algorithms
   - Feedback quality assessment

### 4. Moderation Process

**Moderation Types:**

1. **Internal Moderation**
   - Same institution moderator
   - 25% sample typically
   - Focus on consistency and standards

2. **External Moderation**
   - Independent external expert
   - Required for certain qualifications
   - Validates institutional standards

3. **SETA Moderation**
   - Industry expert moderator
   - Workplace relevance verification
   - Competency standard alignment

**Moderation Criteria:**
- Grade distribution analysis
- Rubric application consistency
- Feedback quality and constructiveness
- Assessment difficulty appropriateness
- Learning outcome achievement

### 5. Grade Release

**Release Process:**

1. **Pre-Release Validation**
   - Completeness checking
   - Grade calculation verification
   - Moderation compliance
   - Appeal window notification

2. **Approval Workflow**
   - Academic coordinator review
   - Head of department sign-off
   - Registrar notification
   - Student services alert

3. **Student Notification**
   - Email/SMS notification
   - LMS portal update
   - Detailed feedback access
   - Appeal process information

### 6. Appeals Process

**Appeal Grounds:**
- Procedural irregularities
- Marking errors or inconsistencies
- Extenuating circumstances
- Technical failures
- Discrimination or bias claims

**Appeal Timeline:**
- **Filing**: Within 10 working days of grade release
- **Acknowledgment**: Within 2 working days
- **Review**: Within 15 working days
- **Decision**: Within 5 working days of review completion

---

## Grading Scheme

### South African Grade Bands

#### Higher Education (Universities)

| Grade | Symbol | Percentage | Description |
|-------|--------|------------|-------------|
| First Class | A+ | 90-100% | Outstanding achievement |
| First Class | A | 80-89% | Excellent achievement |
| Upper Second | B+ | 75-79% | Very good achievement |
| Upper Second | B | 70-74% | Good achievement |
| Lower Second | C+ | 65-69% | Satisfactory achievement |
| Lower Second | C | 60-64% | Adequate achievement |
| Third Class | D+ | 55-59% | Marginal achievement |
| Third Class | D | 50-54% | Poor achievement |
| Fail | F | 0-49% | Unacceptable achievement |

#### TVET Colleges

| Grade | Symbol | Percentage | Description |
|-------|--------|------------|-------------|
| Outstanding | 7 | 90-100% | Outstanding achievement |
| Meritorious | 6 | 80-89% | Meritorious achievement |
| Substantial | 5 | 70-79% | Substantial achievement |
| Adequate | 4 | 60-69% | Adequate achievement |
| Moderate | 3 | 50-59% | Moderate achievement |
| Elementary | 2 | 40-49% | Elementary achievement |
| Not Achieved | 1 | 30-39% | Not achieved |
| Not Achieved | 0 | 0-29% | Not achieved |

#### Competency-Based (SETA)

| Rating | Code | Description | Criteria |
|--------|------|-------------|----------|
| Competent | C | Meets all requirements | All assessment criteria achieved |
| Not Yet Competent | NYC | Does not meet requirements | One or more criteria not achieved |
| Sufficient Evidence | SE | Adequate evidence provided | RPL assessment only |
| Insufficient Evidence | IE | More evidence required | RPL assessment only |

### Weighting Strategies

#### Traditional Academic Programs

```json
{
  "assessmentWeighting": {
    "formativeAssessments": {
      "percentage": 40,
      "components": {
        "quizzes": 15,
        "assignments": 15,
        "participation": 10
      }
    },
    "summativeAssessments": {
      "percentage": 60,
      "components": {
        "midtermExam": 25,
        "finalExam": 35
      }
    }
  },
  "passRequirements": {
    "overallMinimum": 50,
    "subminimumThreshold": 40,
    "examMinimum": 40,
    "attendanceMinimum": 80
  }
}
```

#### Competency-Based Programs

```json
{
  "assessmentStructure": {
    "knowledgeAssessment": {
      "weight": 30,
      "passThreshold": "competent",
      "attempts": 3
    },
    "practicalAssessment": {
      "weight": 50,
      "passThreshold": "competent",
      "attempts": 2
    },
    "workplaceAssessment": {
      "weight": 20,
      "passThreshold": "competent",
      "attempts": 1
    }
  },
  "overallRequirement": "all_competent"
}
```

### Pass/Fail Rules

#### Standard Rules

```csharp
public class GradingRules
{
    public static class University
    {
        public const double PassThreshold = 50.0;
        public const double SubminimumThreshold = 40.0;
        public const double ExamMinimum = 40.0;
        public const double AttendanceMinimum = 80.0;
        
        public static bool IsPass(double finalScore, double examScore, double attendance)
        {
            return finalScore >= PassThreshold && 
                   examScore >= ExamMinimum && 
                   attendance >= AttendanceMinimum &&
                   finalScore >= SubminimumThreshold;
        }
    }
    
    public static class TVET
    {
        public const double PassThreshold = 50.0;
        public const double SubminimumThreshold = 40.0;
        
        public static int GetLevel(double percentage)
        {
            if (percentage >= 90) return 7;
            if (percentage >= 80) return 6;
            if (percentage >= 70) return 5;
            if (percentage >= 60) return 4;
            if (percentage >= 50) return 3;
            if (percentage >= 40) return 2;
            if (percentage >= 30) return 1;
            return 0;
        }
    }
    
    public static class Competency
    {
        public static bool IsCompetent(List<AssessmentCriteriaResult> criteria)
        {
            return criteria.All(c => c.IsAchieved);
        }
    }
}
```

### Reassessment Policies

#### University Level

```csharp
public class ReassessmentPolicy
{
    public struct UniversityRules
    {
        public const int MaxAttempts = 2;
        public const double MinimumForReassessment = 40.0;
        public const double ReassessmentCap = 50.0; // Maximum grade after reassessment
        public const int DaysBetweenAttempts = 30;
        
        public static bool IsEligibleForReassessment(double score, int attempts)
        {
            return score >= MinimumForReassessment && 
                   score < 50.0 && 
                   attempts < MaxAttempts;
        }
    }
    
    public struct TVETRules
    {
        public const int MaxAttempts = 3;
        public const double MinimumForReassessment = 30.0;
        public const int DaysBetweenAttempts = 21;
        
        public static bool IsEligibleForReassessment(double score, int attempts)
        {
            return score >= MinimumForReassessment && 
                   score < 50.0 && 
                   attempts < MaxAttempts;
        }
    }
    
    public struct CompetencyRules
    {
        public const int MaxAttempts = 3;
        public const int DaysBetweenAttempts = 14;
        
        public static bool IsEligibleForReassessment(bool isCompetent, int attempts)
        {
            return !isCompetent && attempts < MaxAttempts;
        }
    }
}
```

### Storage and Audit Considerations

#### Data Retention Policies

```json
{
  "retentionPolicies": {
    "studentSubmissions": {
      "duration": "7 years",
      "reason": "SETA compliance requirement",
      "storageType": "encrypted_archive"
    },
    "gradingRecords": {
      "duration": "10 years", 
      "reason": "Academic transcript support",
      "storageType": "immutable_ledger"
    },
    "moderationReports": {
      "duration": "5 years",
      "reason": "Quality assurance audits",
      "storageType": "secure_database"
    },
    "appealRecords": {
      "duration": "7 years",
      "reason": "Legal compliance",
      "storageType": "legal_archive"
    }
  }
}
```

#### Audit Trail Requirements

```csharp
public class AuditTrailModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string EntityType { get; set; } // Assessment, Submission, Grade
    
    [Required]
    public int EntityId { get; set; }
    
    [Required]
    public string Action { get; set; } // Created, Modified, Deleted, Viewed
    
    [Required]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public Users User { get; set; }
    
    public string OldValues { get; set; } // JSON
    public string NewValues { get; set; } // JSON
    
    public string IPAddress { get; set; }
    public string UserAgent { get; set; }
    public string SessionId { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; }
    
    // Compliance Fields
    public string ComplianceReason { get; set; }
    public bool IsSystemGenerated { get; set; }
    public string DigitalSignature { get; set; }
}
```

---

## Compliance and Reporting

### SETA Reporting Requirements

#### Learner Achievement Data

```json
{
  "learnerAchievementReport": {
    "reportingPeriod": "2025-Q1",
    "setaBody": "ETDP-SETA",
    "institution": {
      "name": "Example University",
      "registrationNumber": "REG001",
      "accreditationNumber": "ACC001"
    },
    "learnerData": [
      {
        "nationalId": "9001011234567",
        "qualificationId": "QUAL001", 
        "unitStandards": [
          {
            "unitStandardId": "US001",
            "assessmentDate": "2025-03-15",
            "result": "Competent",
            "moderatorId": "MOD001",
            "evidence": "Portfolio submitted and verified"
          }
        ],
        "overallStatus": "In Progress",
        "enrollmentDate": "2025-01-15",
        "expectedCompletion": "2025-12-15"
      }
    ]
  }
}
```

#### Assessment Quality Reports

```json
{
  "assessmentQualityReport": {
    "reportingPeriod": "2025-Q1",
    "course": "Computer Programming",
    "assessmentStatistics": {
      "totalAssessments": 15,
      "moderatedAssessments": 4,
      "moderationRate": 26.7,
      "averageScore": 72.3,
      "passRate": 85.2,
      "gradeDistribution": {
        "A": 15,
        "B": 35,
        "C": 30,
        "D": 15,
        "F": 5
      }
    },
    "moderationFindings": {
      "standardsConsistency": "Good",
      "rubricApplication": "Excellent", 
      "feedbackQuality": "Good",
      "recommendedActions": [
        "Increase moderation sample to 30%",
        "Provide additional marker training"
      ]
    }
  }
}
```

### Progress Tracking Integration

#### Student Progress Dashboard

```csharp
public class StudentProgressModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    [ForeignKey("StudentId")]
    public Users Student { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public CourseModel Course { get; set; }
    
    // Overall Progress
    public double OverallPercentage { get; set; }
    public string CurrentGrade { get; set; }
    public bool IsOnTrack { get; set; }
    
    // Assessment Breakdown
    public double FormativeScore { get; set; }
    public double SummativeScore { get; set; }
    public int AssessmentsCompleted { get; set; }
    public int TotalAssessments { get; set; }
    
    // Learning Outcomes
    public string LearningOutcomeProgress { get; set; } // JSON
    
    // Predictions
    public double PredictedFinalGrade { get; set; }
    public string RiskLevel { get; set; } // Low, Medium, High
    public List<string> Recommendations { get; set; }
    
    // Timestamps
    public DateTime LastUpdated { get; set; }
    public DateTime LastActivity { get; set; }
}
```

---

## API Endpoints

### Assessment Management

```csharp
// Assessment CRUD
[Route("api/[controller]")]
public class AssessmentController : ControllerBase
{
    // GET /api/assessment/course/{courseId}
    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetCourseAssessments(int courseId)
    
    // POST /api/assessment
    [HttpPost]
    public async Task<IActionResult> CreateAssessment([FromBody] CreateAssessmentDto dto)
    
    // PUT /api/assessment/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssessment(int id, [FromBody] UpdateAssessmentDto dto)
    
    // POST /api/assessment/{id}/publish
    [HttpPost("{id}/publish")]
    public async Task<IActionResult> PublishAssessment(int id)
    
    // GET /api/assessment/{id}/statistics
    [HttpGet("{id}/statistics")]
    public async Task<IActionResult> GetAssessmentStatistics(int id)
}
```

### Submission Management

```csharp
[Route("api/[controller]")]
public class SubmissionController : ControllerBase
{
    // POST /api/submission/start
    [HttpPost("start")]
    public async Task<IActionResult> StartAssessment([FromBody] StartAssessmentDto dto)
    
    // POST /api/submission/save-progress
    [HttpPost("save-progress")]
    public async Task<IActionResult> SaveProgress([FromBody] SaveProgressDto dto)
    
    // POST /api/submission/submit
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitAssessment([FromBody] SubmitAssessmentDto dto)
    
    // GET /api/submission/student/{studentId}/course/{courseId}
    [HttpGet("student/{studentId}/course/{courseId}")]
    public async Task<IActionResult> GetStudentSubmissions(int studentId, int courseId)
    
    // POST /api/submission/{id}/appeal
    [HttpPost("{id}/appeal")]
    public async Task<IActionResult> FileAppeal(int id, [FromBody] AppealDto dto)
}
```

### Grading and Moderation

```csharp
[Route("api/[controller]")]
public class GradingController : ControllerBase
{
    // GET /api/grading/pending
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingSubmissions()
    
    // POST /api/grading/{submissionId}/mark
    [HttpPost("{submissionId}/mark")]
    public async Task<IActionResult> MarkSubmission(int submissionId, [FromBody] MarkingDto dto)
    
    // POST /api/grading/{submissionId}/moderate
    [HttpPost("{submissionId}/moderate")]
    public async Task<IActionResult> ModerateSubmission(int submissionId, [FromBody] ModerationDto dto)
    
    // POST /api/grading/release-grades
    [HttpPost("release-grades")]
    public async Task<IActionResult> ReleaseGrades([FromBody] ReleaseGradesDto dto)
    
    // GET /api/grading/moderation-required
    [HttpGet("moderation-required")]
    public async Task<IActionResult> GetModerationQueue()
}
```

### Reporting and Analytics

```csharp
[Route("api/[controller]")]
public class ReportingController : ControllerBase
{
    // GET /api/reporting/seta-learner-data
    [HttpGet("seta-learner-data")]
    public async Task<IActionResult> GetSETALearnerData([FromQuery] ReportingPeriodDto period)
    
    // GET /api/reporting/assessment-quality
    [HttpGet("assessment-quality")]
    public async Task<IActionResult> GetAssessmentQualityReport([FromQuery] string courseId)
    
    // GET /api/reporting/student-progress/{studentId}
    [HttpGet("student-progress/{studentId}")]
    public async Task<IActionResult> GetStudentProgress(int studentId)
    
    // GET /api/reporting/institutional-analytics
    [HttpGet("institutional-analytics")]
    public async Task<IActionResult> GetInstitutionalAnalytics([FromQuery] string period)
    
    // POST /api/reporting/generate-transcript
    [HttpPost("generate-transcript")]
    public async Task<IActionResult> GenerateTranscript([FromBody] TranscriptRequestDto dto)
}
```

---

## Implementation Guide

### Phase 1: Core Assessment Framework (4-6 weeks)

1. **Database Schema Implementation**
   - Create all assessment-related tables
   - Set up foreign key relationships
   - Implement indexes for performance
   - Create audit trail tables

2. **Basic Models and Services**
   - Implement core assessment models
   - Create service layer interfaces
   - Build basic CRUD operations
   - Add validation logic

3. **Assessment Authoring**
   - Build assessment creation API
   - Implement question management
   - Add rubric functionality
   - Create assessment configuration

### Phase 2: Submission and Grading (4-6 weeks)

1. **Submission System**
   - Student assessment interface
   - Auto-save functionality
   - File upload handling
   - Plagiarism integration

2. **Grading Workflow**
   - Marker assignment logic
   - Auto-marking for objective questions
   - Manual marking interface
   - Feedback management

3. **Grade Calculation**
   - Implement grading rules
   - Add weighting calculations
   - Build pass/fail logic
   - Create grade recording

### Phase 3: Moderation and Quality Assurance (3-4 weeks)

1. **Moderation Framework**
   - Sample selection algorithms
   - Moderator assignment
   - Score comparison logic
   - Moderation reporting

2. **Appeals Process**
   - Appeal filing system
   - Review workflow
   - Decision tracking
   - Grade adjustment

### Phase 4: Compliance and Reporting (3-4 weeks)

1. **SETA Integration**
   - Competency mapping
   - Unit standard tracking
   - SETA reporting formats
   - Data export functionality

2. **Analytics and Progress**
   - Student progress tracking
   - Institutional analytics
   - Risk identification
   - Predictive modeling

### Phase 5: Advanced Features (2-4 weeks)

1. **Security and Audit**
   - Enhanced audit trails
   - Digital signatures
   - Blockchain integration prep
   - Compliance verification

2. **Performance Optimization**
   - Database query optimization
   - Caching implementation
   - Background processing
   - Load testing

### Technical Considerations

#### Database Performance

```sql
-- Key indexes for assessment queries
CREATE INDEX IX_Assessment_CourseId_Type ON Assessments (CourseId, Type);
CREATE INDEX IX_Submission_StudentId_Status ON Submissions (StudentId, Status);
CREATE INDEX IX_Grade_ReleaseDate ON Submissions (FeedbackDate) WHERE FeedbackDate IS NOT NULL;

-- Partitioning for large submission tables
ALTER TABLE Submissions 
PARTITION BY RANGE (YEAR(SubmittedAt))
(
    PARTITION p2024 VALUES LESS THAN (2025),
    PARTITION p2025 VALUES LESS THAN (2026),
    PARTITION p2026 VALUES LESS THAN (2027)
);
```

#### Caching Strategy

```csharp
public class AssessmentCacheService
{
    private readonly IMemoryCache _cache;
    private readonly IDistributedCache _distributedCache;
    
    // Cache assessment metadata for 1 hour
    public async Task<AssessmentModel> GetAssessmentAsync(int id)
    {
        string cacheKey = $"assessment:{id}";
        
        if (!_cache.TryGetValue(cacheKey, out AssessmentModel assessment))
        {
            assessment = await _repository.GetAssessmentAsync(id);
            _cache.Set(cacheKey, assessment, TimeSpan.FromHours(1));
        }
        
        return assessment;
    }
    
    // Cache student submissions for 5 minutes (frequently changing)
    public async Task<List<SubmissionModel>> GetStudentSubmissionsAsync(int studentId)
    {
        string cacheKey = $"submissions:student:{studentId}";
        
        var cachedSubmissions = await _distributedCache.GetStringAsync(cacheKey);
        if (cachedSubmissions != null)
        {
            return JsonSerializer.Deserialize<List<SubmissionModel>>(cachedSubmissions);
        }
        
        var submissions = await _repository.GetStudentSubmissionsAsync(studentId);
        var serialized = JsonSerializer.Serialize(submissions);
        
        await _distributedCache.SetStringAsync(cacheKey, serialized, 
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        
        return submissions;
    }
}
```

#### Background Processing

```csharp
public class AssessmentBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessPendingModerations();
            await SendAssessmentReminders();
            await GenerateProgressReports();
            await UpdateRiskAnalytics();
            
            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
        }
    }
    
    private async Task ProcessPendingModerations()
    {
        var pendingModerations = await _repository.GetPendingModerationsAsync();
        
        foreach (var moderation in pendingModerations)
        {
            // Process moderation logic
            await _moderationService.ProcessModerationAsync(moderation.Id);
        }
    }
}
```

This comprehensive assessment and grading system provides a robust foundation for South African educational institutions, ensuring compliance with SAQA, SETA, and NQF requirements while delivering a modern, efficient learning management experience.