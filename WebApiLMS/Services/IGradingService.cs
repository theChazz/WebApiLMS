using WebApiLMS.DTOs.Grading;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    /// <summary>
    /// Service interface for grading operations and grade management
    /// </summary>
    public interface IGradingService
    {
        // Basic CRUD operations
        Task<IEnumerable<GradingDto>> GetAllGradesAsync();
        Task<GradingDto?> GetGradeByIdAsync(int id);
        Task<GradingDto> CreateGradeAsync(CreateGradingRequest request);
        Task<GradingDto?> UpdateGradeAsync(int id, UpdateGradingRequest request);
        Task<bool> DeleteGradeAsync(int id);
        
        // Grading-specific operations
        Task<IEnumerable<GradingDto>> GetGradesByStudentAsync(int studentId);
        Task<IEnumerable<GradingDto>> GetGradesByAssessmentAsync(int assessmentId);
        Task<GradingDto?> GetGradeBySubmissionAsync(int submissionId);
        Task<IEnumerable<GradeSummaryDto>> GetGradeSummariesAsync(int? studentId = null, int? assessmentId = null);
        
        // Bulk operations
        Task<IEnumerable<GradingDto>> CreateBulkGradesAsync(BulkGradingRequest request);
        Task<bool> PublishGradesAsync(PublishGradesRequest request);
        Task<bool> UnpublishGradesAsync(List<int> gradeIds);
        
        // Grade calculations and adjustments
        Task<GradingDto?> CalculateGradeAsync(int gradeId, int? gradingScaleId = null);
        Task<GradingDto?> ApplyGradeAdjustmentAsync(GradeAdjustmentRequest request);
        Task<IEnumerable<GradingDto>> ApplyGradeCurveAsync(int assessmentId, decimal curvePercentage);
        
        // Statistics and analytics
        Task<object> GetGradeStatisticsAsync(int assessmentId);
        Task<object> GetStudentGradeAnalyticsAsync(int studentId);
        Task<IEnumerable<GradingDto>> GetTopPerformersAsync(int assessmentId, int count = 10);
        
        // Grade history and versioning
        Task<IEnumerable<GradingDto>> GetGradeHistoryAsync(int studentId, int assessmentId);
        Task<GradingDto?> RevertToGradeVersionAsync(int gradeId, int version);
        
        // Import/Export
        Task<IEnumerable<GradingDto>> ImportGradesFromCsvAsync(int assessmentId, Stream csvStream);
        Task<Stream> ExportGradesToCsvAsync(int assessmentId);
        
        // Validation and utilities
        Task<bool> ValidateGradeAsync(CreateGradingRequest request);
        Task<bool> CanGradeAssessmentAsync(int assessmentId, int graderId);
        Task<decimal> CalculateGpaAsync(int studentId, int? termId = null);
    }
}