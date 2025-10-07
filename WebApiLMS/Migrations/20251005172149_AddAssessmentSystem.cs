using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessmentSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    MinWeightingPercentage = table.Column<double>(type: "float", nullable: false),
                    MaxWeightingPercentage = table.Column<double>(type: "float", nullable: false),
                    RequiresExternalModeration = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    AllowMultipleAttempts = table.Column<bool>(type: "bit", nullable: false),
                    RequiresModeration = table.Column<bool>(type: "bit", nullable: false),
                    SupportsAutoMarking = table.Column<bool>(type: "bit", nullable: false),
                    DefaultDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradingScales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradingScales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowsMultipleAnswers = table.Column<bool>(type: "bit", nullable: false),
                    RequiresTextInput = table.Column<bool>(type: "bit", nullable: false),
                    AllowsFileUpload = table.Column<bool>(type: "bit", nullable: false),
                    SupportsAutoGrading = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssessmentTypeId = table.Column<int>(type: "int", nullable: false),
                    AssessmentCategoryId = table.Column<int>(type: "int", nullable: false),
                    MaxMarks = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    AttemptsAllowed = table.Column<int>(type: "int", nullable: false),
                    WeightingPercentage = table.Column<double>(type: "float", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequiresModeration = table.Column<bool>(type: "bit", nullable: false),
                    RequiresExternalModeration = table.Column<bool>(type: "bit", nullable: false),
                    ModerationPercentage = table.Column<int>(type: "int", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GradingScaleModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_AssessmentCategories_AssessmentCategoryId",
                        column: x => x.AssessmentCategoryId,
                        principalTable: "AssessmentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_AssessmentTypes_AssessmentTypeId",
                        column: x => x.AssessmentTypeId,
                        principalTable: "AssessmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_GradingScales_GradingScaleModelId",
                        column: x => x.GradingScaleModelId,
                        principalTable: "GradingScales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GradeBoundaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradingScaleId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GradePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPassingGrade = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GradingScaleModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeBoundaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeBoundaries_GradingScales_GradingScaleId",
                        column: x => x.GradingScaleId,
                        principalTable: "GradingScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeBoundaries_GradingScales_GradingScaleModelId",
                        column: x => x.GradingScaleModelId,
                        principalTable: "GradingScales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    QuestionTypeId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Marks = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    LearningOutcomeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuestionTypeModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QuestionTypeModelId",
                        column: x => x.QuestionTypeModelId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rubrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssessmentId = table.Column<int>(type: "int", nullable: true),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rubrics_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GradedBy = table.Column<int>(type: "int", nullable: true),
                    GraderComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    TimeTaken = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsLateSubmission = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RubricCriteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RubricId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    LearningOutcomeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RubricModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubricCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RubricCriteria_Rubrics_RubricId",
                        column: x => x.RubricId,
                        principalTable: "Rubrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RubricCriteria_Rubrics_RubricModelId",
                        column: x => x.RubricModelId,
                        principalTable: "Rubrics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedOptionIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarksAwarded = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GradedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmissionModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Submissions_SubmissionModelId",
                        column: x => x.SubmissionModelId,
                        principalTable: "Submissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Gradings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    SubmissionId = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LetterGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GradingScaleId = table.Column<int>(type: "int", nullable: true),
                    GradingMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradedBy = table.Column<int>(type: "int", nullable: true),
                    GradedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    PreviousGradeId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RubricScores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExempt = table.Column<bool>(type: "bit", nullable: false),
                    ExemptionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLate = table.Column<bool>(type: "bit", nullable: false),
                    LatePenalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gradings_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gradings_GradingScales_GradingScaleId",
                        column: x => x.GradingScaleId,
                        principalTable: "GradingScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gradings_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gradings_Users_GradedBy",
                        column: x => x.GradedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gradings_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RubricLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CriterionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RubricCriterionModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubricLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RubricLevels_RubricCriteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "RubricCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RubricLevels_RubricCriteria_RubricCriterionModelId",
                        column: x => x.RubricCriterionModelId,
                        principalTable: "RubricCriteria",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SubmissionId",
                table: "Answers",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SubmissionModelId",
                table: "Answers",
                column: "SubmissionModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_AssessmentCategoryId",
                table: "Assessments",
                column: "AssessmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_AssessmentTypeId",
                table: "Assessments",
                column: "AssessmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CourseId",
                table: "Assessments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_GradingScaleModelId",
                table: "Assessments",
                column: "GradingScaleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeBoundaries_GradingScaleId",
                table: "GradeBoundaries",
                column: "GradingScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeBoundaries_GradingScaleModelId",
                table: "GradeBoundaries",
                column: "GradingScaleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradings_AssessmentId",
                table: "Gradings",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradings_GradedBy",
                table: "Gradings",
                column: "GradedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Gradings_GradingScaleId",
                table: "Gradings",
                column: "GradingScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradings_StudentId",
                table: "Gradings",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradings_SubmissionId",
                table: "Gradings",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AssessmentId",
                table: "Questions",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeModelId",
                table: "Questions",
                column: "QuestionTypeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriteria_RubricId",
                table: "RubricCriteria",
                column: "RubricId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriteria_RubricModelId",
                table: "RubricCriteria",
                column: "RubricModelId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricLevels_CriterionId",
                table: "RubricLevels",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricLevels_RubricCriterionModelId",
                table: "RubricLevels",
                column: "RubricCriterionModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_AssessmentId",
                table: "Rubrics",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AssessmentId",
                table: "Submissions",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentId",
                table: "Submissions",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "GradeBoundaries");

            migrationBuilder.DropTable(
                name: "Gradings");

            migrationBuilder.DropTable(
                name: "RubricLevels");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "RubricCriteria");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "Rubrics");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "AssessmentCategories");

            migrationBuilder.DropTable(
                name: "AssessmentTypes");

            migrationBuilder.DropTable(
                name: "GradingScales");
        }
    }
}
