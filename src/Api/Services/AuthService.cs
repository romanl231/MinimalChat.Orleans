using Api.Services.Interfaces;
using Shared.DTOs;
using Shared.GrainInterfaces;

namespace Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClusterClient _client;

        public AuthService(IClusterClient client) 
        {
            _client = client;
        }

        private static string Normalize(string nickName) => nickName.Trim().ToLowerInvariant();

        private IUserDirectoryGrain ResolveNickNameGrain(string nickName)
        {
            var nickNameKey = Normalize(nickName);
            var nickNameGrain = _client.GetGrain<IUserDirectoryGrain>(nickNameKey);
            return nickNameGrain;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var userId = Guid.NewGuid().ToString();
            var userGrain = _client.GetGrain<IUserGrain>(userId);
            var nicknameGrain = ResolveNickNameGrain(dto.Nickname);
            var claimResult = await nicknameGrain.TryClaim(userId);

            if (claimResult)
            {
                await userGrain.SetProfileAsync(dto);
            }
            
            return claimResult; 
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var nickNameGrain = ResolveNickNameGrain(dto.Nickname);
            var userId = await nickNameGrain.Resolve();

            if (userId == null) return string.Empty;

            var userGrain = _client.GetGrain<IUserGrain>(userId);

            if (await userGrain.ValidatePasswordAsync(dto.Password))
            {
                return userId;
            }

            return string.Empty;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var nickNameGrain = ResolveNickNameGrain(dto.Nickname);
            var userId = await nickNameGrain.Resolve();

            if (userId == null) return false;

            var userGrain = _client.GetGrain<IUserGrain>(userId);
            var result = await userGrain.ChangePasswordAsync(dto.CurrentPassword, dto.NewPassword);

            return result;
        }
    }
}
