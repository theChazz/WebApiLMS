using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public interface IProgramService
    {
        Task<List<ProgramModel>> GetAllProgramsAsync();
        Task<ProgramModel> GetProgramByIdAsync(int id);
        Task<ProgramModel> CreateProgramAsync(ProgramModel program);
        Task<bool> UpdateProgramAsync(int id, ProgramModel program);
        Task<bool> DeleteProgramAsync(int id);
    }
}
