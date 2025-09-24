using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLMS.DTOs.CourseLecturerAssignment;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public interface ICourseLecturerAssignmentService
    {
        Task<List<CourseLecturerAssignmentModel>> GetAllAsync();
        Task<CourseLecturerAssignmentModel> GetByIdAsync(int id);
        Task<CourseLecturerAssignmentModel> CreateAsync(CreateCourseLecturerAssignmentRequest request);
        Task<bool> UpdateAsync(int id, UpdateCourseLecturerAssignmentRequest request);
        Task<bool> DeleteAsync(int id);
    }
}