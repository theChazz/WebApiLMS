using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public interface IUserProgramEnrollmentService
    {
        Task<List<UserProgramEnrollmentModel>> GetAllEnrollmentsAsync();
        Task<UserProgramEnrollmentModel> GetEnrollmentByIdAsync(int id);
        Task<UserProgramEnrollmentModel> AddEnrollmentAsync(UserProgramEnrollmentModel enrollment);
        Task<UserProgramEnrollmentModel> UpdateEnrollmentAsync(int id, UserProgramEnrollmentModel enrollment);
        Task<bool> DeleteEnrollmentAsync(int id);
    }
}
