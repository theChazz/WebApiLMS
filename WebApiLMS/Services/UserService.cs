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
            var roleEntity = await _context.UserRoles.FirstOrDefaultAsync(r => r.Code == role);
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

            return user;
        }

        public async Task<Users> LoginUser(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !Users.VerifyPassword(password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            return user;
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
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
            return await _context.Users.ToListAsync();
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