using WebApiLMS.Models;

namespace WebApiLMS.Services
{
	public interface IUserRoleService
	{
		Task<List<UserRoleModel>> GetAllAsync();
		Task<UserRoleModel> GetByIdAsync(int id);
		Task<UserRoleModel> CreateAsync(UserRoleModel model);
		Task<bool> UpdateAsync(int id, UserRoleModel model);
		Task<bool> DeleteAsync(int id);
	}
}
