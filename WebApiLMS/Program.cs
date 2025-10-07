using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.Models;
using WebApiLMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<WebApiLMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<ProgramService>();
builder.Services.AddScoped<UserProgramEnrollmentService>();
builder.Services.AddScoped<ProgramCourseService>();
builder.Services.AddScoped<SetaBodyService>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<ProgramTypeService>();
builder.Services.AddScoped<CourseStudentEnrollmentService>();
builder.Services.AddScoped<CourseLecturerAssignmentService>();
builder.Services.AddScoped<CourseResourceService>();
builder.Services.AddScoped<AssessmentService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IGradingService, GradingService>();
builder.Services.AddScoped<IRubricService, RubricService>();

// Register missing UserCredential service DI
builder.Services.AddScoped<IUserCredentialService, UserCredentialService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Run migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WebApiLMSDbContext>();
    db.Database.Migrate();
    
    // Seed dummy data
    await DataSeeder.SeedAsync(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS policy
app.UseCors("AllowAll"); app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//app.MapUsersEndpoints(); causes conflict with the UserController

app.Run();
