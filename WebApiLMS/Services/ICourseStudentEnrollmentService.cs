using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLMS.DTOs.CourseStudentEnrollment;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public interface ICourseStudentEnrollmentService
    {
        Task<List<CourseStudentEnrollmentModel>> GetAllAsync();
        Task<CourseStudentEnrollmentModel> GetByIdAsync(int id);
        Task<CourseStudentEnrollmentModel> CreateAsync(CreateCourseStudentEnrollmentRequest request);
        Task<bool> UpdateAsync(int id, UpdateCourseStudentEnrollmentRequest request);
        Task<bool> DeleteAsync(int id);
    }
}