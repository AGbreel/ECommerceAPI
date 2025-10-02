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
        // ✅ Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var user = await _auth.Register(dto);

                // بعد التسجيل الناجح نرجع التوكن والـ UserId
                var token = await _auth.Login(new LoginDto { Email = dto.Email, Password = dto.Password });

                return Ok(new
                {
                    Message = "Registered",
                    Token = token,
                    UserId = user.UserId,
                    Email = user.Email,
                    Role = user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email); // 👈 لازم يكون عندك Repo Method دي
            var token = await _auth.Login(dto);

            if (token == null || user == null)
                return Unauthorized("Invalid credentials");

            return Ok(new
            {
                Token = token,
                UserId = user.UserId,
                Email = user.Email,
                Role = user.Role
            });
        }

        // ✅ Get All Users (Admin only)
        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repo.GetAllAsync();
            return Ok(users);
        }

        // ✅ Get User By Id (Admin only)
        // [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        // ✅ Update User (Admin only)
        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound("User not found");

            user.UserId = id; // نتأكد إن الـ Id مظبوط
            var result = await _repo.UpdateAsync(user);

            if (!result) return BadRequest("Update failed");
            return Ok("User updated successfully");
        }

        // ✅ Delete User (Admin only)
        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound("User not found");

            var result = await _repo.DeleteAsync(id);

            if (!result) return BadRequest("Delete failed");
            return Ok("User deleted successfully");
        }
    }
}
