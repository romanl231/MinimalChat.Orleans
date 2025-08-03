using Api.Services.Interfaces;
using Shared.DTOs;
using Shared.GrainInterfaces;
using System.Collections.Concurrent;

namespace Api.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IClusterClient _clusterClient;

        public ChatRoomService(IClusterClient clusterClient) 
        {
            _clusterClient = clusterClient;
        }

        public async Task<bool> CreateRoomAsync(ChatDTO dto)
        {
            var chatRoomId = Guid.NewGuid().ToString();
            var chatRoomGrain = _clusterClient.GetGrain<IChatRoomGrain>(chatRoomId);
            try
            {
                await chatRoomGrain.SetRoomAsync(dto);
            }
            catch (Exception)
            {
                return false;
            }

            var memberIds = dto.MemberIds?.Where(id => !string.IsNullOrWhiteSpace(id))
                                         .Select(id => id.Trim())
                                         .Distinct(StringComparer.OrdinalIgnoreCase)
                                         .ToList() ?? new List<string>();

            var succeeded = new ConcurrentBag<string>();
            var addTasks = memberIds.Select(id => AddMembershipWithRetriesAsync(id, chatRoomId, succeeded)).ToArray();

            try
            {
                await Task.WhenAll(addTasks);
                return true;
            }
            catch (Exception)
            {
               
                var rollbackTasks = succeeded.Select(async memberId =>
                {
                    try
                    {
                        var membershipGrain = _clusterClient.GetGrain<IUserChatMembershipGrain>(memberId);
                        await membershipGrain.RemoveChatAsync(chatRoomId);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
                await Task.WhenAll(rollbackTasks);

                try
                {
                    await chatRoomGrain.DeleteRoomAsync();
                }
                catch (Exception)
                {
                    throw;
                }

                return false;
            }
        }

        private async Task AddMembershipWithRetriesAsync(string memberId, string chatRoomId, ConcurrentBag<string> succeeded)
        {
            var grain = _clusterClient.GetGrain<IUserChatMembershipGrain>(memberId);
            const int maxAttempts = 2;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    var ok = await grain.AddChatAsync(chatRoomId);
                    if (!ok)
                        throw new InvalidOperationException($"AddChatAsync returned false for {memberId}");

                    succeeded.Add(memberId);
                    return;
                }
                catch (Exception) when (attempt < maxAttempts)
                {
                    await Task.Delay(100 * attempt);
                }
            }

            throw new Exception($"Failed to add membership for {memberId} after retries");
        }

        public async Task<bool> AddMemberAsync(string chatId, string memberId) 
        {
            var chatRoomGrain = _clusterClient.GetGrain<IChatRoomGrain>(chatId);
            if(await chatRoomGrain.AddMemberAsync(memberId))
            {
                var userMembershipGrain = _clusterClient.GetGrain<IUserChatMembershipGrain>(memberId);
                if (!await userMembershipGrain.AddChatAsync(chatId))
                {
                    await chatRoomGrain.RemoveMemberAsync(memberId);
                    return false;
                }
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveMemberAsync(string chatId, string memberId)
        {
            var chatRoomGrain = _clusterClient.GetGrain<IChatRoomGrain>(chatId);
            if(!await chatRoomGrain.RemoveMemberAsync(memberId))
            {
                return false;
            }

            var userMembershipGrain = _clusterClient.GetGrain<IUserChatMembershipGrain>(memberId);
            if(!await userMembershipGrain.RemoveChatAsync(chatId))
            {
                await chatRoomGrain.AddMemberAsync(memberId);
                return false;
            }

            return true;
        }

        public async Task<List<string>> GetUserChatIds(string userId)
        {
            var userMembershipGrain = _clusterClient.GetGrain<IUserChatMembershipGrain>(userId);
            return await userMembershipGrain.GetChatIdsAsync();
        }

        public async Task<List<string>> GetMemberChatIds(string chatId)
        {
            var chatRoomGrain = _clusterClient.GetGrain<IChatRoomGrain>(chatId);
            return await chatRoomGrain.GetMembersAsync();
        }
    }
}
