using Microsoft.EntityFrameworkCore;
using WebApiLMS.Data;
using WebApiLMS.Models;

namespace WebApiLMS.Services
{
	public interface IUserCredentialService
	{
		Task<List<UserCredentialModel>> GetAllAsync();
		Task<UserCredentialModel> GetByIdAsync(int id);
		Task<UserCredentialModel> CreateAsync(UserCredentialModel model);
		Task<bool> UpdateAsync(int id, UserCredentialModel model);
		Task<bool> DeleteAsync(int id);
	}

	public class UserCredentialService : IUserCredentialService
	{
		private readonly WebApiLMSDbContext _context;

		public UserCredentialService(WebApiLMSDbContext context)
		{
			_context = context;
		}

		public async Task<List<UserCredentialModel>> GetAllAsync()
		{
			return await _context.UserCredentials
				.Include(x => x.User)
				.Include(x => x.SetaBody)
				.ToListAsync();
		}

		public async Task<UserCredentialModel> GetByIdAsync(int id)
		{
			return await _context.UserCredentials
				.Include(x => x.User)
				.Include(x => x.SetaBody)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<UserCredentialModel> CreateAsync(UserCredentialModel model)
		{
			_context.UserCredentials.Add(model);
			await _context.SaveChangesAsync();
			return model;
		}

		public async Task<bool> UpdateAsync(int id, UserCredentialModel model)
		{
			var existing = await _context.UserCredentials.FindAsync(id);
			if (existing == null) return false;
			existing.CredentialType = model.CredentialType;
			existing.RegistrationNumber = model.RegistrationNumber;
			existing.SetaBodyId = model.SetaBodyId;
			existing.Scope = model.Scope;
			existing.ValidFrom = model.ValidFrom;
			existing.ValidTo = model.ValidTo;
			existing.Status = model.Status;
			existing.EvidenceUrl = model.EvidenceUrl;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var existing = await _context.UserCredentials.FindAsync(id);
			if (existing == null) return false;
			_context.UserCredentials.Remove(existing);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}


