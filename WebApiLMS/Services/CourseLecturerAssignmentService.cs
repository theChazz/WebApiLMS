using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiLMS.Data;
using WebApiLMS.DTOs.CourseLecturerAssignment;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public class CourseLecturerAssignmentService : ICourseLecturerAssignmentService
    {
        private readonly WebApiLMSDbContext _context;
        public CourseLecturerAssignmentService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseLecturerAssignmentModel>> GetAllAsync()
        {
            return await _context.CourseLecturerAssignments
                .Include(a => a.Course)
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<CourseLecturerAssignmentModel> GetByIdAsync(int id)
        {
            return await _context.CourseLecturerAssignments
                .Include(a => a.Course)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<CourseLecturerAssignmentModel> CreateAsync(CreateCourseLecturerAssignmentRequest request)
        {
            var entity = new CourseLecturerAssignmentModel
            {
                CourseId = request.CourseId,
                UserId = request.UserId
            };
            _context.CourseLecturerAssignments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCourseLecturerAssignmentRequest request)
        {
            // No updatable fields for now
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.CourseLecturerAssignments.FindAsync(id);
            if (entity == null) return false;
            _context.CourseLecturerAssignments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}