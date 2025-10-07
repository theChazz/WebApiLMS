using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public class UserProgramEnrollmentService : IUserProgramEnrollmentService
    {
        // This would typically be injected via Dependency Injection
        private readonly WebApiLMSDbContext _context;

        public UserProgramEnrollmentService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserProgramEnrollmentModel>> GetAllEnrollmentsAsync()
        {
            // Fetch all enrollments from the database
            //return await _context.UserProgramEnrollments.ToListAsync();
            
            // Include the related User and Program details with each enrollment
            return await _context.UserProgramEnrollments
                .Include(e => e.User)   // Add this line to include User details
                .Include(e => e.Program) // Add this line to include Program details
                .ToListAsync();
        }

        public async Task<UserProgramEnrollmentModel> GetEnrollmentByIdAsync(int id)
        {
            // Include navigation properties so callers (DTO mapping/UI) can access names
            return await _context.UserProgramEnrollments
                .Include(e => e.User)
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<UserProgramEnrollmentModel> AddEnrollmentAsync(UserProgramEnrollmentModel enrollment)
        {
            _context.UserProgramEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<UserProgramEnrollmentModel> UpdateEnrollmentAsync(int id, UserProgramEnrollmentModel enrollment)
        {
            var existingEnrollment = await _context.UserProgramEnrollments.FindAsync(id);
            if (existingEnrollment == null) return null;

            existingEnrollment.UserId = enrollment.UserId;
            existingEnrollment.ProgramId = enrollment.ProgramId;
            existingEnrollment.Status = enrollment.Status;
            existingEnrollment.EnrolledAt = enrollment.EnrolledAt;
            existingEnrollment.ExpectedCompletionDate = enrollment.ExpectedCompletionDate;

            await _context.SaveChangesAsync();
            return existingEnrollment;
        }

        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            var enrollment = await _context.UserProgramEnrollments.FindAsync(id);
            if (enrollment == null) return false;

            _context.UserProgramEnrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
