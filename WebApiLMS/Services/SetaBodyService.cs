using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
	public class SetaBodyService : ISetaBodyService
	{
		private readonly WebApiLMSDbContext _context;

		public SetaBodyService(WebApiLMSDbContext context)
		{
			_context = context;
		}

		public async Task<List<SetaBodyModel>> GetAllAsync()
		{
			return await _context.SetaBodies.OrderBy(x => x.Name).ToListAsync();
		}

		public async Task<SetaBodyModel> GetByIdAsync(int id)
		{
			return await _context.SetaBodies.FindAsync(id);
		}

		public async Task<SetaBodyModel> CreateAsync(SetaBodyModel model)
		{
			_context.SetaBodies.Add(model);
			await _context.SaveChangesAsync();
			return model;
		}

		public async Task<bool> UpdateAsync(int id, SetaBodyModel model)
		{
			var existing = await _context.SetaBodies.FindAsync(id);
			if (existing == null) return false;
			existing.Code = model.Code;
			existing.Name = model.Name;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var existing = await _context.SetaBodies.FindAsync(id);
			if (existing == null) return false;
			_context.SetaBodies.Remove(existing);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}


