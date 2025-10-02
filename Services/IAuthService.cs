using ECommerceAPI.DTOs;
using System.Threading.Tasks;

namespace ECommerceAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto dto);
        Task<AuthResponseDto?> Login(LoginDto dto);
    }
}
