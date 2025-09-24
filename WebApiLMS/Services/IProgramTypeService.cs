using WebApiLMS.Models;

namespace WebApiLMS.Services
{
	public interface IProgramTypeService
	{
		Task<List<ProgramTypeModel>> GetAllAsync();
		Task<ProgramTypeModel> GetByIdAsync(int id);
		Task<ProgramTypeModel> CreateAsync(ProgramTypeModel model);
		Task<bool> UpdateAsync(int id, ProgramTypeModel model);
		Task<bool> DeleteAsync(int id);
	}
}
