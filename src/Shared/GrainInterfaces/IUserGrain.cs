using Shared.DTOs;

namespace Shared.GrainInterfaces
{
    public interface IUserGrain : IGrainWithStringKey
    {
        Task SetProfileAsync(RegisterDto dto);
        Task<UserProfileDto> GetProfileAsync();
        Task<bool> ValidatePasswordAsync(string candidatePassword);
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    }
}
