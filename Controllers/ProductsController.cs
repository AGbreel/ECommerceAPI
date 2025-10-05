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
            try
            {
                var products = await _repo.GetAllProducts();
                return Ok(new { Success = true, Data = products });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Get Product By Id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id <= 0) return BadRequest(new { Success = false, Message = "Invalid product ID" });

                var product = await _repo.GetProductById(id);
                if (product == null) return NotFound(new { Success = false, Message = "Product not found" });

                return Ok(new { Success = true, Data = product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Get Products By Category
        [HttpGet("ByCategory/{categoryId:int}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            try
            {
                if (categoryId <= 0) return BadRequest(new { Success = false, Message = "Invalid category ID" });

                var products = await _repo.GetProductsByCategory(categoryId);

                return Ok(new { Success = true, Data = products ?? new List<Product>() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Create Product (Admin Only)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { Success = false, Message = "Invalid product data" });

            try
            {
                await _repo.CreateProduct(product);
                return Ok(new { Success = true, Message = "Product created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Update Product (Admin Only)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            try
            {
                if (id <= 0 || product == null)
                    return BadRequest(new { Success = false, Message = "Invalid product data" });

                product.ProductId = id;
                var updatedProduct = await _repo.UpdateProduct(product);

                if (updatedProduct == null)
                    return NotFound(new { Success = false, Message = "Product not found" });

                return Ok(new
                {
                    Success = true,
                    Message = "Product updated successfully",
                    Data = updatedProduct
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }


        // ✅ Delete Product (Admin Only)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) return BadRequest(new { Success = false, Message = "Invalid product ID" });

                var success = await _repo.DeleteProduct(id);
                if (!success) return NotFound(new { Success = false, Message = "Product not found" });

                return Ok(new { Success = true, Message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddImage(int id, [FromBody] string imageUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imageUrl))
                    return BadRequest(new { Success = false, Message = "Image URL is required" });

                await _repo.AddProductImage(id, imageUrl);
                return Ok(new { Success = true, Message = "Image added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}
