using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLMS.Models;
using WebApiLMS.Services;
using WebApiLMS.DTOs.ProgramCourse;

[Route("api/[controller]")]
[ApiController]
public class ProgramCourseController : ControllerBase
{
    private readonly ProgramCourseService _service;

    public ProgramCourseController(ProgramCourseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProgramCourseDto>>> GetProgramCourses()
    {
        var programCourses = await _service.GetAllProgramCoursesAsync();
        return Ok(programCourses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProgramCourseDto>> GetProgramCourse(int id)
    {
        var programCourse = await _service.GetProgramCourseByIdAsync(id);
        if (programCourse == null)
        {
            return NotFound();
        }
        var programCourseDto = new ProgramCourseDto
        {
            Id = programCourse.Id,
            ProgramId = programCourse.ProgramId,
            ProgramName = programCourse.Program?.Name ?? "N/A",
            CourseId = programCourse.CourseId,
            CourseName = programCourse.Course?.CourseName ?? "N/A",
            IsCompulsory = programCourse.IsCompulsory,
            CreatedAt = programCourse.CreatedAt
        };
        return Ok(programCourseDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProgramCourseDto>> AddProgramCourse(CreateProgramCourseRequest requestDto)
    {
        var programCourseModel = new ProgramCourseModel
        {
            ProgramId = requestDto.ProgramId,
            CourseId = requestDto.CourseId,
            IsCompulsory = requestDto.IsCompulsory,
        };

        var createdProgramCourse = await _service.AddProgramCourseAsync(programCourseModel);
        
        var createdDto = new ProgramCourseDto 
        { 
             Id = createdProgramCourse.Id, 
             ProgramId = createdProgramCourse.ProgramId, 
             CourseId = createdProgramCourse.CourseId, 
             IsCompulsory = createdProgramCourse.IsCompulsory, 
             CreatedAt = createdProgramCourse.CreatedAt 
        }; 

        return CreatedAtAction(nameof(GetProgramCourse), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProgramCourse(int id, UpdateProgramCourseRequest requestDto)
    {
        var updated = await _service.UpdateProgramCourseCompulsoryStatusAsync(id, requestDto.IsCompulsory);
        
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProgramCourse(int id)
    {
        var result = await _service.DeleteProgramCourseAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}