using System;

namespace ECommerceAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; }
    }
}

