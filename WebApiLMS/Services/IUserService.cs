using WebApiLMS.Models;

namespace WebApiLMS.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users> RegisterUser(string fullName, string email, string password, string role);
        Task<Users> LoginUser(string email, string password);
        Task<Users> GetUserById(int id);
        Task<bool> UpdateUser(int id, string fullName, string email, string role, string accountStatus);
        Task<bool> DeleteUser(int id);
        Task<bool> UpdateUserStatus(int id, string accountStatus);
    }
}
