using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
	public class ProgramTypeService : IProgramTypeService
	{
		private readonly WebApiLMSDbContext _context;

		public ProgramTypeService(WebApiLMSDbContext context)
		{
			_context = context;
		}

		public async Task<List<ProgramTypeModel>> GetAllAsync()
		{
			return await _context.ProgramTypes.OrderBy(x => x.Name).ToListAsync();
		}

		public async Task<ProgramTypeModel> GetByIdAsync(int id)
		{
			return await _context.ProgramTypes.FindAsync(id);
		}

		public async Task<ProgramTypeModel> CreateAsync(ProgramTypeModel model)
		{
			_context.ProgramTypes.Add(model);
			await _context.SaveChangesAsync();
			return model;
		}

		public async Task<bool> UpdateAsync(int id, ProgramTypeModel model)
		{
			var existing = await _context.ProgramTypes.FindAsync(id);
			if (existing == null) return false;
			existing.Code = model.Code;
			existing.Name = model.Name;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var existing = await _context.ProgramTypes.FindAsync(id);
			if (existing == null) return false;
			_context.ProgramTypes.Remove(existing);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}


