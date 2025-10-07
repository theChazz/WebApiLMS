using WebApiLMS.DTOs.Question;

namespace WebApiLMS.Services
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDto>> GetAllAsync();
        Task<IEnumerable<QuestionDto>> GetByAssessmentIdAsync(int assessmentId);
        Task<QuestionDto?> GetByIdAsync(int id);
        Task<QuestionDto> CreateAsync(CreateQuestionRequest request);
        Task<QuestionDto?> UpdateAsync(int id, UpdateQuestionRequest request);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ReorderQuestionsAsync(int assessmentId, Dictionary<int, int> questionOrders);
    }
}