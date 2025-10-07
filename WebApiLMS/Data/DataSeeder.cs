using Microsoft.EntityFrameworkCore;
using WebApiLMS.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebApiLMS.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(WebApiLMSDbContext context)
        {
            // Always seed Assessment System lookup tables first
            await SeedAssessmentSystemLookups(context);
            
            // Always try to seed Assessment System data independently
            await AssessmentDataSeeder.SeedAssessmentDummyDataAsync(context);

            // Check if basic LMS data already exists
            if (await context.Users.AnyAsync())
            {
                return; // Basic LMS data already seeded
            }

            // Get UserRole IDs
            var studentRoleId = await context.UserRoles.Where(ur => ur.Code == "Student").Select(ur => ur.Id).FirstOrDefaultAsync();
            var lecturerRoleId = await context.UserRoles.Where(ur => ur.Code == "Lecturer").Select(ur => ur.Id).FirstOrDefaultAsync();
            var adminRoleId = await context.UserRoles.Where(ur => ur.Code == "Admin").Select(ur => ur.Id).FirstOrDefaultAsync();
            var assessorRoleId = await context.UserRoles.Where(ur => ur.Code == "Assessor").Select(ur => ur.Id).FirstOrDefaultAsync();
            var moderatorRoleId = await context.UserRoles.Where(ur => ur.Code == "Moderator").Select(ur => ur.Id).FirstOrDefaultAsync();

            // Hash password "12345678"
            var passwordHash = HashPassword("12345678");

            // Create Users
            var users = new List<Users>
            {
                // Admin Users
                new Users { FullName = "John Admin", Email = "admin@lms.com", PasswordHash = passwordHash, UserRoleId = adminRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Sarah Manager", Email = "manager@lms.com", PasswordHash = passwordHash, UserRoleId = adminRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },

                // Lecturers
                new Users { FullName = "Dr. Michael Smith", Email = "michael.smith@lms.com", PasswordHash = passwordHash, UserRoleId = lecturerRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Prof. Emily Johnson", Email = "emily.johnson@lms.com", PasswordHash = passwordHash, UserRoleId = lecturerRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Dr. David Brown", Email = "david.brown@lms.com", PasswordHash = passwordHash, UserRoleId = lecturerRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Dr. Lisa Wilson", Email = "lisa.wilson@lms.com", PasswordHash = passwordHash, UserRoleId = lecturerRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },

                // Assessors
                new Users { FullName = "Mark Assessor", Email = "mark.assessor@lms.com", PasswordHash = passwordHash, UserRoleId = assessorRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Jane Evaluator", Email = "jane.evaluator@lms.com", PasswordHash = passwordHash, UserRoleId = assessorRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },

                // Moderators
                new Users { FullName = "Tom Moderator", Email = "tom.moderator@lms.com", PasswordHash = passwordHash, UserRoleId = moderatorRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },

                // Students
                new Users { FullName = "Alice Student", Email = "alice@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Bob Wilson", Email = "bob@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Carol Johnson", Email = "carol@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Daniel Martinez", Email = "daniel@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Eva Garcia", Email = "eva@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Frank Anderson", Email = "frank@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Grace Taylor", Email = "grace@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Henry Thompson", Email = "henry@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Ivy Martinez", Email = "ivy@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now },
                new Users { FullName = "Jack Rodriguez", Email = "jack@student.com", PasswordHash = passwordHash, UserRoleId = studentRoleId, AccountStatus = "Active", CreatedAt = DateTime.Now }
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            // Get ProgramType IDs
            var degreeTypeId = await context.ProgramTypes.Where(pt => pt.Code == "Degree").Select(pt => pt.Id).FirstOrDefaultAsync();
            var learnershipTypeId = await context.ProgramTypes.Where(pt => pt.Code == "Learnership").Select(pt => pt.Id).FirstOrDefaultAsync();
            var skillsTypeId = await context.ProgramTypes.Where(pt => pt.Code == "SkillsProgramme").Select(pt => pt.Id).FirstOrDefaultAsync();

            // Create Programs
            var programs = new List<ProgramModel>
            {
                new ProgramModel { Name = "Bachelor of Computer Science", Code = "BCS001", Level = "Bachelor", Department = "Computer Science", Status = "Active", Description = "Comprehensive computer science degree program", ProgramTypeId = degreeTypeId, DurationMonths = 48, CreatedAt = DateTime.Now },
                new ProgramModel { Name = "Software Development Learnership", Code = "SDL001", Level = "NQF Level 5", Department = "IT", Status = "Active", Description = "Practical software development skills", ProgramTypeId = learnershipTypeId, DurationMonths = 12, CreatedAt = DateTime.Now },
                new ProgramModel { Name = "Web Development Skills Programme", Code = "WDS001", Level = "NQF Level 4", Department = "IT", Status = "Active", Description = "Modern web development technologies", ProgramTypeId = skillsTypeId, DurationMonths = 6, CreatedAt = DateTime.Now },
                new ProgramModel { Name = "Data Science Degree", Code = "DS001", Level = "Bachelor", Department = "Data Science", Status = "Active", Description = "Advanced data science and analytics", ProgramTypeId = degreeTypeId, DurationMonths = 48, CreatedAt = DateTime.Now },
                new ProgramModel { Name = "Cybersecurity Skills Programme", Code = "CSS001", Level = "NQF Level 5", Department = "IT", Status = "Active", Description = "Information security fundamentals", ProgramTypeId = skillsTypeId, DurationMonths = 8, CreatedAt = DateTime.Now }
            };

            context.Programs.AddRange(programs);
            await context.SaveChangesAsync();

            // Create Courses
            var courses = new List<CourseModel>
            {
                // Computer Science Courses
                new CourseModel { CourseName = "Programming Fundamentals", Description = "Introduction to programming concepts and logic", Category = "Programming", Difficulty = "Beginner", Syllabus = "Variables, Control Structures, Functions, OOP Basics", Prerequisites = "Basic Mathematics", CreatedAt = DateTime.Now },
                new CourseModel { CourseName = "Data Structures and Algorithms", Description = "Core data structures and algorithmic thinking", Category = "Programming", Difficulty = "Intermediate", Syllabus = "Arrays, Linked Lists, Trees, Graphs, Sorting, Searching", Prerequisites = "Programming Fundamentals", CreatedAt = DateTime.Now },
                new CourseModel { CourseName = "Database Design", Description = "Relational database design and SQL", Category = "Database", Difficulty = "Intermediate", Syllabus = "ER Modeling, Normalization, SQL, Stored Procedures", Prerequisites = "Programming Fundamentals", CreatedAt = DateTime.Now },
                new CourseModel { CourseName = "Web Development", Description = "Modern web application development", Category = "Web", Difficulty = "Intermediate", Syllabus = "HTML, CSS, JavaScript, React, Node.js", Prerequisites = "Programming Fundamentals", CreatedAt = DateTime.Now },
                new CourseModel { CourseName = "Software Engineering", Description = "Software development lifecycle and methodologies", Category = "Engineering", Difficulty = "Advanced", Syllabus = "SDLC, Agile, Testing, Design Patterns", Prerequisites = "Data Structures and Algorithms", CreatedAt = DateTime.Now },
                
                // Data Science Courses
                new CourseModel { CourseName = "Statistics for Data Science", Description = "Statistical foundations for data analysis", Category = "Statistics", Difficulty = "Intermediate", Syllabus = "Descriptive Statistics, Probability, Hypothesis Testing", Prerequisites = "Mathematics", CreatedAt = DateTime.Now },
                new CourseModel { CourseName = "Machine Learning", Description = "Introduction to machine learning algorithms", Category = "AI/ML", Difficulty = "Advanced", Syllabus = "Supervised Learning, Unsupervised Learning, Neural Networks", Prerequisites = "Statistics for Data Science", CreatedAt = DateTime.Now },
                new CourseModel { CourseName = "Data Visualization", Description = "Creating effective data visualizations", Category = "Visualization", Difficulty = "Intermediate", Syllabus = "Charts, Graphs, Interactive Dashboards, Tableau", Prerequisites = "Statistics for Data Science", CreatedAt = DateTime.Now },

                // Cybersecurity Courses
                new CourseModel { CourseName = "Network Security", Description = "Securing network infrastructure", Category = "Security", Difficulty = "Intermediate", Syllabus = "Firewalls, VPNs, Intrusion Detection, Network Protocols", Prerequisites = "Networking Fundamentals", CreatedAt = DateTime.Now },
                new CourseModel { CourseName = "Ethical Hacking", Description = "Penetration testing and vulnerability assessment", Category = "Security", Difficulty = "Advanced", Syllabus = "Penetration Testing, Social Engineering, Vulnerability Assessment", Prerequisites = "Network Security", CreatedAt = DateTime.Now }
            };

            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();

            // Create Program-Course relationships
            var programCourses = new List<ProgramCourseModel>();
            
            // Bachelor of Computer Science courses
            var bcsProgram = programs[0];
            var csCourses = courses.Take(5).ToList();
            foreach (var course in csCourses)
            {
                programCourses.Add(new ProgramCourseModel 
                { 
                    ProgramId = bcsProgram.Id, 
                    CourseId = course.Id, 
                    IsCompulsory = true, 
                    CreatedAt = DateTime.Now 
                });
            }

            // Data Science program courses
            var dsProgram = programs[3];
            var dsCourses = new[] { courses[0], courses[5], courses[6], courses[7] }; // Programming + Data Science courses
            foreach (var course in dsCourses)
            {
                programCourses.Add(new ProgramCourseModel 
                { 
                    ProgramId = dsProgram.Id, 
                    CourseId = course.Id, 
                    IsCompulsory = true, 
                    CreatedAt = DateTime.Now 
                });
            }

            // Web Development Skills Programme
            var webProgram = programs[2];
            var webCourses = new[] { courses[0], courses[3] }; // Programming + Web Development
            foreach (var course in webCourses)
            {
                programCourses.Add(new ProgramCourseModel 
                { 
                    ProgramId = webProgram.Id, 
                    CourseId = course.Id, 
                    IsCompulsory = true, 
                    CreatedAt = DateTime.Now 
                });
            }

            // Cybersecurity Skills Programme
            var cyberProgram = programs[4];
            var cyberCourses = courses.Skip(8).Take(2).ToList(); // Network Security + Ethical Hacking
            foreach (var course in cyberCourses)
            {
                programCourses.Add(new ProgramCourseModel 
                { 
                    ProgramId = cyberProgram.Id, 
                    CourseId = course.Id, 
                    IsCompulsory = true, 
                    CreatedAt = DateTime.Now 
                });
            }

            context.ProgramCourses.AddRange(programCourses);
            await context.SaveChangesAsync();

            // Create User Program Enrollments (Students enrolled in programs)
            var students = users.Where(u => u.UserRoleId == studentRoleId).ToList();
            var userProgramEnrollments = new List<UserProgramEnrollmentModel>();

            for (int i = 0; i < students.Count; i++)
            {
                var programIndex = i % programs.Count;
                userProgramEnrollments.Add(new UserProgramEnrollmentModel
                {
                    UserId = students[i].Id,
                    ProgramId = programs[programIndex].Id,
                    Status = "Active",
                    EnrolledAt = DateTime.Now.AddDays(-30),
                    ExpectedCompletionDate = DateTime.Now.AddMonths(programs[programIndex].DurationMonths)
                });
            }

            context.UserProgramEnrollments.AddRange(userProgramEnrollments);
            await context.SaveChangesAsync();

            // Create Course Lecturer Assignments
            var lecturers = users.Where(u => u.UserRoleId == lecturerRoleId).ToList();
            var courseLecturerAssignments = new List<CourseLecturerAssignmentModel>();

            for (int i = 0; i < courses.Count; i++)
            {
                var lecturerIndex = i % lecturers.Count;
                courseLecturerAssignments.Add(new CourseLecturerAssignmentModel
                {
                    CourseId = courses[i].Id,
                    UserId = lecturers[lecturerIndex].Id,
                    AssignedAt = DateTime.Now.AddDays(-15)
                });
            }

            context.CourseLecturerAssignments.AddRange(courseLecturerAssignments);
            await context.SaveChangesAsync();

            // Create Course Student Enrollments
            var courseStudentEnrollments = new List<CourseStudentEnrollmentModel>();

            foreach (var enrollment in userProgramEnrollments)
            {
                var programCourseIds = programCourses
                    .Where(pc => pc.ProgramId == enrollment.ProgramId)
                    .Select(pc => pc.CourseId)
                    .ToList();

                foreach (var courseId in programCourseIds)
                {
                    courseStudentEnrollments.Add(new CourseStudentEnrollmentModel
                    {
                        CourseId = courseId,
                        StudentId = enrollment.UserId,
                        EnrolledAt = enrollment.EnrolledAt,
                        Progress = Random.Shared.NextDouble() * 100 // Random progress 0-100%
                    });
                }
            }

            context.CourseStudentEnrollments.AddRange(courseStudentEnrollments);
            await context.SaveChangesAsync();

            // Create SETA Bodies
            var setaBodies = new List<SetaBodyModel>
            {
                new SetaBodyModel { Code = "MICT", Name = "Media, Information and Communication Technologies Sector Education and Training Authority" },
                new SetaBodyModel { Code = "SERVICES", Name = "Services Sector Education and Training Authority" },
                new SetaBodyModel { Code = "MERSETA", Name = "Manufacturing, Engineering and Related Services Education and Training Authority" }
            };

            context.SetaBodies.AddRange(setaBodies);
            await context.SaveChangesAsync();

            // Create User Credentials for some lecturers
            var userCredentials = new List<UserCredentialModel>
            {
                new UserCredentialModel
                {
                    UserId = lecturers[0].Id,
                    CredentialType = "Lecturer",
                    RegistrationNumber = "LEC001234",
                    SetaBodyId = setaBodies[0].Id,
                    Scope = "Computer Science, Programming",
                    ValidFrom = DateTime.Now.AddYears(-2),
                    ValidTo = DateTime.Now.AddYears(3),
                    Status = "Active",
                    EvidenceUrl = "https://credentials.seta.gov.za/lecturer/001234"
                },
                new UserCredentialModel
                {
                    UserId = lecturers[1].Id,
                    CredentialType = "Assessor",
                    RegistrationNumber = "ASS005678",
                    SetaBodyId = setaBodies[0].Id,
                    Scope = "Software Development, Data Science",
                    ValidFrom = DateTime.Now.AddYears(-1),
                    ValidTo = DateTime.Now.AddYears(4),
                    Status = "Active",
                    EvidenceUrl = "https://credentials.seta.gov.za/assessor/005678"
                }
            };

            context.UserCredentials.AddRange(userCredentials);
            await context.SaveChangesAsync();

            Console.WriteLine("✅ Dummy data seeded successfully!");
            Console.WriteLine($"Created {users.Count} users");
            Console.WriteLine($"Created {programs.Count} programs");
            Console.WriteLine($"Created {courses.Count} courses");
            Console.WriteLine($"Created {userProgramEnrollments.Count} student enrollments");
            Console.WriteLine($"Created {courseStudentEnrollments.Count} course enrollments");
            Console.WriteLine("All passwords are: 12345678");
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static async Task SeedAssessmentSystemLookups(WebApiLMSDbContext context)
        {
            // Seed Assessment Types
            if (!await context.AssessmentTypes.AnyAsync())
            {
                var assessmentTypes = new List<AssessmentTypeModel>
                {
                    new AssessmentTypeModel { Code = "QUIZ", Name = "Quiz", Description = "Short assessment with multiple questions", IsActive = true },
                    new AssessmentTypeModel { Code = "ASSIGNMENT", Name = "Assignment", Description = "Written assignment with submission", IsActive = true },
                    new AssessmentTypeModel { Code = "EXAM", Name = "Exam", Description = "Formal examination", IsActive = true },
                    new AssessmentTypeModel { Code = "PROJECT", Name = "Project", Description = "Long-term project assessment", IsActive = true },
                    new AssessmentTypeModel { Code = "PORTFOLIO", Name = "Portfolio", Description = "Collection of work over time", IsActive = true }
                };
                await context.AssessmentTypes.AddRangeAsync(assessmentTypes);
            }

            // Seed Assessment Categories
            if (!await context.AssessmentCategories.AnyAsync())
            {
                var assessmentCategories = new List<AssessmentCategoryModel>
                {
                    new AssessmentCategoryModel { Code = "FORMATIVE", Name = "Formative", Description = "Assessment for learning during the course", IsActive = true },
                    new AssessmentCategoryModel { Code = "SUMMATIVE", Name = "Summative", Description = "Assessment of learning at the end of a unit", IsActive = true },
                    new AssessmentCategoryModel { Code = "DIAGNOSTIC", Name = "Diagnostic", Description = "Assessment to identify strengths and weaknesses", IsActive = true },
                    new AssessmentCategoryModel { Code = "PEER", Name = "Peer Assessment", Description = "Assessment by fellow students", IsActive = true },
                    new AssessmentCategoryModel { Code = "SELF", Name = "Self Assessment", Description = "Self-evaluation by the student", IsActive = true }
                };
                await context.AssessmentCategories.AddRangeAsync(assessmentCategories);
            }

            // Seed Question Types
            if (!await context.QuestionTypes.AnyAsync())
            {
                var questionTypes = new List<QuestionTypeModel>
                {
                    new QuestionTypeModel { Name = "Multiple Choice", Description = "Single correct answer from multiple options", AllowsMultipleAnswers = false, SupportsAutoGrading = true },
                    new QuestionTypeModel { Name = "Multiple Select", Description = "Multiple correct answers from options", AllowsMultipleAnswers = true, SupportsAutoGrading = true },
                    new QuestionTypeModel { Name = "True/False", Description = "Binary choice question", AllowsMultipleAnswers = false, SupportsAutoGrading = true },
                    new QuestionTypeModel { Name = "Short Answer", Description = "Brief written response", RequiresTextInput = true, SupportsAutoGrading = false },
                    new QuestionTypeModel { Name = "Essay", Description = "Extended written response", RequiresTextInput = true, SupportsAutoGrading = false },
                    new QuestionTypeModel { Name = "Fill in the Blank", Description = "Complete the missing text", RequiresTextInput = true, SupportsAutoGrading = true },
                    new QuestionTypeModel { Name = "File Upload", Description = "Upload files for assessment", AllowsFileUpload = true, SupportsAutoGrading = false }
                };
                await context.QuestionTypes.AddRangeAsync(questionTypes);
            }

            // Seed Grading Scales
            if (!await context.GradingScales.AnyAsync())
            {
                var gradingScales = new List<GradingScaleModel>
                {
                    new GradingScaleModel { Name = "Percentage", Description = "0-100% grading scale", IsDefault = true, IsActive = true },
                    new GradingScaleModel { Name = "Letter Grade", Description = "A-F letter grading", IsDefault = false, IsActive = true },
                    new GradingScaleModel { Name = "Pass/Fail", Description = "Binary pass or fail", IsDefault = false, IsActive = true },
                    new GradingScaleModel { Name = "Competency", Description = "1-4 competency scale", IsDefault = false, IsActive = true }
                };
                await context.GradingScales.AddRangeAsync(gradingScales);
            }

            await context.SaveChangesAsync();

            // Seed Grade Boundaries (after GradingScales are saved)
            if (!await context.GradeBoundaries.AnyAsync())
            {
                var percentageScale = await context.GradingScales.FirstOrDefaultAsync(gs => gs.Name == "Percentage");
                var letterGradeScale = await context.GradingScales.FirstOrDefaultAsync(gs => gs.Name == "Letter Grade");
                var passFailScale = await context.GradingScales.FirstOrDefaultAsync(gs => gs.Name == "Pass/Fail");
                
                var gradeBoundaries = new List<GradeBoundaryModel>();

                if (percentageScale != null)
                {
                    gradeBoundaries.AddRange(new[]
                    {
                        new GradeBoundaryModel { GradingScaleId = percentageScale.Id, Grade = "A+", Description = "Distinction Plus", MinPercentage = 90, MaxPercentage = 100, GradePoints = 4.0m, IsPassingGrade = true, SortOrder = 1 },
                        new GradeBoundaryModel { GradingScaleId = percentageScale.Id, Grade = "A", Description = "Distinction", MinPercentage = 80, MaxPercentage = 89, GradePoints = 3.5m, IsPassingGrade = true, SortOrder = 2 },
                        new GradeBoundaryModel { GradingScaleId = percentageScale.Id, Grade = "B", Description = "Credit", MinPercentage = 70, MaxPercentage = 79, GradePoints = 3.0m, IsPassingGrade = true, SortOrder = 3 },
                        new GradeBoundaryModel { GradingScaleId = percentageScale.Id, Grade = "C", Description = "Pass", MinPercentage = 60, MaxPercentage = 69, GradePoints = 2.0m, IsPassingGrade = true, SortOrder = 4 },
                        new GradeBoundaryModel { GradingScaleId = percentageScale.Id, Grade = "D", Description = "Marginal Pass", MinPercentage = 50, MaxPercentage = 59, GradePoints = 1.0m, IsPassingGrade = true, SortOrder = 5 },
                        new GradeBoundaryModel { GradingScaleId = percentageScale.Id, Grade = "F", Description = "Fail", MinPercentage = 0, MaxPercentage = 49, GradePoints = 0.0m, IsPassingGrade = false, SortOrder = 6 }
                    });
                }

                if (letterGradeScale != null)
                {
                    gradeBoundaries.AddRange(new[]
                    {
                        new GradeBoundaryModel { GradingScaleId = letterGradeScale.Id, Grade = "A", Description = "Excellent", MinPercentage = 90, MaxPercentage = 100, GradePoints = 4.0m, IsPassingGrade = true, SortOrder = 1 },
                        new GradeBoundaryModel { GradingScaleId = letterGradeScale.Id, Grade = "B", Description = "Good", MinPercentage = 80, MaxPercentage = 89, GradePoints = 3.0m, IsPassingGrade = true, SortOrder = 2 },
                        new GradeBoundaryModel { GradingScaleId = letterGradeScale.Id, Grade = "C", Description = "Satisfactory", MinPercentage = 70, MaxPercentage = 79, GradePoints = 2.0m, IsPassingGrade = true, SortOrder = 3 },
                        new GradeBoundaryModel { GradingScaleId = letterGradeScale.Id, Grade = "D", Description = "Needs Improvement", MinPercentage = 60, MaxPercentage = 69, GradePoints = 1.0m, IsPassingGrade = true, SortOrder = 4 },
                        new GradeBoundaryModel { GradingScaleId = letterGradeScale.Id, Grade = "F", Description = "Failing", MinPercentage = 0, MaxPercentage = 59, GradePoints = 0.0m, IsPassingGrade = false, SortOrder = 5 }
                    });
                }

                if (passFailScale != null)
                {
                    gradeBoundaries.AddRange(new[]
                    {
                        new GradeBoundaryModel { GradingScaleId = passFailScale.Id, Grade = "P", Description = "Pass", MinPercentage = 50, MaxPercentage = 100, GradePoints = 1.0m, IsPassingGrade = true, SortOrder = 1 },
                        new GradeBoundaryModel { GradingScaleId = passFailScale.Id, Grade = "F", Description = "Fail", MinPercentage = 0, MaxPercentage = 49, GradePoints = 0.0m, IsPassingGrade = false, SortOrder = 2 }
                    });
                }

                if (gradeBoundaries.Any())
                {
                    await context.GradeBoundaries.AddRangeAsync(gradeBoundaries);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}