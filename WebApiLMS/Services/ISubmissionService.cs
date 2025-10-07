using WebApiLMS.DTOs.Submission;

namespace WebApiLMS.Services
{
    public interface ISubmissionService
    {
        Task<IEnumerable<SubmissionDto>> GetAllAsync();
        Task<IEnumerable<SubmissionDto>> GetByAssessmentIdAsync(int assessmentId);
        Task<IEnumerable<SubmissionDto>> GetByStudentIdAsync(int studentId);
        Task<SubmissionDto?> GetByIdAsync(int id);
        Task<SubmissionDto?> GetByAssessmentAndStudentAsync(int assessmentId, int studentId);
        Task<SubmissionDto> CreateAsync(CreateSubmissionRequest request);
        Task<SubmissionDto?> UpdateAsync(int id, UpdateSubmissionRequest request);
        Task<SubmissionDto?> GradeAsync(int id, GradeSubmissionRequest request, int gradedByUserId);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasStudentSubmittedAsync(int assessmentId, int studentId);
        Task<IEnumerable<SubmissionDto>> GetPendingGradingAsync();
        Task<SubmissionDto?> AutoGradeAsync(int id);
    }
}