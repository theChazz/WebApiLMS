using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.DTOs.Question;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly WebApiLMSDbContext _context;

        public QuestionService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuestionDto>> GetAllAsync()
        {
            var questions = await _context.Questions
                .Include(q => q.Assessment)
                .Include(q => q.QuestionType)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    AssessmentId = q.AssessmentId,
                    AssessmentTitle = q.Assessment != null ? q.Assessment.Title : "",
                    Type = q.QuestionType != null ? q.QuestionType.Name ?? "" : "",
                    QuestionText = q.QuestionText ?? "",
                    MaxMarks = q.Marks,
                    DisplayOrder = q.SortOrder,
                    IsRequired = true, // Default for now
                    Difficulty = "Medium", // Default for now
                    HelpText = q.Instructions,
                    CorrectAnswer = "", // Will need to be computed from answer options
                    CreatedAt = q.CreatedAt,
                    UpdatedAt = q.UpdatedAt ?? q.CreatedAt,
                    AnswerOptions = new List<AnswerOptionDto>()
                })
                .OrderBy(q => q.DisplayOrder)
                .ToListAsync();

            // Get answer options separately
            foreach (var question in questions)
            {
                var answerOptions = await _context.AnswerOptions
                    .Where(ao => ao.QuestionId == question.Id)
                    .Select(ao => new AnswerOptionDto
                    {
                        Id = ao.Id,
                        OptionText = ao.OptionText ?? "",
                        IsCorrect = ao.IsCorrect,
                        Points = ao.Points ?? 0,
                        Explanation = ao.Explanation,
                        DisplayOrder = ao.SortOrder
                    })
                    .OrderBy(ao => ao.DisplayOrder)
                    .ToListAsync();

                question.AnswerOptions = answerOptions;
            }

            return questions;
        }

        public async Task<IEnumerable<QuestionDto>> GetByAssessmentIdAsync(int assessmentId)
        {
            var questions = await _context.Questions
                .Include(q => q.Assessment)
                .Include(q => q.QuestionType)
                .Where(q => q.AssessmentId == assessmentId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    AssessmentId = q.AssessmentId,
                    AssessmentTitle = q.Assessment != null ? q.Assessment.Title : "",
                    Type = q.QuestionType != null ? q.QuestionType.Name ?? "" : "",
                    QuestionText = q.QuestionText ?? "",
                    MaxMarks = q.Marks,
                    DisplayOrder = q.SortOrder,
                    IsRequired = true, // Default for now
                    Difficulty = "Medium", // Default for now
                    HelpText = q.Instructions,
                    CorrectAnswer = "", // Will need to be computed from answer options
                    CreatedAt = q.CreatedAt,
                    UpdatedAt = q.UpdatedAt ?? q.CreatedAt,
                    AnswerOptions = new List<AnswerOptionDto>()
                })
                .OrderBy(q => q.DisplayOrder)
                .ToListAsync();

            // Get answer options separately
            foreach (var question in questions)
            {
                var answerOptions = await _context.AnswerOptions
                    .Where(ao => ao.QuestionId == question.Id)
                    .Select(ao => new AnswerOptionDto
                    {
                        Id = ao.Id,
                        OptionText = ao.OptionText ?? "",
                        IsCorrect = ao.IsCorrect,
                        Points = ao.Points ?? 0,
                        Explanation = ao.Explanation,
                        DisplayOrder = ao.SortOrder
                    })
                    .OrderBy(ao => ao.DisplayOrder)
                    .ToListAsync();

                question.AnswerOptions = answerOptions;
            }

            return questions;
        }

        public async Task<QuestionDto?> GetByIdAsync(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Assessment)
                .Include(q => q.QuestionType)
                .Where(q => q.Id == id)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    AssessmentId = q.AssessmentId,
                    AssessmentTitle = q.Assessment != null ? q.Assessment.Title : "",
                    Type = q.QuestionType != null ? q.QuestionType.Name ?? "" : "",
                    QuestionText = q.QuestionText ?? "",
                    MaxMarks = q.Marks,
                    DisplayOrder = q.SortOrder,
                    IsRequired = true, // Default for now
                    Difficulty = "Medium", // Default for now
                    HelpText = q.Instructions,
                    CorrectAnswer = "", // Will need to be computed from answer options
                    CreatedAt = q.CreatedAt,
                    UpdatedAt = q.UpdatedAt ?? q.CreatedAt,
                    AnswerOptions = new List<AnswerOptionDto>()
                })
                .FirstOrDefaultAsync();

            if (question != null)
            {
                var answerOptions = await _context.AnswerOptions
                    .Where(ao => ao.QuestionId == question.Id)
                    .Select(ao => new AnswerOptionDto
                    {
                        Id = ao.Id,
                        OptionText = ao.OptionText ?? "",
                        IsCorrect = ao.IsCorrect,
                        Points = ao.Points ?? 0,
                        Explanation = ao.Explanation,
                        DisplayOrder = ao.SortOrder
                    })
                    .OrderBy(ao => ao.DisplayOrder)
                    .ToListAsync();

                question.AnswerOptions = answerOptions;
            }

            return question;
        }

        public async Task<QuestionDto> CreateAsync(CreateQuestionRequest request)
        {
            // For now, we'll use the first question type available
            var firstQuestionType = await _context.QuestionTypes.FirstOrDefaultAsync();
            if (firstQuestionType == null)
            {
                throw new InvalidOperationException("No question types available");
            }

            var question = new QuestionModel
            {
                AssessmentId = request.AssessmentId,
                QuestionTypeId = firstQuestionType.Id,
                QuestionText = request.QuestionText,
                Marks = (int)request.MaxMarks,
                SortOrder = request.DisplayOrder,
                Instructions = request.HelpText,
                LearningOutcomeId = "", // Default for now
                ReferenceText = request.CorrectAnswer,
                MediaUrls = "", // Default for now
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            // Create answer options if provided
            if (request.AnswerOptions.Any())
            {
                var answerOptions = request.AnswerOptions.Select(ao => new AnswerOptionModel
                {
                    QuestionId = question.Id,
                    OptionText = ao.OptionText,
                    IsCorrect = ao.IsCorrect,
                    Points = ao.Points,
                    Explanation = ao.Explanation,
                    SortOrder = ao.DisplayOrder,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                _context.AnswerOptions.AddRange(answerOptions);
                await _context.SaveChangesAsync();
            }

            return await GetByIdAsync(question.Id) ?? throw new InvalidOperationException("Failed to create question");
        }

        public async Task<QuestionDto?> UpdateAsync(int id, UpdateQuestionRequest request)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return null;

            question.QuestionText = request.QuestionText;
            question.Marks = (int)request.MaxMarks;
            question.SortOrder = request.DisplayOrder;
            question.Instructions = request.HelpText;
            question.ReferenceText = request.CorrectAnswer;
            question.UpdatedAt = DateTime.UtcNow;

            // Handle answer options updates
            var existingOptions = await _context.AnswerOptions
                .Where(ao => ao.QuestionId == id)
                .ToListAsync();

            var updatedOptionIds = request.AnswerOptions.Where(ao => ao.Id.HasValue).Select(ao => ao.Id!.Value).ToList();

            // Remove deleted options
            var optionsToRemove = existingOptions.Where(ao => !updatedOptionIds.Contains(ao.Id)).ToList();
            _context.AnswerOptions.RemoveRange(optionsToRemove);

            // Update existing options
            foreach (var optionRequest in request.AnswerOptions.Where(ao => ao.Id.HasValue))
            {
                var existingOption = existingOptions.FirstOrDefault(ao => ao.Id == optionRequest.Id);
                if (existingOption != null)
                {
                    existingOption.OptionText = optionRequest.OptionText;
                    existingOption.IsCorrect = optionRequest.IsCorrect;
                    existingOption.Points = optionRequest.Points;
                    existingOption.Explanation = optionRequest.Explanation;
                    existingOption.SortOrder = optionRequest.DisplayOrder;
                    existingOption.UpdatedAt = DateTime.UtcNow;
                }
            }

            // Add new options
            var newOptions = request.AnswerOptions.Where(ao => !ao.Id.HasValue).Select(ao => new AnswerOptionModel
            {
                QuestionId = question.Id,
                OptionText = ao.OptionText,
                IsCorrect = ao.IsCorrect,
                Points = ao.Points,
                Explanation = ao.Explanation,
                SortOrder = ao.DisplayOrder,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            _context.AnswerOptions.AddRange(newOptions);

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return false;

            // Remove associated answer options first
            var answerOptions = await _context.AnswerOptions.Where(ao => ao.QuestionId == id).ToListAsync();
            _context.AnswerOptions.RemoveRange(answerOptions);

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Questions.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> ReorderQuestionsAsync(int assessmentId, Dictionary<int, int> questionOrders)
        {
            var questions = await _context.Questions
                .Where(q => q.AssessmentId == assessmentId && questionOrders.Keys.Contains(q.Id))
                .ToListAsync();

            if (questions.Count != questionOrders.Count)
                return false;

            foreach (var question in questions)
            {
                if (questionOrders.TryGetValue(question.Id, out int newOrder))
                {
                    question.SortOrder = newOrder;
                    question.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}