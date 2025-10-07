using WebApiLMS.Models;
using WebApiLMS.DTOs.Assessment;

namespace WebApiLMS.Services
{
    /// <summary>
    /// Interface for Assessment service operations
    /// </summary>
    public interface IAssessmentService
    {
        Task<List<AssessmentModel>> GetAllAsync();
        Task<List<AssessmentModel>> GetByCourseIdAsync(int courseId);
        Task<AssessmentModel?> GetByIdAsync(int id);
        Task<AssessmentModel> CreateAsync(CreateAssessmentRequest request);
        Task<bool> UpdateAsync(int id, UpdateAssessmentRequest request);
        Task<bool> DeleteAsync(int id);
        Task<bool> PublishAsync(int id, int publishedBy);
        Task<AssessmentStatisticsDto> GetStatisticsAsync(int id);
    }
}