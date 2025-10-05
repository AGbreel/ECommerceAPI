using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceAPI.Repositories;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repo;

        public CategoriesController(ICategoryRepository repo)
        {
            _repo = repo;
        }

        // ✅ Get All Categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _repo.GetAllCategories();
                return Ok(new { Success = true, Data = categories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Get Category By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _repo.GetCategoryById(id);
                if (category == null)
                    return NotFound(new { Success = false, Message = "Category not found" });

                return Ok(new { Success = true, Data = category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Create Category
        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (category == null)
                return BadRequest(new { Success = false, Message = "Invalid category data" });

            try
            {
                await _repo.CreateCategory(category);
                return Ok(new { Success = true, Message = "Category created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Update Category
        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category dto)
        {
            if (dto == null)
                return BadRequest(new { Success = false, Message = "Invalid category data" });

            try
            {
                var existing = await _repo.GetCategoryById(id);
                if (existing == null)
                    return NotFound(new { Success = false, Message = "Category not found" });

                existing.Name = dto.Name;
                existing.Description = dto.Description;
                existing.ImageUrl = dto.ImageUrl;

                var success = await _repo.UpdateCategory(existing);
                if (!success)
                    return BadRequest(new { Success = false, Message = "Update failed" });

                return Ok(new { Success = true, Message = "Category updated successfully", Data = existing });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Delete Category
        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _repo.DeleteCategory(id);
                if (!success)
                    return NotFound(new { Success = false, Message = "Category not found" });

                return Ok(new { Success = true, Message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}
