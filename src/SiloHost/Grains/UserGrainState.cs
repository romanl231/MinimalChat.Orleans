using Shared.GrainInterfaces;
using SiloHost.Services;
using Shared.DTOs;


namespace SiloHost.Grains
{
    public class UserGrainState
    {
        public string Id { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class UserGrain : Grain<UserGrainState>, IUserGrain
    {
        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            State.Id = this.GetPrimaryKeyString();
            return base.OnActivateAsync(cancellationToken);
        }

        public async Task SetProfileAsync(RegisterDto dto)
        {
            State.Id = Guid.NewGuid().ToString();
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
