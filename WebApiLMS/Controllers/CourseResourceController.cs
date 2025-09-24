using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLMS.Models;
using WebApiLMS.Services;
using WebApiLMS.DTOs.CourseResource;

[Route("api/[controller]")]
[ApiController]
public class CourseResourceController : ControllerBase
{
    private readonly CourseResourceService _service;

    public CourseResourceController(CourseResourceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CourseResourceDto>>> GetCourseResources()
        {
            var resources = await _service.GetAllAsync();
            var dtos = resources.Select(r => new CourseResourceDto
            {
                Id = r.Id,
                CourseId = r.CourseId,
                CourseName = r.Course?.CourseName ?? "N/A",
                Type = r.Type.ToString(),
                Title = r.Title,
                Description = r.Description,
                Url = r.Url,
                Provider = r.Provider,
                MimeType = r.MimeType,
                SizeBytes = r.SizeBytes,
                StartsAt = r.StartsAt,
                EndsAt = r.EndsAt,
                Timezone = r.Timezone,
                IsPublished = r.IsPublished,
                Module = r.Module,
                SortOrder = r.SortOrder,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                Course = r.Course
            });
            return Ok(dtos);
        }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseResourceDto>> GetCourseResource(int id)
        {
            var resource = await _service.GetByIdAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            var dto = new CourseResourceDto
            {
                Id = resource.Id,
                CourseId = resource.CourseId,
                CourseName = resource.Course?.CourseName ?? "N/A",
                Type = resource.Type.ToString(),
                Title = resource.Title,
                Description = resource.Description,
                Url = resource.Url,
                Provider = resource.Provider,
                MimeType = resource.MimeType,
                SizeBytes = resource.SizeBytes,
                StartsAt = resource.StartsAt,
                EndsAt = resource.EndsAt,
                Timezone = resource.Timezone,
                IsPublished = resource.IsPublished,
                Module = resource.Module,
                SortOrder = resource.SortOrder,
                CreatedAt = resource.CreatedAt,
                UpdatedAt = resource.UpdatedAt,
                Course = resource.Course
            };
            return Ok(dto);
        }

    [HttpPost]
    public async Task<ActionResult<CourseResourceDto>> CreateCourseResource(CreateCourseResourceRequest request)
        {
            var createdResource = await _service.CreateAsync(request);

            var dto = new CourseResourceDto
            {
                Id = createdResource.Id,
                CourseId = createdResource.CourseId,
                CourseName = createdResource.Course?.CourseName ?? "N/A",
                Type = createdResource.Type.ToString(),
                Title = createdResource.Title,
                Description = createdResource.Description,
                Url = createdResource.Url,
                Provider = createdResource.Provider,
                MimeType = createdResource.MimeType,
                SizeBytes = createdResource.SizeBytes,
                StartsAt = createdResource.StartsAt,
                EndsAt = createdResource.EndsAt,
                Timezone = createdResource.Timezone,
                IsPublished = createdResource.IsPublished,
                Module = createdResource.Module,
                SortOrder = createdResource.SortOrder,
                CreatedAt = createdResource.CreatedAt,
                UpdatedAt = createdResource.UpdatedAt,
                Course = createdResource.Course
            };

            return CreatedAtAction(nameof(GetCourseResource), new { id = dto.Id }, dto);
        }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourseResource(int id, UpdateCourseResourceRequest request)
        {
            var success = await _service.UpdateAsync(id, request);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourseResource(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    
}
