using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public interface ICourseService
    {
        Task<List<CourseModel>> GetAllCoursesAsync();
        Task<CourseModel> GetCourseByIdAsync(int id);
        Task<List<CourseModel>> SearchCoursesAsync(string query, int take = 10);
        Task<CourseModel> CreateCourseAsync(CourseModel course);
        Task<bool> UpdateCourseAsync(int id, CourseModel course);
        Task<bool> DeleteCourseAsync(int id);
    }
}
