using Shared.DTOs;

namespace Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    }
}
