using Microsoft.AspNetCore.Mvc;
using WebApiLMS.Models;
using WebApiLMS.Services;

namespace WebApiLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _userService.RegisterUser(
                    request.FullName,
                    request.Email,
                    request.Password,
                    request.Role
                );

                return Ok(new AuthResponse
                {
                    Success = true,
                    Message = "Registration successful",
                    User = new UserResponse
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        Role = user.UserRole?.Name ?? user.UserRoleId.ToString(),
                        AccountStatus = user.AccountStatus,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userService.LoginUser(request.Email, request.Password);

                return Ok(new AuthResponse
                {
                    Success = true,
                    Message = "Login successful.",
                    User = new UserResponse
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        Role = user.UserRole?.Name ?? user.UserRoleId.ToString(),
                        AccountStatus = user.AccountStatus,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(new UserResponse
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.UserRole?.Name ?? user.UserRoleId.ToString(),
                    AccountStatus = user.AccountStatus,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var success = await _userService.UpdateUser(
                    id,
                    request.FullName,
                    request.Email,
                    request.Role,
                    request.AccountStatus
                );

                if (!success)
                {
                    return NotFound();
                }

                return Ok(new { Message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _userService.DeleteUser(id);
                if (!success)
                {
                    return NotFound();
                }

                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                var userResponses = users.Select(user => new UserResponse
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.UserRole?.Name ?? user.UserRoleId.ToString(),
                    AccountStatus = user.AccountStatus,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }).ToList();

                return Ok(userResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
} 