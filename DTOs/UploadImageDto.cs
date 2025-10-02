using Microsoft.AspNetCore.Http;

namespace ECommerceAPI.DTOs
{
    public class UploadImageDto
    {
        public IFormFile File { get; set; }
    }
}
