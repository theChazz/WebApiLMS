using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.DTOs.Submission;
using WebApiLMS.Models;
using System.Text.Json;

namespace WebApiLMS.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly WebApiLMSDbContext _context;

        public SubmissionService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubmissionDto>> GetAllAsync()
        {
            var submissions = await _context.Submissions
                .Include(s => s.Assessment)
                .Include(s => s.Student)
                .Select(s => new SubmissionDto
                {
                    Id = s.Id,
                    AssessmentId = s.AssessmentId,
                    AssessmentTitle = s.Assessment != null ? s.Assessment.Title : "",
                    StudentId = s.StudentId,
                    StudentName = s.Student != null ? s.Student.FullName : "",
                    StudentEmail = s.Student != null ? s.Student.Email : "",
                    SubmittedAt = s.SubmittedAt,
                    TotalMarks = s.TotalMarks ?? 0,
                    MaxMarks = s.MaxMarks ?? 0,
                    Percentage = s.Percentage ?? 0,
                    Status = s.Status ?? "Draft",
                    Feedback = s.GraderComments,
                    IsGraded = s.GradedAt.HasValue,
                    GradedAt = s.GradedAt,
                    GradedByUserId = s.GradedBy,
                    GradedByName = "", // Will be populated separately if needed
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt ?? s.CreatedAt,
                    Answers = new List<AnswerDto>()
                })
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();

            // Get answers separately to avoid complex joins
            foreach (var submission in submissions)
            {
                submission.Answers = await GetAnswersForSubmissionAsync(submission.Id);
            }

            return submissions;
        }

        public async Task<IEnumerable<SubmissionDto>> GetByAssessmentIdAsync(int assessmentId)
        {
            var submissions = await _context.Submissions
                .Include(s => s.Assessment)
                .Include(s => s.Student)
                .Where(s => s.AssessmentId == assessmentId)
                .Select(s => new SubmissionDto
                {
                    Id = s.Id,
                    AssessmentId = s.AssessmentId,
                    AssessmentTitle = s.Assessment != null ? s.Assessment.Title : "",
                    StudentId = s.StudentId,
                    StudentName = s.Student != null ? s.Student.FullName : "",
                    StudentEmail = s.Student != null ? s.Student.Email : "",
                    SubmittedAt = s.SubmittedAt,
                    TotalMarks = s.TotalMarks ?? 0,
                    MaxMarks = s.MaxMarks ?? 0,
                    Percentage = s.Percentage ?? 0,
                    Status = s.Status ?? "Draft",
                    Feedback = s.GraderComments,
                    IsGraded = s.GradedAt.HasValue,
                    GradedAt = s.GradedAt,
                    GradedByUserId = s.GradedBy,
                    GradedByName = "",
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt ?? s.CreatedAt,
                    Answers = new List<AnswerDto>()
                })
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();

            foreach (var submission in submissions)
            {
                submission.Answers = await GetAnswersForSubmissionAsync(submission.Id);
            }

            return submissions;
        }

        public async Task<IEnumerable<SubmissionDto>> GetByStudentIdAsync(int studentId)
        {
            var submissions = await _context.Submissions
                .Include(s => s.Assessment)
                .Include(s => s.Student)
                .Where(s => s.StudentId == studentId)
                .Select(s => new SubmissionDto
                {
                    Id = s.Id,
                    AssessmentId = s.AssessmentId,
                    AssessmentTitle = s.Assessment != null ? s.Assessment.Title : "",
                    StudentId = s.StudentId,
                    StudentName = s.Student != null ? s.Student.FullName : "",
                    StudentEmail = s.Student != null ? s.Student.Email : "",
                    SubmittedAt = s.SubmittedAt,
                    TotalMarks = s.TotalMarks ?? 0,
                    MaxMarks = s.MaxMarks ?? 0,
                    Percentage = s.Percentage ?? 0,
                    Status = s.Status ?? "Draft",
                    Feedback = s.GraderComments,
                    IsGraded = s.GradedAt.HasValue,
                    GradedAt = s.GradedAt,
                    GradedByUserId = s.GradedBy,
                    GradedByName = "",
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt ?? s.CreatedAt,
                    Answers = new List<AnswerDto>()
                })
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();

            foreach (var submission in submissions)
            {
                submission.Answers = await GetAnswersForSubmissionAsync(submission.Id);
            }

            return submissions;
        }

        public async Task<SubmissionDto?> GetByIdAsync(int id)
        {
            var submission = await _context.Submissions
                .Include(s => s.Assessment)
                .Include(s => s.Student)
                .Where(s => s.Id == id)
                .Select(s => new SubmissionDto
                {
                    Id = s.Id,
                    AssessmentId = s.AssessmentId,
                    AssessmentTitle = s.Assessment != null ? s.Assessment.Title : "",
                    StudentId = s.StudentId,
                    StudentName = s.Student != null ? s.Student.FullName : "",
                    StudentEmail = s.Student != null ? s.Student.Email : "",
                    SubmittedAt = s.SubmittedAt,
                    TotalMarks = s.TotalMarks ?? 0,
                    MaxMarks = s.MaxMarks ?? 0,
                    Percentage = s.Percentage ?? 0,
                    Status = s.Status ?? "Draft",
                    Feedback = s.GraderComments,
                    IsGraded = s.GradedAt.HasValue,
                    GradedAt = s.GradedAt,
                    GradedByUserId = s.GradedBy,
                    GradedByName = "",
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt ?? s.CreatedAt,
                    Answers = new List<AnswerDto>()
                })
                .FirstOrDefaultAsync();

            if (submission != null)
            {
                submission.Answers = await GetAnswersForSubmissionAsync(submission.Id);
            }

            return submission;
        }

        public async Task<SubmissionDto?> GetByAssessmentAndStudentAsync(int assessmentId, int studentId)
        {
            var submission = await _context.Submissions
                .Include(s => s.Assessment)
                .Include(s => s.Student)
                .Where(s => s.AssessmentId == assessmentId && s.StudentId == studentId)
                .Select(s => new SubmissionDto
                {
                    Id = s.Id,
                    AssessmentId = s.AssessmentId,
                    AssessmentTitle = s.Assessment != null ? s.Assessment.Title : "",
                    StudentId = s.StudentId,
                    StudentName = s.Student != null ? s.Student.FullName : "",
                    StudentEmail = s.Student != null ? s.Student.Email : "",
                    SubmittedAt = s.SubmittedAt,
                    TotalMarks = s.TotalMarks ?? 0,
                    MaxMarks = s.MaxMarks ?? 0,
                    Percentage = s.Percentage ?? 0,
                    Status = s.Status ?? "Draft",
                    Feedback = s.GraderComments,
                    IsGraded = s.GradedAt.HasValue,
                    GradedAt = s.GradedAt,
                    GradedByUserId = s.GradedBy,
                    GradedByName = "",
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt ?? s.CreatedAt,
                    Answers = new List<AnswerDto>()
                })
                .FirstOrDefaultAsync();

            if (submission != null)
            {
                submission.Answers = await GetAnswersForSubmissionAsync(submission.Id);
            }

            return submission;
        }

        public async Task<SubmissionDto> CreateAsync(CreateSubmissionRequest request)
        {
            var submission = new SubmissionModel
            {
                AssessmentId = request.AssessmentId,
                StudentId = request.StudentId,
                SubmittedAt = DateTime.UtcNow,
                Status = "Submitted",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();

            // Create answers
            if (request.Answers.Any())
            {
                var answers = request.Answers.Select(a => new AnswerModel
                {
                    SubmissionId = submission.Id,
                    QuestionId = a.QuestionId,
                    AnswerText = a.AnswerText,
                    SelectedOptionIds = a.SelectedOptionIds.Any() ? JsonSerializer.Serialize(a.SelectedOptionIds) : null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                _context.Answers.AddRange(answers);
                await _context.SaveChangesAsync();
            }

            return await GetByIdAsync(submission.Id) ?? throw new InvalidOperationException("Failed to create submission");
        }

        public async Task<SubmissionDto?> UpdateAsync(int id, UpdateSubmissionRequest request)
        {
            var submission = await _context.Submissions.FindAsync(id);
            if (submission == null)
                return null;

            submission.UpdatedAt = DateTime.UtcNow;

            // Update answers
            var existingAnswers = await _context.Answers.Where(a => a.SubmissionId == id).ToListAsync();

            foreach (var answerRequest in request.Answers)
            {
                var existingAnswer = existingAnswers.FirstOrDefault(a => a.QuestionId == answerRequest.QuestionId);
                if (existingAnswer != null)
                {
                    existingAnswer.AnswerText = answerRequest.AnswerText;
                    existingAnswer.SelectedOptionIds = answerRequest.SelectedOptionIds.Any() 
                        ? JsonSerializer.Serialize(answerRequest.SelectedOptionIds) 
                        : null;
                    existingAnswer.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var newAnswer = new AnswerModel
                    {
                        SubmissionId = submission.Id,
                        QuestionId = answerRequest.QuestionId,
                        AnswerText = answerRequest.AnswerText,
                        SelectedOptionIds = answerRequest.SelectedOptionIds.Any() 
                            ? JsonSerializer.Serialize(answerRequest.SelectedOptionIds) 
                            : null,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.Answers.Add(newAnswer);
                }
            }

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<SubmissionDto?> GradeAsync(int id, GradeSubmissionRequest request, int gradedByUserId)
        {
            var submission = await _context.Submissions.FindAsync(id);
            if (submission == null)
                return null;

            submission.GraderComments = request.Feedback;
            submission.GradedAt = DateTime.UtcNow;
            submission.GradedBy = gradedByUserId;
            submission.UpdatedAt = DateTime.UtcNow;

            decimal totalMarks = 0;
            decimal maxMarks = 0;

            // Grade individual answers
            foreach (var gradeAnswer in request.Answers)
            {
                var answer = await _context.Answers
                    .Include(a => a.Question)
                    .FirstOrDefaultAsync(a => a.SubmissionId == id && a.QuestionId == gradeAnswer.QuestionId);

                if (answer != null)
                {
                    answer.MarksAwarded = gradeAnswer.MarksAwarded;
                    answer.MaxMarks = answer.Question?.Marks ?? 0;
                    answer.Feedback = gradeAnswer.Feedback;
                    answer.IsCorrect = gradeAnswer.MarksAwarded == answer.MaxMarks;
                    answer.GradedAt = DateTime.UtcNow;
                    answer.GradedBy = gradedByUserId;
                    answer.UpdatedAt = DateTime.UtcNow;

                    totalMarks += gradeAnswer.MarksAwarded;
                    maxMarks += answer.MaxMarks ?? 0;
                }
            }

            submission.TotalMarks = totalMarks;
            submission.MaxMarks = maxMarks;
            submission.Percentage = maxMarks > 0 ? (totalMarks / maxMarks) * 100 : 0;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var submission = await _context.Submissions.FindAsync(id);
            if (submission == null)
                return false;

            // Remove associated answers first
            var answers = await _context.Answers.Where(a => a.SubmissionId == id).ToListAsync();
            _context.Answers.RemoveRange(answers);

            _context.Submissions.Remove(submission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Submissions.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> HasStudentSubmittedAsync(int assessmentId, int studentId)
        {
            return await _context.Submissions.AnyAsync(s => s.AssessmentId == assessmentId && s.StudentId == studentId);
        }

        public async Task<IEnumerable<SubmissionDto>> GetPendingGradingAsync()
        {
            var submissions = await _context.Submissions
                .Include(s => s.Assessment)
                .Include(s => s.Student)
                .Where(s => !s.GradedAt.HasValue && s.Status == "Submitted")
                .Select(s => new SubmissionDto
                {
                    Id = s.Id,
                    AssessmentId = s.AssessmentId,
                    AssessmentTitle = s.Assessment != null ? s.Assessment.Title : "",
                    StudentId = s.StudentId,
                    StudentName = s.Student != null ? s.Student.FullName : "",
                    StudentEmail = s.Student != null ? s.Student.Email : "",
                    SubmittedAt = s.SubmittedAt,
                    TotalMarks = s.TotalMarks ?? 0,
                    MaxMarks = s.MaxMarks ?? 0,
                    Percentage = s.Percentage ?? 0,
                    Status = s.Status ?? "Draft",
                    Feedback = s.GraderComments,
                    IsGraded = false,
                    GradedAt = s.GradedAt,
                    GradedByUserId = s.GradedBy,
                    GradedByName = "",
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt ?? s.CreatedAt,
                    Answers = new List<AnswerDto>()
                })
                .OrderBy(s => s.SubmittedAt)
                .ToListAsync();

            foreach (var submission in submissions)
            {
                submission.Answers = await GetAnswersForSubmissionAsync(submission.Id);
            }

            return submissions;
        }

        public async Task<SubmissionDto?> AutoGradeAsync(int id)
        {
            var submission = await _context.Submissions.FindAsync(id);
            if (submission == null)
                return null;

            var answers = await _context.Answers
                .Include(a => a.Question)
                .Where(a => a.SubmissionId == id)
                .ToListAsync();

            decimal totalMarks = 0;
            decimal maxMarks = 0;

            foreach (var answer in answers)
            {
                if (answer.Question != null && !string.IsNullOrEmpty(answer.SelectedOptionIds))
                {
                    var selectedIds = JsonSerializer.Deserialize<List<int>>(answer.SelectedOptionIds) ?? new List<int>();
                    var correctOptions = await _context.AnswerOptions
                        .Where(ao => ao.QuestionId == answer.QuestionId && ao.IsCorrect)
                        .Select(ao => ao.Id)
                        .ToListAsync();

                    // Simple auto-grading logic for multiple choice
                    if (selectedIds.OrderBy(x => x).SequenceEqual(correctOptions.OrderBy(x => x)))
                    {
                        answer.MarksAwarded = answer.Question.Marks;
                        answer.IsCorrect = true;
                    }
                    else
                    {
                        answer.MarksAwarded = 0;
                        answer.IsCorrect = false;
                    }

                    answer.MaxMarks = answer.Question.Marks;
                    answer.GradedAt = DateTime.UtcNow;
                    answer.UpdatedAt = DateTime.UtcNow;

                    totalMarks += answer.MarksAwarded ?? 0;
                    maxMarks += answer.MaxMarks ?? 0;
                }
            }

            submission.TotalMarks = totalMarks;
            submission.MaxMarks = maxMarks;
            submission.Percentage = maxMarks > 0 ? (totalMarks / maxMarks) * 100 : 0;
            submission.GradedAt = DateTime.UtcNow;
            submission.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        private async Task<List<AnswerDto>> GetAnswersForSubmissionAsync(int submissionId)
        {
            var answers = await _context.Answers
                .Include(a => a.Question)
                .Where(a => a.SubmissionId == submissionId)
                .Select(a => new AnswerDto
                {
                    Id = a.Id,
                    QuestionId = a.QuestionId,
                    QuestionText = a.Question != null ? a.Question.QuestionText ?? "" : "",
                    AnswerText = a.AnswerText,
                    MarksAwarded = a.MarksAwarded ?? 0,
                    MaxMarks = a.MaxMarks ?? 0,
                    Feedback = a.Feedback,
                    IsCorrect = a.IsCorrect ?? false,
                    AnsweredAt = a.CreatedAt,
                    SelectedOptionIds = new List<int>()
                })
                .ToListAsync();

            // Parse selected option IDs
            foreach (var answer in answers)
            {
                if (!string.IsNullOrEmpty(await _context.Answers
                    .Where(a => a.Id == answer.Id)
                    .Select(a => a.SelectedOptionIds)
                    .FirstOrDefaultAsync()))
                {
                    var answerModel = await _context.Answers.FindAsync(answer.Id);
                    if (answerModel?.SelectedOptionIds != null)
                    {
                        try
                        {
                            answer.SelectedOptionIds = JsonSerializer.Deserialize<List<int>>(answerModel.SelectedOptionIds) ?? new List<int>();
                        }
                        catch
                        {
                            answer.SelectedOptionIds = new List<int>();
                        }
                    }
                }
            }

            return answers;
        }
    }
}