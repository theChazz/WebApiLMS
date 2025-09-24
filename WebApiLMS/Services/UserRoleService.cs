using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
	public class UserRoleService : IUserRoleService
	{
		private readonly WebApiLMSDbContext _context;

		public UserRoleService(WebApiLMSDbContext context)
		{
			_context = context;
		}

		public async Task<List<UserRoleModel>> GetAllAsync()
		{
			return await _context.UserRoles.OrderBy(x => x.Name).ToListAsync();
		}

		public async Task<UserRoleModel> GetByIdAsync(int id)
		{
			return await _context.UserRoles.FindAsync(id);
		}

		public async Task<UserRoleModel> CreateAsync(UserRoleModel model)
		{
			_context.UserRoles.Add(model);
			await _context.SaveChangesAsync();
			return model;
		}

		public async Task<bool> UpdateAsync(int id, UserRoleModel model)
		{
			var existing = await _context.UserRoles.FindAsync(id);
			if (existing == null) return false;
			existing.Code = model.Code;
			existing.Name = model.Name;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var existing = await _context.UserRoles.FindAsync(id);
			if (existing == null) return false;
			_context.UserRoles.Remove(existing);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}


