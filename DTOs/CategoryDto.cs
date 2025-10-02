using Microsoft.AspNetCore.Http;

namespace ECommerceAPI.DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
