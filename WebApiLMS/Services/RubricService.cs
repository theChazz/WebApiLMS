using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.DTOs.Rubric;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    /// <summary>
    /// Service implementation for rubric operations and rubric-based assessment
    /// </summary>
    public class RubricService : IRubricService
    {
        private readonly WebApiLMSDbContext _context;

        public RubricService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        #region Basic CRUD Operations for Rubrics

        public async Task<IEnumerable<RubricDto>> GetAllRubricsAsync()
        {
            var rubrics = await _context.Rubrics
                .Include(r => r.Assessment)
                .Include(r => r.Criteria!)
                    .ThenInclude(c => c.Levels)
                .OrderBy(r => r.Name)
                .ToListAsync();

            return rubrics.Select(MapToDto);
        }

        public async Task<RubricDto?> GetRubricByIdAsync(int id)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.Assessment)
                .Include(r => r.Criteria!)
                    .ThenInclude(c => c.Levels)
                .FirstOrDefaultAsync(r => r.Id == id);

            return rubric != null ? MapToDto(rubric) : null;
        }

        public async Task<RubricDto> CreateRubricAsync(CreateRubricRequest request)
        {
            var rubric = new RubricModel
            {
                Name = request.Name,
                Description = request.Description,
                AssessmentId = request.AssessmentId,
                MaxScore = request.MaxScore,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rubrics.Add(rubric);
            await _context.SaveChangesAsync();

            // Create criteria if provided
            if (request.Criteria != null && request.Criteria.Any())
            {
                foreach (var criterionRequest in request.Criteria)
                {
                    await CreateCriterionAsync(rubric.Id, criterionRequest);
                }
            }

            return await GetRubricByIdAsync(rubric.Id) ?? throw new InvalidOperationException("Failed to retrieve created rubric");
        }

        public async Task<RubricDto?> UpdateRubricAsync(int id, UpdateRubricRequest request)
        {
            var rubric = await _context.Rubrics.FindAsync(id);
            if (rubric == null) return null;

            // Update only provided fields
            if (request.Name != null) rubric.Name = request.Name;
            if (request.Description != null) rubric.Description = request.Description;
            if (request.AssessmentId.HasValue) rubric.AssessmentId = request.AssessmentId;
            if (request.MaxScore.HasValue) rubric.MaxScore = request.MaxScore.Value;
            if (request.IsActive.HasValue) rubric.IsActive = request.IsActive.Value;

            rubric.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetRubricByIdAsync(id);
        }

        public async Task<bool> DeleteRubricAsync(int id)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.Criteria!)
                    .ThenInclude(c => c.Levels)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rubric == null) return false;

            // Delete all related levels and criteria
            foreach (var criterion in rubric.Criteria ?? new List<RubricCriterionModel>())
            {
                if (criterion.Levels != null)
                {
                    _context.RubricLevels.RemoveRange(criterion.Levels);
                }
            }

            if (rubric.Criteria != null)
            {
                _context.RubricCriteria.RemoveRange(rubric.Criteria);
            }

            _context.Rubrics.Remove(rubric);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Rubric-Specific Operations

        public async Task<IEnumerable<RubricSummaryDto>> GetRubricSummariesAsync()
        {
            var rubrics = await _context.Rubrics
                .Include(r => r.Assessment)
                .Include(r => r.Criteria)
                .ToListAsync();

            return rubrics.Select(r => new RubricSummaryDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                AssessmentId = r.AssessmentId,
                AssessmentTitle = r.Assessment?.Title,
                MaxScore = r.MaxScore,
                IsActive = r.IsActive,
                CriteriaCount = r.Criteria?.Count ?? 0,
                CreatedAt = r.CreatedAt
            });
        }

        public async Task<IEnumerable<RubricDto>> GetRubricsByAssessmentAsync(int assessmentId)
        {
            var rubrics = await _context.Rubrics
                .Include(r => r.Assessment)
                .Include(r => r.Criteria!)
                    .ThenInclude(c => c.Levels)
                .Where(r => r.AssessmentId == assessmentId)
                .OrderBy(r => r.Name)
                .ToListAsync();

            return rubrics.Select(MapToDto);
        }

        public async Task<RubricDto> CreateRubricFromTemplateAsync(CreateRubricFromTemplateRequest request)
        {
            var rubric = new RubricModel
            {
                Name = request.Name,
                Description = request.Description,
                AssessmentId = request.AssessmentId,
                MaxScore = request.MaxScore,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rubrics.Add(rubric);
            await _context.SaveChangesAsync();

            // Create criteria based on template
            await CreateCriteriaFromTemplate(rubric.Id, request);

            return await GetRubricByIdAsync(rubric.Id) ?? throw new InvalidOperationException("Failed to retrieve created rubric");
        }

        public async Task<RubricDto> CopyRubricAsync(CopyRubricRequest request)
        {
            var sourceRubric = await _context.Rubrics
                .Include(r => r.Criteria!)
                    .ThenInclude(c => c.Levels)
                .FirstOrDefaultAsync(r => r.Id == request.SourceRubricId);

            if (sourceRubric == null)
                throw new ArgumentException("Source rubric not found");

            var newRubric = new RubricModel
            {
                Name = request.NewName,
                Description = request.NewDescription ?? sourceRubric.Description,
                AssessmentId = request.NewAssessmentId,
                MaxScore = sourceRubric.MaxScore,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rubrics.Add(newRubric);
            await _context.SaveChangesAsync();

            // Copy criteria and levels
            if (sourceRubric.Criteria != null)
            {
                foreach (var sourceCriterion in sourceRubric.Criteria)
                {
                    var newCriterion = new RubricCriterionModel
                    {
                        RubricId = newRubric.Id,
                        Name = sourceCriterion.Name,
                        Description = sourceCriterion.Description,
                        MaxScore = sourceCriterion.MaxScore,
                        Weight = sourceCriterion.Weight,
                        SortOrder = sourceCriterion.SortOrder,
                        LearningOutcomeId = sourceCriterion.LearningOutcomeId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.RubricCriteria.Add(newCriterion);
                    await _context.SaveChangesAsync();

                    // Copy levels
                    if (sourceCriterion.Levels != null)
                    {
                        foreach (var sourceLevel in sourceCriterion.Levels)
                        {
                            var newLevel = new RubricLevelModel
                            {
                                CriterionId = newCriterion.Id,
                                Name = sourceLevel.Name,
                                Description = sourceLevel.Description,
                                Score = sourceLevel.Score,
                                SortOrder = sourceLevel.SortOrder,
                                CreatedAt = DateTime.UtcNow
                            };

                            _context.RubricLevels.Add(newLevel);
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            return await GetRubricByIdAsync(newRubric.Id) ?? throw new InvalidOperationException("Failed to retrieve copied rubric");
        }

        #endregion

        #region Criterion Operations

        public async Task<RubricCriterionDto?> GetCriterionByIdAsync(int criterionId)
        {
            var criterion = await _context.RubricCriteria
                .Include(c => c.Rubric)
                .Include(c => c.Levels)
                .FirstOrDefaultAsync(c => c.Id == criterionId);

            return criterion != null ? MapCriterionToDto(criterion) : null;
        }

        public async Task<RubricCriterionDto> CreateCriterionAsync(int rubricId, CreateRubricCriterionRequest request)
        {
            var criterion = new RubricCriterionModel
            {
                RubricId = rubricId,
                Name = request.Name,
                Description = request.Description,
                MaxScore = request.MaxScore,
                Weight = request.Weight,
                SortOrder = request.SortOrder,
                LearningOutcomeId = request.LearningOutcomeId,
                CreatedAt = DateTime.UtcNow
            };

            _context.RubricCriteria.Add(criterion);
            await _context.SaveChangesAsync();

            // Create levels if provided
            if (request.Levels != null && request.Levels.Any())
            {
                foreach (var levelRequest in request.Levels)
                {
                    await CreateLevelAsync(criterion.Id, levelRequest);
                }
            }

            return await GetCriterionByIdAsync(criterion.Id) ?? throw new InvalidOperationException("Failed to retrieve created criterion");
        }

        public async Task<RubricCriterionDto?> UpdateCriterionAsync(int criterionId, UpdateRubricCriterionRequest request)
        {
            var criterion = await _context.RubricCriteria.FindAsync(criterionId);
            if (criterion == null) return null;

            // Update only provided fields
            if (request.Name != null) criterion.Name = request.Name;
            if (request.Description != null) criterion.Description = request.Description;
            if (request.MaxScore.HasValue) criterion.MaxScore = request.MaxScore.Value;
            if (request.Weight.HasValue) criterion.Weight = request.Weight.Value;
            if (request.SortOrder.HasValue) criterion.SortOrder = request.SortOrder.Value;
            if (request.LearningOutcomeId != null) criterion.LearningOutcomeId = request.LearningOutcomeId;

            criterion.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetCriterionByIdAsync(criterionId);
        }

        public async Task<bool> DeleteCriterionAsync(int criterionId)
        {
            var criterion = await _context.RubricCriteria
                .Include(c => c.Levels)
                .FirstOrDefaultAsync(c => c.Id == criterionId);

            if (criterion == null) return false;

            // Delete all levels first
            if (criterion.Levels != null)
            {
                _context.RubricLevels.RemoveRange(criterion.Levels);
            }

            _context.RubricCriteria.Remove(criterion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReorderCriteriaAsync(ReorderCriteriaRequest request)
        {
            var criteria = await _context.RubricCriteria
                .Where(c => c.RubricId == request.RubricId)
                .ToListAsync();

            foreach (var orderItem in request.CriteriaOrder)
            {
                var criterion = criteria.FirstOrDefault(c => c.Id == orderItem.CriterionId);
                if (criterion != null)
                {
                    criterion.SortOrder = orderItem.SortOrder;
                    criterion.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RubricCriterionDto>> BulkUpdateCriteriaAsync(BulkUpdateCriteriaRequest request)
        {
            var existingCriteria = await _context.RubricCriteria
                .Include(c => c.Levels)
                .Where(c => c.RubricId == request.RubricId)
                .ToListAsync();

            var updatedCriteria = new List<RubricCriterionModel>();

            foreach (var criterionItem in request.Criteria)
            {
                if (criterionItem.IsDeleted && criterionItem.Id.HasValue)
                {
                    // Delete criterion
                    var toDelete = existingCriteria.FirstOrDefault(c => c.Id == criterionItem.Id);
                    if (toDelete != null)
                    {
                        if (toDelete.Levels != null) _context.RubricLevels.RemoveRange(toDelete.Levels);
                        _context.RubricCriteria.Remove(toDelete);
                    }
                    continue;
                }

                RubricCriterionModel criterion;

                if (criterionItem.Id.HasValue)
                {
                    // Update existing criterion
                    criterion = existingCriteria.FirstOrDefault(c => c.Id == criterionItem.Id);
                    if (criterion == null) continue;

                    criterion.Name = criterionItem.Name;
                    criterion.Description = criterionItem.Description;
                    criterion.MaxScore = criterionItem.MaxScore;
                    criterion.Weight = criterionItem.Weight;
                    criterion.SortOrder = criterionItem.SortOrder;
                    criterion.LearningOutcomeId = criterionItem.LearningOutcomeId;
                    criterion.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    // Create new criterion
                    criterion = new RubricCriterionModel
                    {
                        RubricId = request.RubricId,
                        Name = criterionItem.Name,
                        Description = criterionItem.Description,
                        MaxScore = criterionItem.MaxScore,
                        Weight = criterionItem.Weight,
                        SortOrder = criterionItem.SortOrder,
                        LearningOutcomeId = criterionItem.LearningOutcomeId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.RubricCriteria.Add(criterion);
                }

                updatedCriteria.Add(criterion);

                // Handle levels if provided
                if (criterionItem.Levels != null)
                {
                    await BulkUpdateLevelsForCriterion(criterion, criterionItem.Levels);
                }
            }

            await _context.SaveChangesAsync();

            // Return updated criteria
            var resultCriteria = await _context.RubricCriteria
                .Include(c => c.Levels)
                .Where(c => c.RubricId == request.RubricId)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            return resultCriteria.Select(MapCriterionToDto);
        }

        #endregion

        #region Level Operations

        public async Task<RubricLevelDto?> GetLevelByIdAsync(int levelId)
        {
            var level = await _context.RubricLevels
                .Include(l => l.Criterion)
                .FirstOrDefaultAsync(l => l.Id == levelId);

            return level != null ? MapLevelToDto(level) : null;
        }

        public async Task<RubricLevelDto> CreateLevelAsync(int criterionId, CreateRubricLevelRequest request)
        {
            var level = new RubricLevelModel
            {
                CriterionId = criterionId,
                Name = request.Name,
                Description = request.Description,
                Score = request.Score,
                SortOrder = request.SortOrder,
                CreatedAt = DateTime.UtcNow
            };

            _context.RubricLevels.Add(level);
            await _context.SaveChangesAsync();

            return await GetLevelByIdAsync(level.Id) ?? throw new InvalidOperationException("Failed to retrieve created level");
        }

        public async Task<RubricLevelDto?> UpdateLevelAsync(int levelId, UpdateRubricLevelRequest request)
        {
            var level = await _context.RubricLevels.FindAsync(levelId);
            if (level == null) return null;

            // Update only provided fields
            if (request.Name != null) level.Name = request.Name;
            if (request.Description != null) level.Description = request.Description;
            if (request.Score.HasValue) level.Score = request.Score.Value;
            if (request.SortOrder.HasValue) level.SortOrder = request.SortOrder.Value;

            level.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetLevelByIdAsync(levelId);
        }

        public async Task<bool> DeleteLevelAsync(int levelId)
        {
            var level = await _context.RubricLevels.FindAsync(levelId);
            if (level == null) return false;

            _context.RubricLevels.Remove(level);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Rubric Evaluation and Scoring (Simplified Implementation)

        public async Task<RubricEvaluationDto> EvaluateWithRubricAsync(RubricEvaluationRequest request)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.Criteria!)
                    .ThenInclude(c => c.Levels)
                .FirstOrDefaultAsync(r => r.Id == request.RubricId);

            if (rubric == null)
                throw new ArgumentException("Rubric not found");

            var student = await _context.Users.FindAsync(request.StudentId);
            if (student == null)
                throw new ArgumentException("Student not found");

            // Calculate scores for each criterion
            var criterionEvaluations = new List<CriterionEvaluationDto>();
            decimal totalScore = 0;
            decimal maxPossibleScore = 0;

            foreach (var criterionScore in request.CriterionScores)
            {
                var criterion = rubric.Criteria?.FirstOrDefault(c => c.Id == criterionScore.CriterionId);
                if (criterion == null) continue;

                decimal score = 0;
                string? selectedLevelName = null;

                if (criterionScore.IsExempt)
                {
                    // Exempt criteria don't contribute to score
                    score = 0;
                }
                else if (criterionScore.CustomScore.HasValue)
                {
                    score = criterionScore.CustomScore.Value;
                }
                else if (criterionScore.SelectedLevelId.HasValue)
                {
                    var selectedLevel = criterion.Levels?.FirstOrDefault(l => l.Id == criterionScore.SelectedLevelId);
                    if (selectedLevel != null)
                    {
                        score = selectedLevel.Score;
                        selectedLevelName = selectedLevel.Name;
                    }
                }

                var weightedScore = score * criterion.Weight;

                criterionEvaluations.Add(new CriterionEvaluationDto
                {
                    CriterionId = criterion.Id,
                    CriterionName = criterion.Name,
                    MaxScore = criterion.MaxScore,
                    Weight = criterion.Weight,
                    SelectedLevelId = criterionScore.SelectedLevelId,
                    SelectedLevelName = selectedLevelName,
                    Score = score,
                    WeightedScore = weightedScore,
                    Comments = criterionScore.Comments,
                    IsExempt = criterionScore.IsExempt
                });

                if (!criterionScore.IsExempt)
                {
                    totalScore += weightedScore;
                    maxPossibleScore += criterion.MaxScore * criterion.Weight;
                }
            }

            var percentage = maxPossibleScore > 0 ? (totalScore / maxPossibleScore) * 100 : 0;

            return new RubricEvaluationDto
            {
                RubricId = request.RubricId,
                RubricName = rubric.Name,
                StudentId = request.StudentId,
                StudentName = student.FullName,
                SubmissionId = request.SubmissionId,
                CriterionEvaluations = criterionEvaluations,
                TotalScore = totalScore,
                MaxPossibleScore = maxPossibleScore,
                Percentage = percentage,
                OverallComments = request.OverallComments,
                EvaluatedAt = DateTime.UtcNow
            };
        }

        public async Task<RubricEvaluationDto?> GetRubricEvaluationAsync(int rubricId, int studentId, int? submissionId = null)
        {
            // This would typically retrieve saved evaluations from a dedicated table
            // For now, return null as this is a simplified implementation
            await Task.CompletedTask;
            return null;
        }

        public async Task<IEnumerable<RubricEvaluationDto>> GetRubricEvaluationsByAssessmentAsync(int assessmentId)
        {
            // This would typically retrieve saved evaluations for all students in an assessment
            // For now, return empty list as this is a simplified implementation
            await Task.CompletedTask;
            return new List<RubricEvaluationDto>();
        }

        public async Task<RubricEvaluationDto?> UpdateRubricEvaluationAsync(int evaluationId, RubricEvaluationRequest request)
        {
            // This would typically update a saved evaluation
            // For now, just return a new evaluation as this is a simplified implementation
            return await EvaluateWithRubricAsync(request);
        }

        #endregion

        #region Analytics and Statistics

        public async Task<object> GetRubricStatisticsAsync(int rubricId)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.Criteria)
                .FirstOrDefaultAsync(r => r.Id == rubricId);

            if (rubric == null)
                return new { Message = "Rubric not found" };

            return new
            {
                RubricId = rubricId,
                Name = rubric.Name,
                CriteriaCount = rubric.Criteria?.Count ?? 0,
                MaxScore = rubric.MaxScore,
                IsActive = rubric.IsActive,
                CreatedAt = rubric.CreatedAt,
                // Additional statistics would be calculated from evaluation data
                EvaluationsCount = 0, // Placeholder
                AverageScore = 0.0m, // Placeholder
                UsageCount = 0 // Placeholder
            };
        }

        public async Task<object> GetCriterionAnalyticsAsync(int criterionId)
        {
            var criterion = await _context.RubricCriteria
                .Include(c => c.Rubric)
                .Include(c => c.Levels)
                .FirstOrDefaultAsync(c => c.Id == criterionId);

            if (criterion == null)
                return new { Message = "Criterion not found" };

            return new
            {
                CriterionId = criterionId,
                Name = criterion.Name,
                RubricName = criterion.Rubric?.Name,
                MaxScore = criterion.MaxScore,
                Weight = criterion.Weight,
                LevelsCount = criterion.Levels?.Count ?? 0,
                // Additional analytics would be calculated from evaluation data
                AverageScore = 0.0m, // Placeholder
                MostSelectedLevel = "N/A", // Placeholder
                DifficultyIndex = 0.0m // Placeholder
            };
        }

        public async Task<object> GetRubricUsageReportAsync(int rubricId)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.Assessment)
                .FirstOrDefaultAsync(r => r.Id == rubricId);

            if (rubric == null)
                return new { Message = "Rubric not found" };

            return new
            {
                RubricId = rubricId,
                Name = rubric.Name,
                AssessmentTitle = rubric.Assessment?.Title,
                CreatedAt = rubric.CreatedAt,
                LastUsed = (DateTime?)null, // Placeholder
                TotalEvaluations = 0, // Placeholder
                UniqueStudents = 0, // Placeholder
                AverageEvaluationTime = 0.0, // Placeholder
                UsageByMonth = new List<object>() // Placeholder
            };
        }

        #endregion

        #region Validation and Utilities

        public async Task<bool> ValidateRubricAsync(int rubricId)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.Criteria!)
                    .ThenInclude(c => c.Levels)
                .FirstOrDefaultAsync(r => r.Id == rubricId);

            if (rubric == null) return false;

            // Validation rules
            if (string.IsNullOrWhiteSpace(rubric.Name)) return false;
            if (rubric.MaxScore <= 0) return false;
            if (rubric.Criteria == null || !rubric.Criteria.Any()) return false;

            foreach (var criterion in rubric.Criteria)
            {
                if (string.IsNullOrWhiteSpace(criterion.Name)) return false;
                if (criterion.MaxScore <= 0) return false;
                if (criterion.Weight <= 0) return false;
                if (criterion.Levels == null || !criterion.Levels.Any()) return false;

                foreach (var level in criterion.Levels)
                {
                    if (string.IsNullOrWhiteSpace(level.Name)) return false;
                    if (level.Score < 0 || level.Score > criterion.MaxScore) return false;
                }
            }

            return true;
        }

        public async Task<bool> CanUseRubricAsync(int rubricId, int userId)
        {
            var rubric = await _context.Rubrics.FindAsync(rubricId);
            if (rubric == null || !rubric.IsActive) return false;

            // Add authorization logic here (e.g., check if user is instructor for the assessment)
            // For now, return true for active rubrics
            return true;
        }

        public async Task<decimal> CalculateRubricScoreAsync(int rubricId, List<CriterionScoreRequest> criterionScores)
        {
            var rubric = await _context.Rubrics
                .Include(r => r.Criteria!)
                    .ThenInclude(c => c.Levels)
                .FirstOrDefaultAsync(r => r.Id == rubricId);

            if (rubric == null) return 0;

            decimal totalScore = 0;

            foreach (var criterionScore in criterionScores)
            {
                var criterion = rubric.Criteria?.FirstOrDefault(c => c.Id == criterionScore.CriterionId);
                if (criterion == null || criterionScore.IsExempt) continue;

                decimal score = 0;

                if (criterionScore.CustomScore.HasValue)
                {
                    score = criterionScore.CustomScore.Value;
                }
                else if (criterionScore.SelectedLevelId.HasValue)
                {
                    var selectedLevel = criterion.Levels?.FirstOrDefault(l => l.Id == criterionScore.SelectedLevelId);
                    if (selectedLevel != null)
                    {
                        score = selectedLevel.Score;
                    }
                }

                totalScore += score * criterion.Weight;
            }

            return totalScore;
        }

        #endregion

        #region Import/Export (Simplified Implementation)

        public async Task<RubricDto> ImportRubricAsync(Stream jsonStream)
        {
            // This would implement JSON import functionality
            await Task.CompletedTask;
            throw new NotImplementedException("Rubric import functionality needs to be implemented");
        }

        public async Task<Stream> ExportRubricAsync(int rubricId)
        {
            // This would implement JSON export functionality
            await Task.CompletedTask;
            throw new NotImplementedException("Rubric export functionality needs to be implemented");
        }

        public async Task<Stream> ExportRubricEvaluationsAsync(int rubricId)
        {
            // This would implement evaluation export functionality
            await Task.CompletedTask;
            throw new NotImplementedException("Rubric evaluation export functionality needs to be implemented");
        }

        #endregion

        #region Private Helper Methods

        private RubricDto MapToDto(RubricModel rubric)
        {
            return new RubricDto
            {
                Id = rubric.Id,
                Name = rubric.Name,
                Description = rubric.Description,
                AssessmentId = rubric.AssessmentId,
                AssessmentTitle = rubric.Assessment?.Title,
                MaxScore = rubric.MaxScore,
                IsActive = rubric.IsActive,
                CreatedAt = rubric.CreatedAt,
                UpdatedAt = rubric.UpdatedAt,
                Criteria = rubric.Criteria?.OrderBy(c => c.SortOrder).Select(MapCriterionToDto).ToList()
            };
        }

        private RubricCriterionDto MapCriterionToDto(RubricCriterionModel criterion)
        {
            return new RubricCriterionDto
            {
                Id = criterion.Id,
                RubricId = criterion.RubricId,
                Name = criterion.Name,
                Description = criterion.Description,
                MaxScore = criterion.MaxScore,
                Weight = criterion.Weight,
                SortOrder = criterion.SortOrder,
                LearningOutcomeId = criterion.LearningOutcomeId,
                CreatedAt = criterion.CreatedAt,
                UpdatedAt = criterion.UpdatedAt,
                Levels = criterion.Levels?.OrderBy(l => l.SortOrder).Select(MapLevelToDto).ToList()
            };
        }

        private RubricLevelDto MapLevelToDto(RubricLevelModel level)
        {
            return new RubricLevelDto
            {
                Id = level.Id,
                CriterionId = level.CriterionId,
                Name = level.Name,
                Description = level.Description,
                Score = level.Score,
                SortOrder = level.SortOrder,
                CreatedAt = level.CreatedAt,
                UpdatedAt = level.UpdatedAt
            };
        }

        private async Task CreateCriteriaFromTemplate(int rubricId, CreateRubricFromTemplateRequest request)
        {
            var criterionNames = request.CriterionNames ?? GetDefaultCriterionNames(request.TemplateType!);
            var levelNames = request.LevelNames ?? GetDefaultLevelNames(request.TemplateType!, request.NumberOfLevels);

            for (int i = 0; i < criterionNames.Count; i++)
            {
                var criterion = new RubricCriterionModel
                {
                    RubricId = rubricId,
                    Name = criterionNames[i],
                    Description = $"Evaluation criterion: {criterionNames[i]}",
                    MaxScore = request.MaxScore / criterionNames.Count,
                    Weight = 1.0m,
                    SortOrder = i + 1,
                    CreatedAt = DateTime.UtcNow
                };

                _context.RubricCriteria.Add(criterion);
                await _context.SaveChangesAsync();

                // Create levels for this criterion
                for (int j = 0; j < levelNames.Count; j++)
                {
                    var scorePercentage = (decimal)(j + 1) / levelNames.Count;
                    var level = new RubricLevelModel
                    {
                        CriterionId = criterion.Id,
                        Name = levelNames[j],
                        Description = $"{levelNames[j]} performance level",
                        Score = criterion.MaxScore * scorePercentage,
                        SortOrder = j + 1,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.RubricLevels.Add(level);
                }
            }

            await _context.SaveChangesAsync();
        }

        private static List<string> GetDefaultCriterionNames(string templateType)
        {
            return templateType.ToLower() switch
            {
                "standard" => new List<string> { "Content", "Organization", "Language Use", "Mechanics" },
                "holistic" => new List<string> { "Overall Performance" },
                "analytic" => new List<string> { "Knowledge", "Comprehension", "Application", "Analysis", "Synthesis", "Evaluation" },
                "singlepoint" => new List<string> { "Criteria 1", "Criteria 2", "Criteria 3" },
                _ => new List<string> { "Criterion 1", "Criterion 2", "Criterion 3" }
            };
        }

        private static List<string> GetDefaultLevelNames(string templateType, int numberOfLevels)
        {
            return numberOfLevels switch
            {
                2 => new List<string> { "Needs Improvement", "Proficient" },
                3 => new List<string> { "Beginning", "Developing", "Proficient" },
                4 => new List<string> { "Beginning", "Developing", "Proficient", "Advanced" },
                5 => new List<string> { "Inadequate", "Developing", "Satisfactory", "Good", "Excellent" },
                _ => Enumerable.Range(1, numberOfLevels).Select(i => $"Level {i}").ToList()
            };
        }

        private async Task BulkUpdateLevelsForCriterion(RubricCriterionModel criterion, List<LevelUpdateItem> levelItems)
        {
            var existingLevels = await _context.RubricLevels
                .Where(l => l.CriterionId == criterion.Id)
                .ToListAsync();

            foreach (var levelItem in levelItems)
            {
                if (levelItem.IsDeleted && levelItem.Id.HasValue)
                {
                    // Delete level
                    var toDelete = existingLevels.FirstOrDefault(l => l.Id == levelItem.Id);
                    if (toDelete != null)
                    {
                        _context.RubricLevels.Remove(toDelete);
                    }
                    continue;
                }

                if (levelItem.Id.HasValue)
                {
                    // Update existing level
                    var level = existingLevels.FirstOrDefault(l => l.Id == levelItem.Id);
                    if (level != null)
                    {
                        level.Name = levelItem.Name;
                        level.Description = levelItem.Description;
                        level.Score = levelItem.Score;
                        level.SortOrder = levelItem.SortOrder;
                        level.UpdatedAt = DateTime.UtcNow;
                    }
                }
                else
                {
                    // Create new level
                    var newLevel = new RubricLevelModel
                    {
                        CriterionId = criterion.Id,
                        Name = levelItem.Name,
                        Description = levelItem.Description,
                        Score = levelItem.Score,
                        SortOrder = levelItem.SortOrder,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.RubricLevels.Add(newLevel);
                }
            }
        }

        #endregion
    }
}