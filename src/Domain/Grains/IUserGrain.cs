using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOs;

namespace Domain.Grains
{
    public interface IUserGrain : IGrainWithStringKey
    {
        Task SetProfileAsync(RegisterDto dto);
        Task<UserProfileDto> GetProfileAsync();
        Task<bool> ValidatePasswordAsync(string candidatePassword);
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    }
}
