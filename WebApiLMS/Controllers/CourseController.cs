using Microsoft.AspNetCore.Mvc;
using WebApiLMS.Models;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                return Ok(courses);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching courses");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int take = 10)
        {
            try
            {
                var results = await _courseService.SearchCoursesAsync(q ?? string.Empty, take);
                var shaped = results.Select(c => new { id = c.Id, courseName = c.CourseName });
                return Ok(shaped);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while searching courses");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                if (course == null)
                {
                    return NotFound("Course not found");
                }
                return Ok(course);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the course");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseModel request)
        {
            try
            {
                var course = await _courseService.CreateCourseAsync(request);
                return Ok(new { Id = course.Id, Message = "Course created successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the course");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseModel request)
        {
            try
            {
                var success = await _courseService.UpdateCourseAsync(id, request);
                if (!success)
                {
                    return NotFound("Course not found");
                }
                return Ok(new { Message = "Course updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the course");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var success = await _courseService.DeleteCourseAsync(id);
                if (!success)
                {
                    return NotFound("Course not found");
                }
                return Ok(new { Message = "Course deleted successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the course");
            }
        }
    }
} 