using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public interface IProgramCourseService
    {
        Task<List<ProgramCourseModel>> GetAllProgramCoursesAsync();
        Task<ProgramCourseModel> GetProgramCourseByIdAsync(int id);
        Task<ProgramCourseModel> AddProgramCourseAsync(ProgramCourseModel programCourse);
        Task<ProgramCourseModel> UpdateProgramCourseAsync(int id, ProgramCourseModel programCourse);
        Task<bool> UpdateProgramCourseCompulsoryStatusAsync(int id, bool isCompulsory);
        Task<bool> DeleteProgramCourseAsync(int id);
    }
}
