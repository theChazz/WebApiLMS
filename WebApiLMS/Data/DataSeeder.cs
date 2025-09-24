using WebApiLMS.Models;

namespace WebApiLMS.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(WebApiLMSDbContext context)
        {
            // Ensure database is created/migrated
            await context.Database.EnsureCreatedAsync();

            if (!context.SetaBodies.Any())
            {
                var setas = new List<SetaBodyModel>
                {
                    new SetaBodyModel { Code = "LGSETA", Name = "Local Government SETA" },
                    new SetaBodyModel { Code = "ETDP-SETA", Name = "Education, Training and Development Practices SETA" },
                    new SetaBodyModel { Code = "PSETA", Name = "Public Service Sector Education and Training Authority" },
                    new SetaBodyModel { Code = "AgriSETA", Name = "Agricultural Sector Education and Training Authority" },
                    new SetaBodyModel { Code = "EWSETA", Name = "Energy and Water Sector Education and Training Authority" },
                    new SetaBodyModel { Code = "CETA", Name = "Construction Education and Training Authority" },
                    new SetaBodyModel { Code = "MICTSETA", Name = "Media, Information and Communication Technologies SETA" },
                    new SetaBodyModel { Code = "merSETA", Name = "Manufacturing, Engineering and Related Services SETA" },
                    new SetaBodyModel { Code = "FP&M SETA", Name = "Fibre Processing and Manufacturing SETA" },
                    new SetaBodyModel { Code = "Services SETA", Name = "Services Sector Education and Training Authority" },
                    new SetaBodyModel { Code = "TETA", Name = "Transport Education Training Authority" },
                    new SetaBodyModel { Code = "QCTO", Name = "Quality Council for Trades and Occupations" },
                    new SetaBodyModel { Code = "CATHSSETA", Name = "Culture, Arts, Tourism, Hospitality, and Sport Sector Education and Training Authority" },
                };
                context.SetaBodies.AddRange(setas);
                await context.SaveChangesAsync();
            }

            if (!context.ProgramTypes.Any())
            {
                var programTypes = new List<ProgramTypeModel>
                {
                    new ProgramTypeModel { Code = "Degree", Name = "Degree Programme" },
                    new ProgramTypeModel { Code = "Diploma", Name = "Diploma Programme" },
                    new ProgramTypeModel { Code = "Certificate", Name = "Certificate Programme" },
                    new ProgramTypeModel { Code = "Learnership", Name = "Learnership Programme" },
                    new ProgramTypeModel { Code = "Skills", Name = "Skills Programme" },
                    new ProgramTypeModel { Code = "Apprenticeship", Name = "Apprenticeship Programme" }
                };
                context.ProgramTypes.AddRange(programTypes);
                await context.SaveChangesAsync();
            }

            if (!context.Courses.Any())
            {
                var introProgramming = new CourseModel
                {
                    CourseName = "Intro to Programming",
                    Description = "Learn the basics of programming with C#.",
                    Category = "Computer Science",
                    Difficulty = "Beginner",
                    Syllabus = "Week 1: Basics, Week 2: Control Flow, Week 3: OOP",
                    Prerequisites = "None"
                };

                var databaseSystems = new CourseModel
                {
                    CourseName = "Database Systems",
                    Description = "Relational databases, SQL and design principles.",
                    Category = "Information Systems",
                    Difficulty = "Intermediate",
                    Syllabus = "Week 1: ER Modeling, Week 2: SQL, Week 3: Normalization",
                    Prerequisites = "Intro to Programming"
                };

                context.Courses.AddRange(introProgramming, databaseSystems);
                await context.SaveChangesAsync();

                // Add resources
                var now = DateTime.UtcNow;
                var resources = new List<CourseResourceModel>
                {
                    new CourseResourceModel
                    {
                        CourseId = introProgramming.Id,
                        Type = CourseResourceType.Document,
                        Title = "Syllabus PDF",
                        Description = "Course syllabus",
                        Url = "https://example.com/resources/intro-programming/syllabus.pdf",
                        Provider = "file",
                        MimeType = "application/pdf",
                        IsPublished = true,
                        Module = "Overview",
                        SortOrder = 1,
                        CreatedAt = now
                    },
                    new CourseResourceModel
                    {
                        CourseId = introProgramming.Id,
                        Type = CourseResourceType.Document,
                        Title = "Week 1 Slides",
                        Description = "Getting started with C#",
                        Url = "https://example.com/resources/intro-programming/week1-slides.pptx",
                        Provider = "file",
                        MimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                        IsPublished = true,
                        Module = "Week 1",
                        SortOrder = 2,
                        CreatedAt = now
                    },
                    new CourseResourceModel
                    {
                        CourseId = introProgramming.Id,
                        Type = CourseResourceType.Video,
                        Title = "Intro Video",
                        Description = "Welcome and overview",
                        Url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                        Provider = "youtube",
                        MimeType = "text/html",
                        IsPublished = true,
                        Module = "Overview",
                        SortOrder = 3,
                        CreatedAt = now
                    },
                    new CourseResourceModel
                    {
                        CourseId = introProgramming.Id,
                        Type = CourseResourceType.LiveSession,
                        Title = "Weekly Q&A",
                        Description = "Live Q&A session",
                        Url = "https://teams.microsoft.com/l/meetup-join/demo-link",
                        Provider = "teams",
                        StartsAt = now.AddDays(2).AddHours(14),
                        EndsAt = now.AddDays(2).AddHours(15),
                        Timezone = "Africa/Johannesburg",
                        IsPublished = true,
                        Module = "Week 1",
                        SortOrder = 4,
                        CreatedAt = now
                    },
                    new CourseResourceModel
                    {
                        CourseId = databaseSystems.Id,
                        Type = CourseResourceType.Document,
                        Title = "ER Modeling Guide",
                        Description = "ER diagrams and best practices",
                        Url = "https://example.com/resources/db/er-modeling-guide.docx",
                        Provider = "file",
                        MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        IsPublished = true,
                        Module = "Week 1",
                        SortOrder = 1,
                        CreatedAt = now
                    },
                    new CourseResourceModel
                    {
                        CourseId = databaseSystems.Id,
                        Type = CourseResourceType.Document,
                        Title = "SQL Cheat Sheet",
                        Description = "Common SQL queries",
                        Url = "https://example.com/resources/db/sql-cheatsheet.xlsx",
                        Provider = "file",
                        MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        IsPublished = true,
                        Module = "Week 2",
                        SortOrder = 2,
                        CreatedAt = now
                    },
                    new CourseResourceModel
                    {
                        CourseId = databaseSystems.Id,
                        Type = CourseResourceType.Document,
                        Title = "Sample Dataset",
                        Description = "ZIP containing sample CSVs",
                        Url = "https://example.com/resources/db/sample-dataset.zip",
                        Provider = "file",
                        MimeType = "application/zip",
                        IsPublished = true,
                        Module = "Week 2",
                        SortOrder = 3,
                        CreatedAt = now
                    },
                    new CourseResourceModel
                    {
                        CourseId = databaseSystems.Id,
                        Type = CourseResourceType.LiveSession,
                        Title = "SQL Workshop",
                        Description = "Live SQL lab",
                        Url = "https://teams.microsoft.com/l/meetup-join/demo-link-2",
                        Provider = "teams",
                        StartsAt = now.AddDays(3).AddHours(12),
                        EndsAt = now.AddDays(3).AddHours(14),
                        Timezone = "Africa/Johannesburg",
                        IsPublished = true,
                        Module = "Week 2",
                        SortOrder = 4,
                        CreatedAt = now
                    }
                };

                context.CourseResources.AddRange(resources);
                await context.SaveChangesAsync();
            }

            if (!context.Programs.Any())
            {
                var computerScienceDegree = new ProgramModel
                {
                    Name = "Computer Science",
                    Code = "BSc",
                    Level = "7",
                    Department = "Dept of Computer Science",
                    Status = "Active",
                    Description = "The suggested second-choice programmes for BSc (Computer Science) are BSc (Information and Knowledge Systems) and BCom (Informatics).",
                    ProgramTypeId = 1, // Degree Programme
                    DurationMonths = 40
                };

                context.Programs.Add(computerScienceDegree);
                await context.SaveChangesAsync();
            }
        }
    }
}
