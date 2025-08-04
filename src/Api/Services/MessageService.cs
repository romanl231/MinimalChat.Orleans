using Api.Services.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Shared.DTOs;
using Shared.GrainInterfaces;

namespace Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IClusterClient _clusterClient;

        public MessageService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public async Task<bool> SendMessageAsync(RegisterMessageDTO dto)
        {
            var messageId = Guid.NewGuid().ToString();
            var messageGrain = _clusterClient.GetGrain<IMessageGrain>(messageId);
            if(!await messageGrain.SetMessageAsync(dto))
            {
                return false;
            }

            var chatMessageHistoryGrain = _clusterClient.GetGrain<IChatMessageHistoryGrain>(dto.ChatRoomId);
            if(!await chatMessageHistoryGrain.AddMessageAsync(messageId))
            {
                await messageGrain.DeleteMessageAsync();
                return false;
            }

            return true;
        }

        public async Task<List<DisplayMessageDto>> GetChatMessagesAsync(string chatId)
        {
            return new List<DisplayMessageDto>();
        }

        public async Task<bool> DeleteMessageAsync(string chatRoomId, string messageId)
        {
            var chatHistoryGrain = _clusterClient.GetGrain<IChatMessageHistoryGrain>(chatRoomId);
            if (await chatHistoryGrain.RemoveMessageAsync(messageId))
            {
                return false;
            }

            var messageGrain = _clusterClient.GetGrain<IMessageGrain>(messageId);
            if(!await messageGrain.DeleteMessageAsync())
            {
                await chatHistoryGrain.AddMessageAsync(messageId);
                return false;
            }

            return true;
        }

        public async Task<DisplayMessageDto> GetMessageByIdAsync(string messageId)
        {
            var messageGrain = _clusterClient.GetGrain<IMessageGrain>(messageId);
            return await messageGrain.GetMessageAsync();
        }
    }
}
