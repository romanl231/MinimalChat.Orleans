using Shared.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiloHost.Grains
{
    public class UserChatMembershipGrainState
    {
        public List<string> ChatIds {  get; set; } = new List<string>();
    }

    public class UserChatMembershipGrain : Grain<UserChatMembershipGrainState>, IUserChatMembershipGrain
    {
        public async Task<bool> AddChatAsync(string chatId)
        {
            State.ChatIds.Add(chatId);
            await WriteStateAsync();
            return true;
        }

        public async Task<bool> RemoveChatAsync(string chatId)
        {
            State.ChatIds.Remove(chatId);
            await WriteStateAsync();
            return true;
        }

        public Task<List<string>> GetChatIdsAsync()
        {
            var chatIds = State.ChatIds;
            return Task.FromResult(chatIds);
        }
    }
}
