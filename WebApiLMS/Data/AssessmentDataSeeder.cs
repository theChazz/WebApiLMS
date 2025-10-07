using Microsoft.EntityFrameworkCore;
using WebApiLMS.Models;

namespace WebApiLMS.Data
{
    public static class AssessmentDataSeeder
    {
        public static async Task SeedAssessmentDummyDataAsync(WebApiLMSDbContext context)
        {
            // Check if assessment data already exists
            if (await context.Assessments.AnyAsync())
            {
                Console.WriteLine("Assessment dummy data already exists. Skipping seeding.");
                return;
            }

            Console.WriteLine("Starting Assessment System dummy data seeding...");

            // Get required lookup data
            var courses = await context.Courses.ToListAsync();
            var students = await context.Users
                .Include(u => u.UserRole)
                .Where(u => u.UserRole.Code == "Student")
                .ToListAsync();
            var lecturers = await context.Users
                .Include(u => u.UserRole)
                .Where(u => u.UserRole.Code == "Lecturer")
                .ToListAsync();
            var assessors = await context.Users
                .Include(u => u.UserRole)
                .Where(u => u.UserRole.Code == "Assessor")
                .ToListAsync();

            var assessmentTypes = await context.AssessmentTypes.ToListAsync();
            var assessmentCategories = await context.AssessmentCategories.ToListAsync();
            var questionTypes = await context.QuestionTypes.ToListAsync();
            var gradingScales = await context.GradingScales.ToListAsync();

            if (!courses.Any() || !students.Any() || !lecturers.Any())
            {
                Console.WriteLine("Required base data (courses, students, lecturers) not found. Please seed base data first.");
                return;
            }

            // Seed Assessments
            var assessments = new List<AssessmentModel>();
            var assessmentId = 1;

            foreach (var course in courses.Take(5)) // Create assessments for first 5 courses
            {
                var formativeType = assessmentTypes.FirstOrDefault(at => at.Code == "QUIZ");
                var summativeType = assessmentTypes.FirstOrDefault(at => at.Code == "EXAM");
                var assignmentType = assessmentTypes.FirstOrDefault(at => at.Code == "ASSIGNMENT");

                var formativeCategory = assessmentCategories.FirstOrDefault(ac => ac.Code == "FORMATIVE");
                var summativeCategory = assessmentCategories.FirstOrDefault(ac => ac.Code == "SUMMATIVE");

                var lecturer = lecturers[assessmentId % lecturers.Count];

                // Create multiple assessments per course
                assessments.AddRange(new[]
                {
                    new AssessmentModel
                    {
                        CourseId = course.Id,
                        Title = $"{course.CourseName} - Weekly Quiz {assessmentId}",
                        Description = $"Weekly knowledge check for {course.CourseName}",
                        AssessmentTypeId = formativeType?.Id ?? 1,
                        AssessmentCategoryId = formativeCategory?.Id ?? 1,
                        MaxMarks = 20,
                        Duration = 30, // 30 minutes
                        AttemptsAllowed = 2,
                        WeightingPercentage = 10.0,
                        OpenDate = DateTime.Now.AddDays(-7),
                        DueDate = DateTime.Now.AddDays(7),
                        CloseDate = DateTime.Now.AddDays(10),
                        RequiresModeration = false,
                        RequiresExternalModeration = false,
                        IsPublished = true,
                        IsActive = true,
                        CreatedBy = lecturer.Id,
                        CreatedAt = DateTime.Now.AddDays(-10)
                    },
                    new AssessmentModel
                    {
                        CourseId = course.Id,
                        Title = $"{course.CourseName} - Assignment {assessmentId}",
                        Description = $"Practical assignment for {course.CourseName}",
                        AssessmentTypeId = assignmentType?.Id ?? 2,
                        AssessmentCategoryId = formativeCategory?.Id ?? 1,
                        MaxMarks = 100,
                        Duration = 10080, // 1 week in minutes
                        AttemptsAllowed = 1,
                        WeightingPercentage = 30.0,
                        OpenDate = DateTime.Now.AddDays(-14),
                        DueDate = DateTime.Now.AddDays(14),
                        CloseDate = DateTime.Now.AddDays(16),
                        RequiresModeration = true,
                        RequiresExternalModeration = false,
                        ModerationPercentage = 20,
                        IsPublished = true,
                        IsActive = true,
                        CreatedBy = lecturer.Id,
                        CreatedAt = DateTime.Now.AddDays(-20)
                    },
                    new AssessmentModel
                    {
                        CourseId = course.Id,
                        Title = $"{course.CourseName} - Final Exam",
                        Description = $"Final examination for {course.CourseName}",
                        AssessmentTypeId = summativeType?.Id ?? 3,
                        AssessmentCategoryId = summativeCategory?.Id ?? 2,
                        MaxMarks = 150,
                        Duration = 180, // 3 hours
                        AttemptsAllowed = 1,
                        WeightingPercentage = 60.0,
                        OpenDate = DateTime.Now.AddDays(30),
                        DueDate = DateTime.Now.AddDays(30),
                        CloseDate = DateTime.Now.AddDays(31),
                        RequiresModeration = true,
                        RequiresExternalModeration = true,
                        ModerationPercentage = 100,
                        IsPublished = false, // Not yet published
                        IsActive = true,
                        CreatedBy = lecturer.Id,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    }
                });

                assessmentId++;
            }

            await context.Assessments.AddRangeAsync(assessments);
            await context.SaveChangesAsync();

            // Seed Questions for each assessment
            var questions = new List<QuestionModel>();
            var questionId = 1;

            var multipleChoiceType = questionTypes.FirstOrDefault(qt => qt.Name == "Multiple Choice");
            var shortAnswerType = questionTypes.FirstOrDefault(qt => qt.Name == "Short Answer");
            var essayType = questionTypes.FirstOrDefault(qt => qt.Name == "Essay");

            foreach (var assessment in assessments)
            {
                var questionCount = assessment.Title.Contains("Quiz") ? 5 : 
                                 assessment.Title.Contains("Assignment") ? 3 : 10; // Exam

                for (int i = 1; i <= questionCount; i++)
                {
                    var questionType = assessment.Title.Contains("Quiz") ? multipleChoiceType :
                                     assessment.Title.Contains("Assignment") ? essayType :
                                     (i % 2 == 0 ? multipleChoiceType : shortAnswerType);

                    questions.Add(new QuestionModel
                    {
                        AssessmentId = assessment.Id,
                        QuestionTypeId = questionType?.Id ?? 1,
                        QuestionText = GenerateQuestionText(assessment.Title, i, questionType?.Name ?? "Multiple Choice"),
                        Marks = assessment.Title.Contains("Quiz") ? 4 : 
                               assessment.Title.Contains("Assignment") ? 33 : 15,
                        SortOrder = i,
                        Instructions = $"Answer question {i} completely and clearly.",
                        CreatedAt = assessment.CreatedAt
                    });
                }

                questionId++;
            }

            await context.Questions.AddRangeAsync(questions);
            await context.SaveChangesAsync();

            // Seed Answer Options for Multiple Choice questions
            var answerOptions = new List<AnswerOptionModel>();
            var multipleChoiceQuestions = questions.Where(q => 
                questionTypes.Any(qt => qt.Id == q.QuestionTypeId && qt.Name == "Multiple Choice")).ToList();

            foreach (var question in multipleChoiceQuestions)
            {
                var options = GenerateAnswerOptions(question.QuestionText);
                for (int i = 0; i < options.Count; i++)
                {
                    answerOptions.Add(new AnswerOptionModel
                    {
                        QuestionId = question.Id,
                        OptionText = options[i].Text,
                        IsCorrect = options[i].IsCorrect,
                        Points = options[i].IsCorrect ? question.Marks : 0,
                        SortOrder = i + 1,
                        CreatedAt = question.CreatedAt
                    });
                }
            }

            await context.AnswerOptions.AddRangeAsync(answerOptions);
            await context.SaveChangesAsync();

            // Seed Student Submissions for published assessments
            var publishedAssessments = assessments.Where(a => a.IsPublished).ToList();
            var submissions = new List<SubmissionModel>();
            var submissionId = 1;

            foreach (var assessment in publishedAssessments)
            {
                // Get students enrolled in this course
                var enrolledStudents = await context.CourseStudentEnrollments
                    .Where(cse => cse.CourseId == assessment.CourseId)
                    .Select(cse => cse.StudentId)
                    .ToListAsync();

                var courseStudents = students.Where(s => enrolledStudents.Contains(s.Id)).Take(5).ToList();

                foreach (var student in courseStudents)
                {
                    var attemptCount = assessment.Title.Contains("Quiz") ? Random.Shared.Next(1, 3) : 1;
                    
                    for (int attempt = 1; attempt <= attemptCount; attempt++)
                    {
                        var submissionDate = assessment.OpenDate?.AddHours(Random.Shared.Next(1, 168)) ?? DateTime.Now; // Random time within a week
                        var isLateSubmission = submissionDate > assessment.DueDate;

                        submissions.Add(new SubmissionModel
                        {
                            AssessmentId = assessment.Id,
                            StudentId = student.Id,
                            AttemptNumber = attempt,
                            Status = "Submitted",
                            SubmittedAt = submissionDate,
                            MaxMarks = assessment.MaxMarks,
                            TotalMarks = GenerateRandomScore(assessment.MaxMarks),
                            IsLateSubmission = isLateSubmission,
                            CreatedAt = submissionDate
                        });
                    }
                }

                submissionId++;
            }

            // Calculate percentages for submissions
            foreach (var submission in submissions)
            {
                if (submission.TotalMarks.HasValue && submission.MaxMarks.HasValue && submission.MaxMarks > 0)
                {
                    submission.Percentage = (submission.TotalMarks.Value / submission.MaxMarks.Value) * 100;
                    if (submission.IsLateSubmission)
                    {
                        // Apply 10% late penalty
                        submission.Percentage = Math.Max(0, submission.Percentage.Value - 10);
                    }
                }
            }

            await context.Submissions.AddRangeAsync(submissions);
            await context.SaveChangesAsync();

            // Seed Answers for submissions
            var answers = new List<AnswerModel>();
            
            foreach (var submission in submissions)
            {
                var assessmentQuestions = questions.Where(q => q.AssessmentId == submission.AssessmentId).ToList();
                
                foreach (var question in assessmentQuestions)
                {
                    answers.Add(new AnswerModel
                    {
                        SubmissionId = submission.Id,
                        QuestionId = question.Id,
                        AnswerText = GenerateAnswerText(question.QuestionText ?? "Question", question.QuestionTypeId, questionTypes),
                        MaxMarks = question.Marks,
                        MarksAwarded = GenerateRandomScore(question.Marks),
                        CreatedAt = submission.CreatedAt
                    });
                }
            }

            await context.Answers.AddRangeAsync(answers);
            await context.SaveChangesAsync();

            // Seed Gradings for completed submissions
            var gradings = new List<GradingModel>();
            var defaultGradingScale = gradingScales.FirstOrDefault(gs => gs.IsDefault) ?? gradingScales.FirstOrDefault();
            
            foreach (var submission in submissions.Where(s => s.Status == "Submitted"))
            {
                var grader = assessors.Any() ? assessors[Random.Shared.Next(assessors.Count)] : 
                            lecturers[Random.Shared.Next(lecturers.Count)];

                var assessment = assessments.First(a => a.Id == submission.AssessmentId);
                
                gradings.Add(new GradingModel
                {
                    StudentId = submission.StudentId,
                    AssessmentId = submission.AssessmentId,
                    SubmissionId = submission.Id,
                    Score = submission.TotalMarks ?? 0,
                    MaxScore = submission.MaxMarks ?? assessment.MaxMarks,
                    Percentage = submission.Percentage ?? 0,
                    GradedBy = grader.Id,
                    GradedAt = submission.CreatedAt.AddDays(Random.Shared.Next(1, 5)),
                    GradingScaleId = defaultGradingScale?.Id,
                    Comments = GenerateGradingComments(submission.Percentage ?? 0),
                    Status = "Completed",
                    CreatedAt = submission.CreatedAt
                });
            }

            await context.Gradings.AddRangeAsync(gradings);
            await context.SaveChangesAsync();

            // Create some Rubrics for assignment-type assessments
            var assignmentAssessments = assessments.Where(a => a.Title?.Contains("Assignment") == true).Take(3).ToList();
            var rubrics = new List<RubricModel>();

            foreach (var assessment in assignmentAssessments)
            {
                rubrics.Add(new RubricModel
                {
                    AssessmentId = assessment.Id,
                    Name = $"{assessment.Title} - Grading Rubric",
                    Description = "Detailed rubric for assignment grading",
                    MaxScore = assessment.MaxMarks,
                    CreatedAt = assessment.CreatedAt
                });
            }

            await context.Rubrics.AddRangeAsync(rubrics);
            await context.SaveChangesAsync();

            // Create Rubric Criteria
            var rubricCriteria = new List<RubricCriterionModel>();

            foreach (var rubric in rubrics)
            {
                var criteria = new[]
                {
                    new { Name = "Content Knowledge", Description = "Demonstrates understanding of subject matter", Weight = 0.4m, MaxScore = rubric.MaxScore * 0.4m },
                    new { Name = "Analysis & Evaluation", Description = "Critical thinking and analysis skills", Weight = 0.3m, MaxScore = rubric.MaxScore * 0.3m },
                    new { Name = "Presentation & Organization", Description = "Clear structure and presentation", Weight = 0.2m, MaxScore = rubric.MaxScore * 0.2m },
                    new { Name = "Research & References", Description = "Quality of research and citations", Weight = 0.1m, MaxScore = rubric.MaxScore * 0.1m }
                };

                for (int i = 0; i < criteria.Length; i++)
                {
                    var criterion = criteria[i];
                    rubricCriteria.Add(new RubricCriterionModel
                    {
                        RubricId = rubric.Id,
                        Name = criterion.Name,
                        Description = criterion.Description,
                        MaxScore = criterion.MaxScore,
                        Weight = criterion.Weight,
                        SortOrder = i + 1,
                        CreatedAt = rubric.CreatedAt
                    });
                }
            }

            await context.RubricCriteria.AddRangeAsync(rubricCriteria);
            await context.SaveChangesAsync();

            Console.WriteLine("✅ Assessment System dummy data seeded successfully!");
            Console.WriteLine($"Created {assessments.Count} assessments");
            Console.WriteLine($"Created {questions.Count} questions");
            Console.WriteLine($"Created {answerOptions.Count} answer options");
            Console.WriteLine($"Created {submissions.Count} submissions");
            Console.WriteLine($"Created {answers.Count} answers");
            Console.WriteLine($"Created {gradings.Count} gradings");
            Console.WriteLine($"Created {rubrics.Count} rubrics with {rubricCriteria.Count} criteria");
        }

        private static string GenerateQuestionText(string assessmentTitle, int questionNumber, string questionType)
        {
            var subject = assessmentTitle.Split(" - ")[0];
            
            var questions = questionType switch
            {
                "Multiple Choice" => new[]
                {
                    $"Which of the following is a key concept in {subject}?",
                    $"What is the primary purpose of {subject.ToLower()} methodologies?",
                    $"In {subject}, which approach is most commonly used for problem-solving?",
                    $"Which statement best describes {subject.ToLower()} principles?",
                    $"What is the correct sequence in {subject.ToLower()} processes?"
                },
                "Short Answer" => new[]
                {
                    $"Explain the main concepts covered in {subject}.",
                    $"Describe how {subject.ToLower()} applies to real-world scenarios.",
                    $"What are the key benefits of studying {subject}?",
                    $"Compare and contrast different approaches in {subject}.",
                    $"Analyze the importance of {subject.ToLower()} in modern technology."
                },
                "Essay" => new[]
                {
                    $"Critically evaluate the role of {subject} in contemporary technology solutions. Provide specific examples and justify your arguments with relevant theory and practice.",
                    $"Design and propose a comprehensive solution using {subject} principles. Your answer should include methodology, implementation considerations, and expected outcomes.",
                    $"Analyze the evolution of {subject} and predict future developments. Discuss how current trends might influence the field in the next decade."
                },
                _ => new[] { $"Question {questionNumber} about {subject}" }
            };

            return questions[questionNumber % questions.Length];
        }

        private static List<(string Text, bool IsCorrect)> GenerateAnswerOptions(string questionText)
        {
            return new List<(string Text, bool IsCorrect)>
            {
                ("Option A - This is the correct answer based on fundamental principles", true),
                ("Option B - This is a plausible but incorrect alternative", false),
                ("Option C - This is another distractor option", false),
                ("Option D - This is clearly incorrect but might seem appealing", false)
            };
        }

        private static string GenerateAnswerText(string questionText, int questionTypeId, List<QuestionTypeModel> questionTypes)
        {
            var questionType = questionTypes.FirstOrDefault(qt => qt.Id == questionTypeId);
            
            return questionType?.Name switch
            {
                "Multiple Choice" => "A", // Selected option A
                "Short Answer" => "This is a concise answer demonstrating understanding of the key concepts discussed in the question.",
                "Essay" => "This is a comprehensive essay response that addresses all aspects of the question. The answer demonstrates critical thinking, analysis, and application of relevant theories and concepts. Multiple paragraphs would typically elaborate on different points with supporting evidence and examples.",
                _ => "Sample answer text"
            };
        }

        private static decimal GenerateRandomScore(decimal maxScore)
        {
            // Generate scores with realistic distribution (more scores in 60-90% range)
            var random = Random.Shared;
            var percentage = random.NextDouble();
            
            // Weight towards higher scores (typical grade distribution)
            if (percentage < 0.1) // 10% low scores (40-60%)
                percentage = 0.4 + (random.NextDouble() * 0.2);
            else if (percentage < 0.3) // 20% medium scores (60-75%)
                percentage = 0.6 + (random.NextDouble() * 0.15);
            else // 70% higher scores (75-95%)
                percentage = 0.75 + (random.NextDouble() * 0.2);

            return Math.Round(maxScore * (decimal)percentage, 2);
        }

        private static string GenerateGradingComments(decimal percentage)
        {
            return percentage switch
            {
                >= 90 => "Excellent work! Demonstrates exceptional understanding and application of concepts.",
                >= 80 => "Very good work. Shows strong grasp of the material with minor areas for improvement.",
                >= 70 => "Good effort. Understanding is evident but could benefit from more depth in analysis.",
                >= 60 => "Satisfactory work. Meets basic requirements but needs improvement in several areas.",
                >= 50 => "Below expectations. Requires significant improvement to meet learning objectives.",
                _ => "Unsatisfactory. Please review the material and seek additional support."
            };
        }
    }
}