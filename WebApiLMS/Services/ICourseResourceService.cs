using WebApiLMS.DTOs.CourseResource;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public interface ICourseResourceService
    {
        Task<List<CourseResourceModel>> GetAllAsync();
        Task<CourseResourceModel?> GetByIdAsync(int id);
        Task<CourseResourceModel> CreateAsync(CreateCourseResourceRequest request);
        Task<bool> UpdateAsync(int id, UpdateCourseResourceRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
