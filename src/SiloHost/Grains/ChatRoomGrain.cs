using Shared.DTOs;
using Shared.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiloHost.Grains
{
    public class ChatRoomGrainState 
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
        public List<string> MemberIds { get; set; } = new List<string>();
    }

    public class ChatRoomGrain : Grain<ChatRoomGrainState>, IChatRoomGrain
    {
        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            return base.OnActivateAsync(cancellationToken);
        }

        public async Task SetRoomAsync(ChatDTO dto)
        {
            State.Title = dto.Title;
            State.Description = dto.Description;
            State.CreatorId = dto.CreatorId;
            State.MemberIds = dto.MemberIds;
            await WriteStateAsync();
        }

        public Task<ChatDTO> GetRoomAsync()
        {
            var dto = MapStateToDTO();
            return Task.FromResult(dto);
        }

        private ChatDTO MapStateToDTO()
        {
            return new ChatDTO(
                State.Title,
                State.Description,
                State.CreatorId,
                State.MemberIds);
        }

        public async Task<bool> AddMemberAsync(string userId)
        {
            try
            {
                State.MemberIds.Add(userId);
                await WriteStateAsync();
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> RemoveMemberAsync(string userId)
        {
            try
            {
                State.MemberIds.Remove(userId);
                await WriteStateAsync();
                return true;
            } catch (Exception) { return false; }
        }

        public Task<List<string>> GetMembersAsync()
        {
            var members = State.MemberIds;
            return Task.FromResult(members);
        }

        public Task<bool> DeleteRoomAsync()
        {
            State.Title = string.Empty;
            State.Description = string.Empty;
            State.CreatorId = string.Empty;
            State.MemberIds = new List<string>();
            return Task.FromResult(true);
        }
    }
}
