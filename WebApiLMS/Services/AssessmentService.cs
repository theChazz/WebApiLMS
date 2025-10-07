using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.Models;
using WebApiLMS.DTOs.Assessment;

namespace WebApiLMS.Services
{
    /// <summary>
    /// Service for Assessment operations
    /// </summary>
    public class AssessmentService : IAssessmentService
    {
        private readonly WebApiLMSDbContext _context;

        public AssessmentService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssessmentModel>> GetAllAsync()
        {
            return await _context.Assessments
                .Include(a => a.Course)
                .Include(a => a.AssessmentType)
                .Include(a => a.AssessmentCategory)
                .ToListAsync();
        }

        public async Task<List<AssessmentModel>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Assessments
                .Include(a => a.Course)
                .Include(a => a.AssessmentType)
                .Include(a => a.AssessmentCategory)
                .Where(a => a.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<AssessmentModel?> GetByIdAsync(int id)
        {
            return await _context.Assessments
                .Include(a => a.Course)
                .Include(a => a.AssessmentType)
                .Include(a => a.AssessmentCategory)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AssessmentModel> CreateAsync(CreateAssessmentRequest request)
        {
            try
            {
                // Skip validation temporarily to avoid timeout issues
                // TODO: Re-enable after ensuring reference tables exist
                
                var assessment = new AssessmentModel
                {
                    CourseId = request.CourseId,
                    Title = request.Title,
                    Description = request.Description,
                    AssessmentTypeId = request.AssessmentTypeId,
                    AssessmentCategoryId = request.AssessmentCategoryId,
                    MaxMarks = request.MaxMarks,
                    Duration = request.Duration,
                    AttemptsAllowed = request.AttemptsAllowed,
                    WeightingPercentage = request.WeightingPercentage,
                    OpenDate = request.OpenDate,
                    DueDate = request.DueDate,
                    CloseDate = request.CloseDate,
                    RequiresModeration = request.RequiresModeration,
                    RequiresExternalModeration = request.RequiresExternalModeration,
                    ModerationPercentage = request.ModerationPercentage,
                    IsPublished = false,
                    IsActive = false,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Assessments.Add(assessment);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(assessment.Id) ?? assessment;
            }
            catch (Exception ex)
            {
                // Log the actual error and throw a more specific message
                throw new InvalidOperationException($"Failed to create assessment: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateAssessmentRequest request)
        {
            try
            {
                var assessment = await _context.Assessments.FindAsync(id);
                if (assessment == null) return false;

                assessment.Title = request.Title;
                assessment.Description = request.Description;
                assessment.AssessmentTypeId = request.AssessmentTypeId;
                assessment.AssessmentCategoryId = request.AssessmentCategoryId;
                assessment.MaxMarks = request.MaxMarks;
                assessment.Duration = request.Duration;
                assessment.AttemptsAllowed = request.AttemptsAllowed;
                assessment.WeightingPercentage = request.WeightingPercentage;
                assessment.OpenDate = request.OpenDate;
                assessment.DueDate = request.DueDate;
                assessment.CloseDate = request.CloseDate;
                assessment.RequiresModeration = request.RequiresModeration;
                assessment.RequiresExternalModeration = request.RequiresExternalModeration;
                assessment.ModerationPercentage = request.ModerationPercentage;
                assessment.IsPublished = request.IsPublished;
                assessment.IsActive = request.IsActive;
                assessment.ModifiedBy = request.ModifiedBy;
                assessment.ModifiedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var assessment = await _context.Assessments.FindAsync(id);
                if (assessment == null) return false;

                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PublishAsync(int id, int publishedBy)
        {
            try
            {
                var assessment = await _context.Assessments.FindAsync(id);
                if (assessment == null) return false;

                assessment.IsPublished = true;
                assessment.IsActive = true;
                assessment.ModifiedBy = publishedBy;
                assessment.ModifiedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<AssessmentStatisticsDto> GetStatisticsAsync(int id)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessment == null)
            {
                throw new ArgumentException("Assessment not found", nameof(id));
            }

            var questions = await _context.Questions
                .Where(q => q.AssessmentId == id)
                .ToListAsync();

            var submissions = await _context.Submissions
                .Where(s => s.AssessmentId == id)
                .ToListAsync();

            var completedSubmissions = submissions.Where(s => s.Status == "Submitted" || s.Status == "Marked").ToList();

            var statistics = new AssessmentStatisticsDto
            {
                AssessmentId = id,
                AssessmentTitle = assessment.Title,
                TotalQuestions = questions.Count,
                TotalMarks = questions.Sum(q => q.Marks),
                TotalSubmissions = submissions.Count,
                CompletedSubmissions = completedSubmissions.Count,
                PendingSubmissions = submissions.Count(s => s.Status == "Submitted"),
                DraftSubmissions = submissions.Count(s => s.Status == "Draft")
            };

            if (completedSubmissions.Any())
            {
                var scores = completedSubmissions.Where(s => s.Percentage.HasValue).Select(s => s.Percentage!.Value).ToList();
                
                if (scores.Any())
                {
                    statistics.AverageScore = (double)scores.Average();
                    statistics.HighestScore = (double)scores.Max();
                    statistics.LowestScore = (double)scores.Min();
                    statistics.PassRate = scores.Count(s => s >= 50) / (double)scores.Count * 100;
                }

                var completionTimes = completedSubmissions
                    .Where(s => s.TimeTaken.HasValue)
                    .Select(s => s.TimeTaken!.Value)
                    .ToList();

                if (completionTimes.Any())
                {
                    statistics.AverageCompletionTime = TimeSpan.FromTicks((long)completionTimes.Average(t => t.Ticks));
                    statistics.FastestCompletionTime = completionTimes.Min();
                    statistics.SlowestCompletionTime = completionTimes.Max();
                }

                // Grade distribution
                var grades = completedSubmissions.Where(s => !string.IsNullOrEmpty(s.Grade)).GroupBy(s => s.Grade);
                foreach (var gradeGroup in grades)
                {
                    statistics.GradeDistribution[gradeGroup.Key!] = gradeGroup.Count();
                }
            }

            return statistics;
        }

        // Diagnostic methods for testing validation
        public async Task<bool> TestCourseExists(int courseId)
        {
            try
            {
                var course = await _context.Courses.FindAsync(courseId);
                return course != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking course {courseId}: {ex.Message}");
            }
        }

        public async Task<bool> TestAssessmentTypeExists(int assessmentTypeId)
        {
            try
            {
                var assessmentType = await _context.AssessmentTypes.FindAsync(assessmentTypeId);
                return assessmentType != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking assessment type {assessmentTypeId}: {ex.Message}");
            }
        }

        public async Task<bool> TestAssessmentCategoryExists(int assessmentCategoryId)
        {
            try
            {
                var assessmentCategory = await _context.AssessmentCategories.FindAsync(assessmentCategoryId);
                return assessmentCategory != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking assessment category {assessmentCategoryId}: {ex.Message}");
            }
        }

        public async Task<bool> TestUserExists(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                return user != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking user {userId}: {ex.Message}");
            }
        }
    }
}