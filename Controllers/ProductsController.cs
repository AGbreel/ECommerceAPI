using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceAPI.Repositories;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        // ✅ Get All Products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repo.GetAllProducts();
            return Ok(new { Success = true, Data = products });
        }

        // ✅ Get Product By Id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest(new { Success = false, Message = "Invalid product ID" });

            var product = await _repo.GetProductById(id);
            if (product == null) return NotFound(new { Success = false, Message = "Product not found" });

            return Ok(new { Success = true, Data = product });
        }

        // ✅ Get Products By Category
        [HttpGet("ByCategory/{categoryId:int}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            if (categoryId <= 0) return BadRequest(new { Success = false, Message = "Invalid category ID" });

            var products = await _repo.GetProductsByCategory(categoryId);

            return Ok(new { Success = true, Data = products ?? new List<Product>() });
        }

        // ✅ Create Product (Admin Only)
        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { Success = false, Message = "Invalid product data" });

            await _repo.CreateProduct(product);
            return Ok(new { Success = true, Message = "Product created successfully" });
        }

        // ✅ Update Product (Admin Only)
        // [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (id <= 0 || product == null)
                return BadRequest(new { Success = false, Message = "Invalid product data" });

            product.ProductId = id;
            var success = await _repo.UpdateProduct(product);

            if (!success) return NotFound(new { Success = false, Message = "Product not found" });
            return Ok(new { Success = true, Message = "Product updated successfully" });
        }

        // ✅ Delete Product (Admin Only)
        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest(new { Success = false, Message = "Invalid product ID" });

            var success = await _repo.DeleteProduct(id);
            if (!success) return NotFound(new { Success = false, Message = "Product not found" });

            return Ok(new { Success = true, Message = "Product deleted successfully" });
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddImage(int id, [FromBody] string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return BadRequest("Image URL is required");

            await _repo.AddProductImage(id, imageUrl);
            return Ok(new { Message = "Image added successfully" });
        }

    }
}
