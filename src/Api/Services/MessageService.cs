using Api.Services.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Shared.DTOs;
using Shared.GrainInterfaces;
using System.Collections.Concurrent;

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
            var chatMessageMemoryGrain = _clusterClient.GetGrain<IChatMessageHistoryGrain>(chatId);
            var messageIds = await chatMessageMemoryGrain.GetMessageIdsAsync();
            if (!messageIds.Any()) return new List<DisplayMessageDto>();
            var normalizedMessageIds = messageIds.Where(id => !string.IsNullOrWhiteSpace(id))
                                         .Select(id => id.Trim())
                                         .Distinct(StringComparer.OrdinalIgnoreCase)
                                         .ToList();

            var succeded = new ConcurrentBag<string>();

            var tasks = normalizedMessageIds.Select(m => 
                TryGetMessageWithRetriesAsync(m, chatId, succeded)).ToList();

            var messages = await Task.WhenAll(tasks);

            return messages.ToList();
        }

        public async Task<DisplayMessageDto> TryGetMessageWithRetriesAsync(string messageId, string chatRoomId, ConcurrentBag<string> succeded)
        {
            var messageGrain = _clusterClient.GetGrain<IMessageGrain>(messageId);
            int maxAttempts = 2;

            for (int attempt = 1; attempt < maxAttempts; attempt++)
            {
                try
                {
                    var message = await messageGrain.GetMessageAsync();
                    succeded.Add(messageId);
                    return MapMessageDto(message, chatRoomId);
                }
                catch (Exception) when (attempt < maxAttempts)
                {
                    await Task.Delay(100 * attempt);
                }
            }

            throw new Exception($"Failed to get info about {messageId}");
        }

        private static DisplayMessageDto MapMessageDto(DisplayMessageDto dto, string chatRoomId) =>
            new DisplayMessageDto(dto.SenderId, dto.Text, chatRoomId);

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
