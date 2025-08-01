using Domain.Grains;
using Grains.Services;
using Orleans;
using Shared.DTOs;
using System.Security.Cryptography;


namespace Grains
{
    public class UserGrainState
    {
        public string NickName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class UserGrain : Grain<UserGrainState>, IUserGrain
    {
        public async Task SetProfileAsync(RegisterDto dto)
        {
            State.NickName = dto.Nickname;
            State.Description = dto.Description;
            State.PasswordHash = PasswordHasher.Hash(dto.Password);
            await WriteStateAsync();
        }

        public Task<UserProfileDto> GetProfileAsync()
        {
            var dto = new UserProfileDto(State.NickName, State.Description);
            return Task.FromResult(dto);
        }

        public Task<bool> ValidatePasswordAsync(string candidatePassword)
        {
            var result = PasswordHasher.Verify(candidatePassword, State.PasswordHash);
            return Task.FromResult(result);
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var valid = PasswordHasher.Verify(currentPassword, State.PasswordHash);

            if (valid)
            {
                State.PasswordHash = PasswordHasher.Hash(newPassword);
                await WriteStateAsync();
            }

            return valid;
        }
    }
}
