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



// using System;
// using ECommerce.Models;
// using Microsoft.AspNetCore.Identity;

// namespace ECommerceAPI.Models
// {
//     public class User : IdentityUser<int>
//     {
//         public DateTime CreatedAt { get; set; }
//         public Order Order { get; set; }
//         public int OrderId { get; set; }
//     }
// }
