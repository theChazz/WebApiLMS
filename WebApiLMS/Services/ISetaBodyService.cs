using WebApiLMS.Models;

namespace WebApiLMS.Services
{
	public interface ISetaBodyService
	{
		Task<List<SetaBodyModel>> GetAllAsync();
		Task<SetaBodyModel> GetByIdAsync(int id);
		Task<SetaBodyModel> CreateAsync(SetaBodyModel model);
		Task<bool> UpdateAsync(int id, SetaBodyModel model);
		Task<bool> DeleteAsync(int id);
	}
}
