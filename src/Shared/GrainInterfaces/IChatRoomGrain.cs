using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOs;

namespace Shared.GrainInterfaces
{
    public interface IChatRoomGrain : IGrainWithStringKey
    {
        Task SetRoomAsync(ChatDTO dto);
        Task<ChatDTO> GetRoomAsync();
        Task<bool> AddMemberAsync(string userId);
        Task<bool> RemoveMemberAsync(string userId);
        Task<List<string>> GetMembersAsync();
        Task<bool> DeleteRoomAsync();
    }
}
