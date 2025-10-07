## Use Sequential Thinking MCP

CRUD Generation Prompt for ASP.NET Core (with EF Migration)
UPDATED: Following Service Interface Separation & Consistent Patterns

## Step 1: Run EF Core Migration
- After adding new models and DbSet properties, run:
  ```sh
  dotnet ef migrations add <MigrationName> --project <PathToYourCsproj>
  dotnet ef database update --project <PathToYourCsproj>
  ```
- This ensures your database schema is up to date before implementing CRUD logic.
- Do not seed via code. Populate lookups via SQL scripts or dedicated migrations only.

## Step 2: Generate Complete CRUD for a New Entity in ASP.NET Core

**Context:**  
I am building an ASP.NET Core Web API project. I want you to generate all the code needed for a new entity, following the structure and conventions of my existing project.  
Use the following files as templates for each layer:

- **Controllers:**  
  - CourseController.cs  
  - ProgramController.cs  
  - ProgramCourseController.cs
- **Models:**  
  - CourseModel.cs  
  - ProgramModel.cs  
  - ProgramCourseModel.cs
- **Services:**  
  - CourseService.cs  
  - ProgramService.cs  
  - ProgramCourseService.cs
- **DTOs:**  
  - All files in /DTOs/ProgramCourse/ (e.g., ProgramCourseDto.cs, CreateProgramCourseRequest.cs, UpdateProgramCourseRequest.cs)
- **Startup/Registration:**  
  - Program.cs (for service registration, CORS, Swagger, etc.)

---

### Requirements for the Generated CRUD
1. **Model:**
   - Use Data Annotations for validation and database mapping.
   - Follow the structure of the sample models (e.g., Id as primary key, navigation properties if needed).
2. **DTOs:**
   - Create DTOs for reading, creating, and updating the entity.
   - Place them in a dedicated DTOs folder for the entity.
3. **Service Layer (UPDATED - Interface Separation):**
   - ✅ **ALWAYS create separate interface (IEntityService.cs) and implementation (EntityService.cs)**
   - ✅ **Interface in separate file** - never combine interface + implementation
   - ✅ **Controller injects concrete service class** - not interface
   - ✅ **Program.cs registers concrete service class** - not interface
   - Use async methods for all database operations.
   - Inject the DbContext and use Entity Framework Core for data access.
   - Return appropriate types (e.g., entity, bool for success, or null for not found).
4. **Controller:**
   - Use [ApiController] and [Route("api/[controller]")].
   - Implement endpoints for:
     - GET /api/[entity] (all)
     - GET /api/[entity]/{id} (by id)
     - POST /api/[entity] (create)
     - PUT /api/[entity]/{id} (update)
     - DELETE /api/[entity]/{id} (delete)
   - Use DTOs for input/output.
   - Return appropriate HTTP status codes and error handling as in the sample controllers.
5. **Program.cs / Startup (UPDATED - Concrete Registration):**
   - ✅ **Register concrete service class**: `builder.Services.AddScoped<EntityService>();`
   - ❌ **DO NOT register interface**: `builder.Services.AddScoped<IEntityService, EntityService>();`
   - ✅ **Controller injects concrete class**: `EntityService service` (not `IEntityService service`)
   - Ensure CORS and Swagger are enabled.
   - Add the DbSet for the new entity in the DbContext if not already present.
6. **DbContext:**
   - Add a DbSet<NewEntityModel> property.
7. **Validation & Error Handling:**
   - Follow the try/catch and error response patterns in the sample controllers.
   - Validate input using Data Annotations and model binding.
8. **Navigation Properties:**
   - If the entity has relationships (e.g., foreign keys), include navigation properties and configure them as in the sample models.

---

### CRITICAL: Junction/Relationship Entity Pattern (UPDATED - CourseResource, ProgramCourse, etc.)

**IMPORTANT:** For junction/relationship entities that connect two or more entities (like ProgramCourse, CourseStudentEnrollment, CourseLecturerAssignment, CourseResource), follow this EXACT pattern to ensure consistency across the entire codebase:

#### Service Interface Pattern:
```csharp
public interface I[EntityName]Service
{
    Task<List<[EntityName]Model>> GetAllAsync();           // Returns full models with includes
    Task<[EntityName]Model> GetByIdAsync(int id);          // Returns full model with includes
    Task<[EntityName]Model> CreateAsync(Create[EntityName]Request request); // Returns created model
    Task<bool> UpdateAsync(int id, Update[EntityName]Request request);
    Task<bool> DeleteAsync(int id);
}
```

#### Service Implementation Pattern:
```csharp
public class [EntityName]Service : I[EntityName]Service
{
    public async Task<List<[EntityName]Model>> GetAllAsync()
    {
        // ✅ Simple - return ALL entities with related data
        // ✅ NO filtering parameters (follows CourseResource pattern)
        return await _context.[EntityName]s
            .Include(e => e.RelatedEntity1)
            .Include(e => e.RelatedEntity2)
            .OrderBy(e => e.SortOrder ?? e.Id) // Optional ordering
            .ToListAsync();
    }

    public async Task<[EntityName]Model> GetByIdAsync(int id)
    {
        return await _context.[EntityName]s
            .Include(e => e.RelatedEntity1)
            .Include(e => e.RelatedEntity2)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<[EntityName]Model> CreateAsync(Create[EntityName]Request request)
    {
        var entity = new [EntityName]Model
        {
            // Map properties from request
        };
        _context.[EntityName]s.Add(entity);
        await _context.SaveChangesAsync();
        return entity; // Return the created entity (not DTO)
    }
}
```

#### Controller Pattern (UPDATED - DTO Consistency):
```csharp
[HttpGet]
public async Task<ActionResult<List<[EntityName]Dto>>> GetAll()
{
    var entities = await _service.GetAllAsync();
    var dtos = entities.Select(e => new [EntityName]Dto
    {
      Id = e.Id, // For internal linking/routing only. UI MUST NOT display raw IDs.
        // Map basic properties
        RelatedEntityName = e.RelatedEntity?.Name ?? "N/A", // Include navigation names
        // ... other properties
        RelatedEntity = e.RelatedEntity // Include full navigation model
    });
    return Ok(dtos); // Return DTOs with navigation data
}

[HttpGet("{id}")]
public async Task<ActionResult<[EntityName]Dto>> GetById(int id)
{
    var entity = await _service.GetByIdAsync(id);
    if (entity == null) return NotFound();

    var dto = new [EntityName]Dto
    {
        Id = entity.Id,
        // Map basic properties
        RelatedEntityName = entity.RelatedEntity?.Name ?? "N/A",
        RelatedEntity = entity.RelatedEntity // Include full navigation model
    };
    return Ok(dto);
}

[HttpPost]
public async Task<ActionResult<[EntityName]Dto>> Create([FromBody] Create[EntityName]Request request)
{
    var createdEntity = await _service.CreateAsync(request);

    var createdDto = new [EntityName]Dto
    {
        Id = createdEntity.Id,
        // Map basic properties
        RelatedEntityName = createdEntity.RelatedEntity?.Name ?? "N/A",
        RelatedEntity = createdEntity.RelatedEntity // Include full navigation model
    };
    return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
}
```

#### Key Points for Junction Entities (UPDATED):
1. ✅ **Service methods return Model types** (not DTOs) for GetAll, GetById, and Create
2. ✅ **Controller returns DTOs** (not raw models) for all endpoints
3. ✅ **DTOs include navigation properties** (RelatedEntityName + full RelatedEntity model)
4. ✅ **Always use .Include()** for related entities in service queries
5. ✅ **No filtering parameters** in GetAll() - return ALL entities (follows CourseResource pattern)
6. ✅ **Separate interface and implementation** files for all services
7. ✅ **Register concrete service classes** (not interfaces) in Program.cs
8. ✅ **Do NOT show IDs in the UI** — display human-friendly names only. Keep Id/foreign keys for internal routing and as hidden form fields.
9. ✅ **Server-set fields** (e.g., EnrolledAt/CreatedAt) are set on create and not updatable in Update requests

---

### Example Entity Name: Student
**Please generate:**
- StudentModel.cs (with Data Annotations)
- DTOs: StudentDto.cs, CreateStudentRequest.cs, UpdateStudentRequest.cs
- IStudentService.cs and StudentService.cs
- StudentController.cs
- Registration in Program.cs
- DbSet<StudentModel> in WebApiLMSDbContext.cs

**Follow the structure and conventions of the provided templates.**  
**Ensure all CRUD operations are fully functional and ready to use.**  
**If any additional configuration or code is needed for the CRUD to work (e.g., migrations, DI registration, DbContext changes), include that as well.**

---

### Additional Notes
- Use async/await for all database operations.
- Use dependency injection for services.
- Use [FromBody] for POST/PUT requests.
- Return NotFound, Ok, CreatedAtAction, or NoContent as appropriate.
- Use DTOs for all API input/output, not the EF models directly.
- If the entity has relationships, include navigation properties and handle them as in ProgramCourseModel.
- Ensure the new entity is included in the DbContext and registered in DI in Program.cs.

---

## 🚀 RECENT IMPROVEMENTS (Service Refactoring Session)

### **✅ Major Architectural Changes:**

1. **Service Interface Separation:**
   - ✅ All services now have separate interface and implementation files
   - ✅ Fixed inconsistent patterns across the codebase
   - ✅ Improved maintainability and testability

2. **CourseResource Pattern Standardization:**
   - ✅ Removed filtering parameters from GetAll() methods
   - ✅ Simplified API endpoints to return ALL resources
   - ✅ Consistent with ProgramCourseController pattern

3. **DTO Consistency Improvements:**
   - ✅ Added navigation property names (e.g., CourseName, UserName)
   - ✅ Included full navigation models in DTOs
   - ✅ Standardized null handling with "N/A" fallbacks

4. **Dependency Injection Fixes:**
   - ✅ Register concrete service classes (not interfaces)
   - ✅ Fixed all Program.cs registrations
   - ✅ Consistent injection patterns across controllers

5. **Error Resolution:**
   - ✅ Fixed CourseName property missing in DTO
   - ✅ Fixed Course navigation property missing
   - ✅ Resolved all compilation errors

### **📊 Pattern Consistency Achieved:**

| **Component** | **Before** | **After** | **Status** |
|---------------|------------|-----------|------------|
| **Service Files** | Mixed patterns | Interface + Implementation | ✅ Consistent |
| **Controller Returns** | Mixed (Models/DTOs) | DTOs only | ✅ Consistent |
| **DI Registration** | Mixed approaches | Concrete classes only | ✅ Consistent |
| **Navigation Data** | Inconsistent | Full models + names | ✅ Consistent |
| **GetAll() Method** | With filtering params | No params (simple) | ✅ Consistent |

---

**End of Prompt**
