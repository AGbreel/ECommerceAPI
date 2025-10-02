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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repo.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _repo.GetCategoryById(id);
            if (category == null) return NotFound("Category not found");
            return Ok(category);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            await _repo.CreateCategory(category);
            return Ok(new { Message = "Category created successfully" });
        }

        // [Authorize(Roles = "Admin")]
        // x: Controllers/CategoriesController.cs
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category dto)
        {
            var existing = await _repo.GetCategoryById(id);
            if (existing == null) return NotFound("Category not found");

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.ImageUrl = dto.ImageUrl; // لو بعت لينك صورة جديد

            var success = await _repo.UpdateCategory(existing);
            if (!success) return BadRequest("Update failed");

            return Ok(new { Message = "Category updated successfully", existing });
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repo.DeleteCategory(id);
            if (!success) return NotFound("Category not found");
            return Ok(new { Message = "Category deleted successfully" });
        }
    }
}
