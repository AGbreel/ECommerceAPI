using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceAPI.Services;
using ECommerceAPI.Repositories;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IUserRepository _repo;

        public UsersController(IAuthService auth, IUserRepository repo)
        {
            _auth = auth;
            _repo = repo;
        }

        // ✅ Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var user = await _auth.Register(dto);

                var token = await _auth.Login(new LoginDto { Email = dto.Email, Password = dto.Password });

                return Ok(new
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = new
                    {
                        Token = token,
                        UserId = user.UserId,
                        Email = user.Email,
                        Role = user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // ✅ Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            var token = await _auth.Login(dto);

            if (token == null || user == null)
                return Unauthorized(new { Success = false, Message = "Invalid credentials" });

            return Ok(new
            {
                Success = true,
                Message = "Login successful",
                Data = new
                {
                    Token = token,
                    UserId = user.UserId,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }

        // ✅ Get All Users (Admin only)
        [HttpGet]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repo.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = users
            });
        }

        // ✅ Get User By Id
        [HttpGet("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { Success = false, Message = "User not found" });

            return Ok(new
            {
                Success = true,
                Message = "User retrieved successfully",
                Data = user
            });
        }

        // ✅ Update User
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Success = false, Message = "User not found" });

            user.UserId = id;
            var result = await _repo.UpdateAsync(user);

            if (!result)
                return BadRequest(new { Success = false, Message = "Update failed" });

            return Ok(new
            {
                Success = true,
                Message = "User updated successfully"
            });
        }

        // ✅ Delete User
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Success = false, Message = "User not found" });

            var result = await _repo.DeleteAsync(id);

            if (!result)
                return BadRequest(new { Success = false, Message = "Delete failed" });

            return Ok(new
            {
                Success = true,
                Message = "User deleted successfully"
            });
        }
    }
}
