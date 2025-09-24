// More toolboxes we need!
using System.Collections.Generic; // For lists of things
using System.Threading.Tasks; // For doing work without waiting (async)
using Microsoft.EntityFrameworkCore; // The main toolbox for talking to the database
using WebApiLMS.Data; // Where the database connection lives (the library card)
using WebApiLMS.Models; // Where the blueprints (like ProgramCourseModel) live

// This line starts defining our worker, named "ProgramCourseService"
public class ProgramCourseService
{
    // This variable holds our connection to the database (like the librarian's key to the library).
    // 'readonly' means it can only be set once when the worker is created.
    private readonly WebApiLMSDbContext _context;

    // This is the constructor. It's run when we create a new "ProgramCourseService" worker.
    // It takes the database connection ('context') as an input...
    public ProgramCourseService(WebApiLMSDbContext context)
    {
        // ...and saves it in the worker's private '_context' variable so it can use it later.
        _context = context;
    }

    // --- Methods (Things the worker can DO) ---

    // This method gets ALL the Program-Course links from the database.
    // 'async Task<List<ProgramCourseModel>>' means:
    //   'async': It might take time, so don't freeze while waiting.
    //   'Task': Represents the ongoing work.
    //   '<List<ProgramCourseModel>>': When done, it will give back a list of Program-Course link blueprints.
    public async Task<List<ProgramCourseModel>> GetAllProgramCoursesAsync()
    {
        // Ask the database ('_context') for the 'ProgramCourses' table.
        // '.Include(pc => pc.Program)' also fetches the full Program details for each link.
        // '.Include(pc => pc.Course)' also fetches the full Course details for each link.
        // '.ToListAsync()' tells the database "Get everything and put it in a list".
        // 'await' means "wait here until the database gives us the list".
        return await _context.ProgramCourses
            .Include(pc => pc.Program) // Get Program details too
            .Include(pc => pc.Course)  // Get Course details too
            .ToListAsync();           // Turn the result into a List
    }

    // This method gets ONE specific Program-Course link using its unique ID.
    // 'async Task<ProgramCourseModel>' means it will eventually give back one link blueprint (or nothing).
    public async Task<ProgramCourseModel> GetProgramCourseByIdAsync(int id) // Takes the ID number we're looking for
    {
        // Ask the database ('_context') for the 'ProgramCourses' table.
        // Include Program and Course details.
        // '.FirstOrDefaultAsync(pc => pc.Id == id)' finds the FIRST link where the link's 'Id' matches the 'id' we passed in.
        // 'await' waits for the database to find it.
        return await _context.ProgramCourses
            .Include(pc => pc.Program) // Get Program details too
            .Include(pc => pc.Course)  // Get Course details too
            .FirstOrDefaultAsync(pc => pc.Id == id); // Find the one with this specific ID
    }

    // This method ADDS a NEW Program-Course link to the database.
    // Takes the new link details ('programCourse') as input.
    // Returns the details of the link that was just added (including its new ID).
    public async Task<ProgramCourseModel> AddProgramCourseAsync(ProgramCourseModel programCourse)
    {
        // Tell the database context: "Get ready to add this new link".
        _context.ProgramCourses.Add(programCourse);
        // Tell the database context: "Okay, save all the changes you have pending (like the Add)".
        // 'await' waits for the save to finish.
        await _context.SaveChangesAsync();
        // Give back the 'programCourse' blueprint, which now has the ID assigned by the database.
        return programCourse;
    }

    // This method UPDATES an existing Program-Course link in the database.
    // Takes the ID of the link to update and the new details ('programCourse').
    // Returns the updated link blueprint (or nothing if not found).
    public async Task<ProgramCourseModel> UpdateProgramCourseAsync(int id, ProgramCourseModel programCourse)
    {
        // First, find the existing link in the database using its ID.
        // 'FindAsync(id)' is a quick way to find by ID.
        var existingProgramCourse = await _context.ProgramCourses.FindAsync(id);
        // If we didn't find anything (it's 'null')...
        if (existingProgramCourse == null)
        {
            // ...return nothing (null) to signal it wasn't found.
            return null; // or maybe signal an error
        }

        // If we found it, update its properties with the new values from 'programCourse'.
        existingProgramCourse.ProgramId = programCourse.ProgramId;   // Update the Program ID
        existingProgramCourse.CourseId = programCourse.CourseId;     // Update the Course ID
        existingProgramCourse.IsCompulsory = programCourse.IsCompulsory; // Update if it's required

        // Tell the database to save these changes.
        await _context.SaveChangesAsync();
        // Return the updated blueprint.
        return existingProgramCourse;
    }

    // Specific method to update only the IsCompulsory status
    public async Task<bool> UpdateProgramCourseCompulsoryStatusAsync(int id, bool isCompulsory)
    {
        var existingProgramCourse = await _context.ProgramCourses.FindAsync(id);
        if (existingProgramCourse == null)
        {
            return false; // Not found
        }

        existingProgramCourse.IsCompulsory = isCompulsory;
        await _context.SaveChangesAsync();
        return true; // Success
    }

    // Returns 'true' if deleted successfully, 'false' if it wasn't found.
    public async Task<bool> DeleteProgramCourseAsync(int id)
    {
        // Find the link to delete by its ID.
        var existingProgramCourse = await _context.ProgramCourses.FindAsync(id);
        if (existingProgramCourse == null)
        {
            // ...return false (couldn't delete).
            return false; // or maybe signal an error
        }

        // If found, tell the database context: "Get ready to remove this link".
        _context.ProgramCourses.Remove(existingProgramCourse);
        // Tell the database to save the changes (perform the removal).
        await _context.SaveChangesAsync();
        // Return true (deletion was successful).
        return true;
    }
}