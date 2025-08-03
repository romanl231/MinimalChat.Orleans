using Shared.DTOs;

namespace Api.Services.Interfaces
{
    public interface IChatRoomService
    {
        Task<bool> CreateRoomAsync(ChatDTO dto);
        Task<bool> AddMemberAsync(string chatId, string memberId);
        Task<bool> RemoveMemberAsync(string chatId, string memberId);
        Task<List<string>> GetUserChatIds(string userId);
        Task<List<string>> GetMemberChatIds(string chatId);
    }
}
