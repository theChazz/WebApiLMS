using WebApiLMS.Data;
using WebApiLMS.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiLMS.Services
{
    public class UserService : IUserService
    {
        private readonly WebApiLMSDbContext _context;

        public UserService(WebApiLMSDbContext context)
        {
            _context = context;
        }

        private async Task<int> MapRoleToId(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required.");

            // If a numeric string is provided, treat as Role Id first
            if (int.TryParse(role, out var parsedId))
            {
                var byId = await _context.UserRoles.FirstOrDefaultAsync(r => r.Id == parsedId);
                if (byId != null) return byId.Id;
            }

            // Otherwise match by Code (preferred) or Name (fallback), case-insensitive
            var normalized = role.Trim();
            var roleEntity = await _context.UserRoles
                .FirstOrDefaultAsync(r => r.Code == normalized || r.Name == normalized);

            if (roleEntity == null)
            {
                // Try case-insensitive match
                roleEntity = await _context.UserRoles
                    .FirstOrDefaultAsync(r => r.Code.ToLower() == normalized.ToLower() || r.Name.ToLower() == normalized.ToLower());
            }

            if (roleEntity == null)
            {
                throw new Exception($"Unknown role: {role}");
            }
            return roleEntity.Id;
        }

    public async Task<Users> RegisterUser(string fullName, string email, string password, string role)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }

            var user = new Users
            {
                FullName = fullName,
                Email = email,
                PasswordHash = Users.HashPassword(password),
                UserRoleId = await MapRoleToId(role),
                AccountStatus = "Active",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Ensure role navigation property is available for callers
            await _context.Entry(user).Reference(u => u.UserRole).LoadAsync();

            return user;
        }

        public async Task<Users> LoginUser(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !Users.VerifyPassword(password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            return user;
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UpdateUser(int id, string fullName, string email, string role, string accountStatus)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            user.FullName = fullName;
            user.Email = email;
            user.UserRoleId = await MapRoleToId(role);
            user.UpdatedAt = DateTime.UtcNow;
            user.AccountStatus = accountStatus;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.UserRole)
                .ToListAsync();
        }

        public async Task<bool> UpdateUserStatus(int id, string accountStatus)
        {
            var allowedStatuses = new List<string> { "Active", "Inactive", "Suspended" };
            if (!allowedStatuses.Contains(accountStatus))
            {
                 throw new ArgumentException("Invalid account status provided.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            user.AccountStatus = accountStatus;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
} 