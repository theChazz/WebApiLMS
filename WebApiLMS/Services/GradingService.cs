using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApiLMS.Data;
using WebApiLMS.DTOs.Grading;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    /// <summary>
    /// Service implementation for grading operations and grade management
    /// </summary>
    public class GradingService : IGradingService
    {
        private readonly WebApiLMSDbContext _context;

        public GradingService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        #region Basic CRUD Operations

        public async Task<IEnumerable<GradingDto>> GetAllGradesAsync()
        {
            var grades = await _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .Include(g => g.Grader)
                .Include(g => g.GradingScale)
                .ToListAsync();

            return grades.Select(MapToDto);
        }

        public async Task<GradingDto?> GetGradeByIdAsync(int id)
        {
            var grade = await _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .Include(g => g.Grader)
                .Include(g => g.GradingScale)
                .FirstOrDefaultAsync(g => g.Id == id);

            return grade != null ? MapToDto(grade) : null;
        }

        public async Task<GradingDto> CreateGradeAsync(CreateGradingRequest request)
        {
            // Calculate percentage
            var percentage = request.MaxScore > 0 ? (request.Score / request.MaxScore) * 100 : 0;

            var grade = new GradingModel
            {
                StudentId = request.StudentId,
                AssessmentId = request.AssessmentId,
                SubmissionId = request.SubmissionId,
                Score = request.Score,
                MaxScore = request.MaxScore,
                Percentage = percentage,
                GradingScaleId = request.GradingScaleId,
                GradingMethod = request.GradingMethod,
                Comments = request.Comments,
                Feedback = request.Feedback,
                Status = request.Status,
                IsExempt = request.IsExempt,
                ExemptionReason = request.ExemptionReason,
                IsLate = request.IsLate,
                LatePenalty = request.LatePenalty,
                Notes = request.Notes,
                RubricScores = request.RubricScores != null ? JsonSerializer.Serialize(request.RubricScores) : null,
                CreatedAt = DateTime.UtcNow
            };

            // Calculate letter grade if grading scale is provided
            if (request.GradingScaleId.HasValue)
            {
                grade = await CalculateLetterGradeAsync(grade);
            }

            _context.Gradings.Add(grade);
            await _context.SaveChangesAsync();

            return await GetGradeByIdAsync(grade.Id) ?? throw new InvalidOperationException("Failed to retrieve created grade");
        }

        public async Task<GradingDto?> UpdateGradeAsync(int id, UpdateGradingRequest request)
        {
            var grade = await _context.Gradings.FindAsync(id);
            if (grade == null) return null;

            // Update only provided fields
            if (request.Score.HasValue) grade.Score = request.Score.Value;
            if (request.MaxScore.HasValue) grade.MaxScore = request.MaxScore.Value;
            if (request.GradingScaleId.HasValue) grade.GradingScaleId = request.GradingScaleId;
            if (request.GradingMethod != null) grade.GradingMethod = request.GradingMethod;
            if (request.Comments != null) grade.Comments = request.Comments;
            if (request.Feedback != null) grade.Feedback = request.Feedback;
            if (request.Status != null) grade.Status = request.Status;
            if (request.IsExempt.HasValue) grade.IsExempt = request.IsExempt.Value;
            if (request.ExemptionReason != null) grade.ExemptionReason = request.ExemptionReason;
            if (request.IsLate.HasValue) grade.IsLate = request.IsLate.Value;
            if (request.LatePenalty.HasValue) grade.LatePenalty = request.LatePenalty;
            if (request.Notes != null) grade.Notes = request.Notes;
            if (request.RubricScores != null) grade.RubricScores = JsonSerializer.Serialize(request.RubricScores);

            // Recalculate percentage if score or max score changed
            if (request.Score.HasValue || request.MaxScore.HasValue)
            {
                grade.Percentage = grade.MaxScore > 0 ? (grade.Score / grade.MaxScore) * 100 : 0;
            }

            // Recalculate letter grade if needed
            if (grade.GradingScaleId.HasValue)
            {
                grade = await CalculateLetterGradeAsync(grade);
            }

            grade.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetGradeByIdAsync(id);
        }

        public async Task<bool> DeleteGradeAsync(int id)
        {
            var grade = await _context.Gradings.FindAsync(id);
            if (grade == null) return false;

            _context.Gradings.Remove(grade);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Grading-Specific Operations

        public async Task<IEnumerable<GradingDto>> GetGradesByStudentAsync(int studentId)
        {
            var grades = await _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .Include(g => g.Grader)
                .Include(g => g.GradingScale)
                .Where(g => g.StudentId == studentId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();

            return grades.Select(MapToDto);
        }

        public async Task<IEnumerable<GradingDto>> GetGradesByAssessmentAsync(int assessmentId)
        {
            var grades = await _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .Include(g => g.Grader)
                .Include(g => g.GradingScale)
                .Where(g => g.AssessmentId == assessmentId)
                .OrderBy(g => g.Student!.FullName)
                .ToListAsync();

            return grades.Select(MapToDto);
        }

        public async Task<GradingDto?> GetGradeBySubmissionAsync(int submissionId)
        {
            var grade = await _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .Include(g => g.Grader)
                .Include(g => g.GradingScale)
                .FirstOrDefaultAsync(g => g.SubmissionId == submissionId);

            return grade != null ? MapToDto(grade) : null;
        }

        public async Task<IEnumerable<GradeSummaryDto>> GetGradeSummariesAsync(int? studentId = null, int? assessmentId = null)
        {
            var query = _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .AsQueryable();

            if (studentId.HasValue)
                query = query.Where(g => g.StudentId == studentId.Value);

            if (assessmentId.HasValue)
                query = query.Where(g => g.AssessmentId == assessmentId.Value);

            var grades = await query.ToListAsync();

            return grades.Select(g => new GradeSummaryDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                StudentName = g.Student?.FullName,
                AssessmentId = g.AssessmentId,
                AssessmentTitle = g.Assessment?.Title,
                Score = g.Score,
                MaxScore = g.MaxScore,
                Percentage = g.Percentage,
                LetterGrade = g.LetterGrade,
                Status = g.Status,
                IsPublished = g.IsPublished,
                GradedAt = g.GradedAt
            });
        }

        #endregion

        #region Bulk Operations

        public async Task<IEnumerable<GradingDto>> CreateBulkGradesAsync(BulkGradingRequest request)
        {
            var assessment = await _context.Assessments.FindAsync(request.AssessmentId);
            if (assessment == null) throw new ArgumentException("Assessment not found");

            var grades = new List<GradingModel>();

            foreach (var studentGrade in request.Grades)
            {
                var percentage = assessment.MaxMarks > 0 ? (studentGrade.Score / assessment.MaxMarks) * 100 : 0;

                var grade = new GradingModel
                {
                    StudentId = studentGrade.StudentId,
                    AssessmentId = request.AssessmentId,
                    Score = studentGrade.Score,
                    MaxScore = assessment.MaxMarks,
                    Percentage = percentage,
                    GradingMethod = request.GradingMethod,
                    Comments = studentGrade.Comments ?? request.Comments,
                    Feedback = studentGrade.Feedback,
                    Status = request.PublishGrades ? "Published" : "Draft",
                    IsPublished = request.PublishGrades,
                    IsExempt = studentGrade.IsExempt,
                    ExemptionReason = studentGrade.ExemptionReason,
                    CreatedAt = DateTime.UtcNow
                };

                grades.Add(grade);
            }

            _context.Gradings.AddRange(grades);
            await _context.SaveChangesAsync();

            var gradeIds = grades.Select(g => g.Id).ToList();
            var results = await _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .Where(g => gradeIds.Contains(g.Id))
                .ToListAsync();

            return results.Select(MapToDto);
        }

        public async Task<bool> PublishGradesAsync(PublishGradesRequest request)
        {
            var grades = await _context.Gradings
                .Where(g => request.GradeIds.Contains(g.Id))
                .ToListAsync();

            foreach (var grade in grades)
            {
                grade.IsPublished = true;
                grade.Status = "Published";
                grade.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnpublishGradesAsync(List<int> gradeIds)
        {
            var grades = await _context.Gradings
                .Where(g => gradeIds.Contains(g.Id))
                .ToListAsync();

            foreach (var grade in grades)
            {
                grade.IsPublished = false;
                grade.Status = "Draft";
                grade.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Grade Calculations and Adjustments

        public async Task<GradingDto?> CalculateGradeAsync(int gradeId, int? gradingScaleId = null)
        {
            var grade = await _context.Gradings.FindAsync(gradeId);
            if (grade == null) return null;

            if (gradingScaleId.HasValue)
                grade.GradingScaleId = gradingScaleId;

            if (grade.GradingScaleId.HasValue)
            {
                grade = await CalculateLetterGradeAsync(grade);
                await _context.SaveChangesAsync();
            }

            return await GetGradeByIdAsync(gradeId);
        }

        public async Task<GradingDto?> ApplyGradeAdjustmentAsync(GradeAdjustmentRequest request)
        {
            var grade = await _context.Gradings.FindAsync(request.GradeId);
            if (grade == null) return null;

            var adjustedScore = request.AdjustmentType?.ToLower() switch
            {
                "curve" => grade.Score + request.AdjustmentValue,
                "bonus" => grade.Score + request.AdjustmentValue,
                "penalty" => grade.Score - request.AdjustmentValue,
                "override" => request.AdjustmentValue,
                _ => grade.Score
            };

            // Ensure score doesn't exceed max score (unless it's a bonus)
            if (request.AdjustmentType?.ToLower() != "bonus")
            {
                adjustedScore = Math.Min(adjustedScore, grade.MaxScore);
            }

            adjustedScore = Math.Max(adjustedScore, 0); // Ensure non-negative

            grade.Score = adjustedScore;
            grade.Percentage = grade.MaxScore > 0 ? (grade.Score / grade.MaxScore) * 100 : 0;
            grade.Notes = $"{grade.Notes}\n{request.AdjustmentType} adjustment: {request.AdjustmentValue} - {request.Reason}".Trim();
            grade.UpdatedAt = DateTime.UtcNow;

            if (request.CreateNewVersion)
            {
                grade.Version += 1;
            }

            await _context.SaveChangesAsync();
            return await GetGradeByIdAsync(request.GradeId);
        }

        public async Task<IEnumerable<GradingDto>> ApplyGradeCurveAsync(int assessmentId, decimal curvePercentage)
        {
            var grades = await _context.Gradings
                .Where(g => g.AssessmentId == assessmentId && !g.IsExempt)
                .ToListAsync();

            foreach (var grade in grades)
            {
                var curvePoints = grade.MaxScore * (curvePercentage / 100);
                grade.Score = Math.Min(grade.Score + curvePoints, grade.MaxScore);
                grade.Percentage = grade.MaxScore > 0 ? (grade.Score / grade.MaxScore) * 100 : 0;
                grade.Notes = $"{grade.Notes}\nCurve applied: +{curvePercentage}%".Trim();
                grade.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return grades.Select(MapToDto);
        }

        #endregion

        #region Statistics and Analytics

        public async Task<object> GetGradeStatisticsAsync(int assessmentId)
        {
            var grades = await _context.Gradings
                .Where(g => g.AssessmentId == assessmentId && !g.IsExempt)
                .ToListAsync();

            if (!grades.Any())
                return new { Message = "No grades found for this assessment" };

            var scores = grades.Select(g => g.Percentage).ToList();

            return new
            {
                TotalStudents = grades.Count,
                AverageScore = scores.Average(),
                MedianScore = CalculateMedian(scores),
                HighestScore = scores.Max(),
                LowestScore = scores.Min(),
                StandardDeviation = CalculateStandardDeviation(scores),
                PassingRate = grades.Count(g => g.Percentage >= 50) / (double)grades.Count * 100,
                GradeDistribution = grades.GroupBy(g => g.LetterGrade)
                    .ToDictionary(g => g.Key ?? "N/A", g => g.Count())
            };
        }

        public async Task<object> GetStudentGradeAnalyticsAsync(int studentId)
        {
            var grades = await _context.Gradings
                .Include(g => g.Assessment)
                .Where(g => g.StudentId == studentId && !g.IsExempt)
                .ToListAsync();

            if (!grades.Any())
                return new { Message = "No grades found for this student" };

            var scores = grades.Select(g => g.Percentage).ToList();

            return new
            {
                TotalAssessments = grades.Count,
                AverageScore = scores.Average(),
                HighestScore = scores.Max(),
                LowestScore = scores.Min(),
                TrendAnalysis = CalculateTrend(grades),
                RecentPerformance = grades.OrderByDescending(g => g.CreatedAt).Take(5)
                    .Select(g => new { g.Assessment?.Title, g.Percentage, g.LetterGrade })
            };
        }

        public async Task<IEnumerable<GradingDto>> GetTopPerformersAsync(int assessmentId, int count = 10)
        {
            var topGrades = await _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .Include(g => g.GradingScale)
                .Where(g => g.AssessmentId == assessmentId && !g.IsExempt)
                .OrderByDescending(g => g.Percentage)
                .Take(count)
                .ToListAsync();

            return topGrades.Select(MapToDto);
        }

        #endregion

        #region Grade History and Versioning

        public async Task<IEnumerable<GradingDto>> GetGradeHistoryAsync(int studentId, int assessmentId)
        {
            var grades = await _context.Gradings
                .Include(g => g.Student)
                .Include(g => g.Assessment)
                .Include(g => g.Grader)
                .Include(g => g.GradingScale)
                .Where(g => g.StudentId == studentId && g.AssessmentId == assessmentId)
                .OrderByDescending(g => g.Version)
                .ToListAsync();

            return grades.Select(MapToDto);
        }

        public async Task<GradingDto?> RevertToGradeVersionAsync(int gradeId, int version)
        {
            var currentGrade = await _context.Gradings.FindAsync(gradeId);
            if (currentGrade == null) return null;

            var targetVersion = await _context.Gradings
                .FirstOrDefaultAsync(g => g.PreviousGradeId == gradeId && g.Version == version);

            if (targetVersion == null) return null;

            // Create new version with reverted data
            currentGrade.Score = targetVersion.Score;
            currentGrade.MaxScore = targetVersion.MaxScore;
            currentGrade.Percentage = targetVersion.Percentage;
            currentGrade.LetterGrade = targetVersion.LetterGrade;
            currentGrade.Comments = targetVersion.Comments;
            currentGrade.Feedback = targetVersion.Feedback;
            currentGrade.Version += 1;
            currentGrade.UpdatedAt = DateTime.UtcNow;
            currentGrade.Notes = $"{currentGrade.Notes}\nReverted to version {version}".Trim();

            await _context.SaveChangesAsync();
            return await GetGradeByIdAsync(gradeId);
        }

        #endregion

        #region Import/Export (Simplified Implementation)

        public async Task<IEnumerable<GradingDto>> ImportGradesFromCsvAsync(int assessmentId, Stream csvStream)
        {
            // This is a simplified implementation
            // In a real application, you would use a CSV parsing library like CsvHelper
            throw new NotImplementedException("CSV import functionality needs to be implemented with a CSV parsing library");
        }

        public async Task<Stream> ExportGradesToCsvAsync(int assessmentId)
        {
            // This is a simplified implementation
            // In a real application, you would generate CSV content
            throw new NotImplementedException("CSV export functionality needs to be implemented");
        }

        #endregion

        #region Validation and Utilities

        public async Task<bool> ValidateGradeAsync(CreateGradingRequest request)
        {
            // Check if student exists
            var studentExists = await _context.Users.AnyAsync(u => u.Id == request.StudentId);
            if (!studentExists) return false;

            // Check if assessment exists
            var assessmentExists = await _context.Assessments.AnyAsync(a => a.Id == request.AssessmentId);
            if (!assessmentExists) return false;

            // Check if submission exists (if provided)
            if (request.SubmissionId.HasValue)
            {
                var submissionExists = await _context.Submissions.AnyAsync(s => s.Id == request.SubmissionId.Value);
                if (!submissionExists) return false;
            }

            // Additional validation logic can be added here
            return true;
        }

        public async Task<bool> CanGradeAssessmentAsync(int assessmentId, int graderId)
        {
            // Check if the grader is authorized to grade this assessment
            // This could involve checking instructor assignments, etc.
            var assessment = await _context.Assessments.FindAsync(assessmentId);
            return assessment != null; // Simplified - add proper authorization logic
        }

        public async Task<decimal> CalculateGpaAsync(int studentId, int? termId = null)
        {
            var query = _context.Gradings
                .Where(g => g.StudentId == studentId && g.IsPublished && !g.IsExempt && g.GradePoints.HasValue);

            // Add term filtering if needed (would require term/semester tracking)
            var grades = await query.ToListAsync();

            if (!grades.Any()) return 0;

            var totalPoints = grades.Sum(g => g.GradePoints!.Value);
            var totalCredits = grades.Count; // Simplified - should use actual credit hours

            return totalCredits > 0 ? totalPoints / totalCredits : 0;
        }

        #endregion

        #region Private Helper Methods

        private GradingDto MapToDto(GradingModel grade)
        {
            var dto = new GradingDto
            {
                Id = grade.Id,
                StudentId = grade.StudentId,
                StudentName = grade.Student?.FullName,
                AssessmentId = grade.AssessmentId,
                AssessmentTitle = grade.Assessment?.Title,
                SubmissionId = grade.SubmissionId,
                Score = grade.Score,
                MaxScore = grade.MaxScore,
                Percentage = grade.Percentage,
                LetterGrade = grade.LetterGrade,
                GradePoints = grade.GradePoints,
                GradingScaleId = grade.GradingScaleId,
                GradingScaleName = grade.GradingScale?.Name,
                GradingMethod = grade.GradingMethod,
                Comments = grade.Comments,
                Feedback = grade.Feedback,
                GradedBy = grade.GradedBy,
                GraderName = grade.Grader?.FullName,
                GradedAt = grade.GradedAt,
                Status = grade.Status,
                IsPublished = grade.IsPublished,
                Version = grade.Version,
                IsExempt = grade.IsExempt,
                ExemptionReason = grade.ExemptionReason,
                IsLate = grade.IsLate,
                LatePenalty = grade.LatePenalty,
                Notes = grade.Notes,
                CreatedAt = grade.CreatedAt,
                UpdatedAt = grade.UpdatedAt
            };

            // Parse rubric scores from JSON
            if (!string.IsNullOrEmpty(grade.RubricScores))
            {
                try
                {
                    dto.RubricScores = JsonSerializer.Deserialize<Dictionary<string, decimal>>(grade.RubricScores);
                }
                catch
                {
                    dto.RubricScores = null;
                }
            }

            return dto;
        }

        private async Task<GradingModel> CalculateLetterGradeAsync(GradingModel grade)
        {
            if (!grade.GradingScaleId.HasValue) return grade;

            var gradeBoundary = await _context.GradeBoundaries
                .Where(gb => gb.GradingScaleId == grade.GradingScaleId.Value &&
                           gb.MinPercentage <= grade.Percentage &&
                           gb.MaxPercentage >= grade.Percentage)
                .FirstOrDefaultAsync();

            if (gradeBoundary != null)
            {
                grade.LetterGrade = gradeBoundary.Grade;
                grade.GradePoints = gradeBoundary.GradePoints;
            }

            return grade;
        }

        private static decimal CalculateMedian(List<decimal> values)
        {
            var sorted = values.OrderBy(x => x).ToList();
            var count = sorted.Count;
            
            if (count % 2 == 0)
            {
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2;
            }
            else
            {
                return sorted[count / 2];
            }
        }

        private static decimal CalculateStandardDeviation(List<decimal> values)
        {
            var average = values.Average();
            var sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            var standardDeviation = Math.Sqrt((double)(sumOfSquaresOfDifferences / values.Count));
            return (decimal)standardDeviation;
        }

        private static string CalculateTrend(List<GradingModel> grades)
        {
            if (grades.Count < 2) return "Insufficient data";

            var recentGrades = grades.OrderBy(g => g.CreatedAt).TakeLast(5).ToList();
            if (recentGrades.Count < 2) return "Insufficient data";

            var oldAvg = recentGrades.Take(recentGrades.Count / 2).Average(g => g.Percentage);
            var newAvg = recentGrades.Skip(recentGrades.Count / 2).Average(g => g.Percentage);

            var difference = newAvg - oldAvg;
            return difference switch
            {
                > 5 => "Improving",
                < -5 => "Declining", 
                _ => "Stable"
            };
        }

        #endregion
    }
}