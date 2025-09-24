using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.DTOs.CourseResource;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public class CourseResourceService
    {
        private readonly WebApiLMSDbContext _context;

        public CourseResourceService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseResourceModel>> GetAllAsync()
        {
            return await _context.CourseResources
                .Include(r => r.Course)
                .OrderBy(r => r.SortOrder)
                .ThenBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<CourseResourceModel?> GetByIdAsync(int id)
        {
            return await _context.CourseResources
                .Include(r => r.Course)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<CourseResourceModel> CreateAsync(CreateCourseResourceRequest request)
        {
            var type = Enum.Parse<CourseResourceType>(request.Type, true);

            var courseExists = await _context.Courses.AnyAsync(c => c.Id == request.CourseId);
            if (!courseExists)
            {
                throw new KeyNotFoundException($"Course {request.CourseId} not found");
            }

            var entity = new CourseResourceModel
            {
                CourseId = request.CourseId,
                Type = type,
                Title = request.Title,
                Description = request.Description,
                Url = request.Url,
                Provider = request.Provider,
                MimeType = request.MimeType,
                SizeBytes = request.SizeBytes,
                StartsAt = request.StartsAt,
                EndsAt = request.EndsAt,
                Timezone = request.Timezone,
                IsPublished = request.IsPublished,
                Module = request.Module,
                SortOrder = request.SortOrder,
                CreatedAt = DateTime.UtcNow
            };

            _context.CourseResources.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCourseResourceRequest request)
        {
            var entity = await _context.CourseResources.FindAsync(id);
            if (entity == null) return false;

            entity.Type = Enum.Parse<CourseResourceType>(request.Type, true);
            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.Url = request.Url;
            entity.Provider = request.Provider;
            entity.MimeType = request.MimeType;
            entity.SizeBytes = request.SizeBytes;
            entity.StartsAt = request.StartsAt;
            entity.EndsAt = request.EndsAt;
            entity.Timezone = request.Timezone;
            entity.IsPublished = request.IsPublished;
            entity.Module = request.Module;
            entity.SortOrder = request.SortOrder;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.CourseResources.FindAsync(id);
            if (entity == null) return false;

            _context.CourseResources.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
