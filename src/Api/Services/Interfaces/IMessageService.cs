using Shared.DTOs;

namespace Api.Services.Interfaces
{
    public interface IMessageService
    {
        Task<bool> SendMessageAsync(RegisterMessageDTO dto);
        Task<List<DisplayMessageDto>> GetChatMessagesAsync(string chatId);
        Task<bool> DeleteMessageAsync(string chatId, string messageId);
        Task<DisplayMessageDto> GetMessageByIdAsync(string messageId);
    }
}
