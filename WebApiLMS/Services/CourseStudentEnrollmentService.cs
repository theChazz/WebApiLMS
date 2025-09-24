using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiLMS.Data;
using WebApiLMS.DTOs.CourseStudentEnrollment;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public class CourseStudentEnrollmentService : ICourseStudentEnrollmentService
    {
        private readonly WebApiLMSDbContext _context;
        public CourseStudentEnrollmentService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseStudentEnrollmentModel>> GetAllAsync()
        {
            return await _context.CourseStudentEnrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .ToListAsync();
        }

        public async Task<CourseStudentEnrollmentModel> GetByIdAsync(int id)
        {
            return await _context.CourseStudentEnrollments
                .Include(enrollment => enrollment.Course)
                .Include(enrollment => enrollment.Student)
                .FirstOrDefaultAsync(enrollment => enrollment.Id == id);
        }

        public async Task<CourseStudentEnrollmentModel> CreateAsync(CreateCourseStudentEnrollmentRequest request)
        {
            var entity = new CourseStudentEnrollmentModel
            {
                CourseId = request.CourseId,
                StudentId = request.StudentId,
                Progress = request.Progress
            };
            _context.CourseStudentEnrollments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCourseStudentEnrollmentRequest request)
        {
            var entity = await _context.CourseStudentEnrollments.FindAsync(id);
            if (entity == null) return false;
            entity.Progress = request.Progress;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.CourseStudentEnrollments.FindAsync(id);
            if (entity == null) return false;
            _context.CourseStudentEnrollments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}