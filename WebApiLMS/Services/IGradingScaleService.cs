using WebApiLMS.DTOs.GradingScale;

namespace WebApiLMS.Services
{
    /// <summary>
    /// Service interface for grading scale operations and grade boundary management
    /// </summary>
    public interface IGradingScaleService
    {
        // Basic CRUD operations for Grading Scales
        Task<IEnumerable<GradingScaleDto>> GetAllGradingScalesAsync();
        Task<GradingScaleDto?> GetGradingScaleByIdAsync(int id);
        Task<GradingScaleDto> CreateGradingScaleAsync(CreateGradingScaleRequest request);
        Task<GradingScaleDto?> UpdateGradingScaleAsync(int id, UpdateGradingScaleRequest request);
        Task<bool> DeleteGradingScaleAsync(int id);
        
        // Grading scale-specific operations
        Task<IEnumerable<GradingScaleSummaryDto>> GetGradingScaleSummariesAsync();
        Task<GradingScaleDto?> GetDefaultGradingScaleAsync();
        Task<bool> SetDefaultGradingScaleAsync(SetDefaultGradingScaleRequest request);
        Task<GradingScaleDto> CreateGradingScaleFromTemplateAsync(CreateGradingScaleFromTemplateRequest request);
        Task<GradingScaleDto> CopyGradingScaleAsync(CopyGradingScaleRequest request);
        
        // Grade boundary operations
        Task<GradeBoundaryDto?> GetGradeBoundaryByIdAsync(int boundaryId);
        Task<IEnumerable<GradeBoundaryDto>> GetGradeBoundariesAsync(int gradingScaleId);
        Task<GradeBoundaryDto> CreateGradeBoundaryAsync(int gradingScaleId, CreateGradeBoundaryRequest request);
        Task<GradeBoundaryDto?> UpdateGradeBoundaryAsync(int boundaryId, UpdateGradeBoundaryRequest request);
        Task<bool> DeleteGradeBoundaryAsync(int boundaryId);
        Task<IEnumerable<GradeBoundaryDto>> BulkCreateGradeBoundariesAsync(BulkCreateGradeBoundariesRequest request);
        Task<IEnumerable<GradeBoundaryDto>> BulkUpdateGradeBoundariesAsync(BulkUpdateGradeBoundariesRequest request);
        Task<bool> ReorderGradeBoundariesAsync(ReorderGradeBoundariesRequest request);
        
        // Grade calculations
        Task<GradeCalculationDto> CalculateGradeAsync(CalculateGradeRequest request);
        Task<IEnumerable<GradeCalculationDto>> BatchCalculateGradesAsync(BatchCalculateGradesRequest request);
        Task<GradeBoundaryDto?> FindGradeBoundaryForPercentageAsync(int gradingScaleId, decimal percentage);
        
        // Scale adjustments and transformations
        Task<GradingScaleDto> AdjustGradeBoundariesAsync(AdjustGradeBoundariesRequest request);
        Task<GradingScaleDto> NormalizeGradeBoundariesAsync(int gradingScaleId);
        Task<bool> ValidateGradeBoundariesAsync(int gradingScaleId);
        
        // Statistics and analytics
        Task<GradingScaleStatsDto> GetGradingScaleStatisticsAsync(int gradingScaleId);
        Task<object> GetGradeDistributionAsync(int gradingScaleId, int? assessmentId = null);
        Task<object> GetGradingScaleUsageReportAsync(int gradingScaleId);
        Task<object> CompareGradingScalesAsync(List<int> gradingScaleIds);
        
        // Import/Export and templates
        Task<GradingScaleDto> ImportGradingScaleAsync(Stream jsonStream);
        Task<Stream> ExportGradingScaleAsync(int gradingScaleId);
        Task<IEnumerable<object>> GetAvailableTemplatesAsync();
        
        // Validation and utilities
        Task<bool> ValidateGradingScaleAsync(int gradingScaleId);
        Task<bool> IsGradingScaleInUseAsync(int gradingScaleId);
        Task<IEnumerable<object>> GetGradingScaleConflictsAsync(int gradingScaleId);
        Task<decimal> CalculateGpaAsync(List<GradeCalculationDto> grades);
    }
}