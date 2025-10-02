using Microsoft.Extensions.Configuration;
using ECommerceAPI.Repositories;  // ✅ عشان IUserRepository
using ECommerceAPI.Models;        // ✅ عشان User

public class AdminSeedDto
{
    public string Email { get; set; } = string.Empty;     // ✅ حل الـ warning CS8618
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

public static class DbSeeder
{
    public static async Task SeedAdminsAsync(IServiceProvider provider)
    {
        var config = provider.GetRequiredService<IConfiguration>();
        var admins = config.GetSection("AdminUsers").Get<List<AdminSeedDto>>() ?? new();

        var userRepo = provider.GetRequiredService<IUserRepository>();

        foreach (var a in admins)
        {
            var existing = await userRepo.GetByEmailAsync(a.Email);  // ✅ خلي الاسم يطابق الموجود في UserRepository
            if (existing == null)
            {
                var hash = BCrypt.Net.BCrypt.HashPassword(a.Password);
                var user = new User
                {
                    FullName = string.IsNullOrWhiteSpace(a.FullName) ? a.Email : a.FullName,
                    Email = a.Email,
                    PasswordHash = hash,
                    Role = "Admin"
                };
                await userRepo.CreateUser(user);
            }
        }
    }
}
