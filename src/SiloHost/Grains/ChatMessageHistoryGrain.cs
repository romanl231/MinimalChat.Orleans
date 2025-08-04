using Shared.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiloHost.Grains
{
    public class ChatMessageHistoryGrainState
    {
        public List<string> MessageIds { get; set; } = new List<string>();
    }

    public class ChatMessageHistoryGrain : Grain<ChatMessageHistoryGrainState>, IChatMessageHistoryGrain
    {
        public async Task<bool> AddMessageAsync(string messageId)
        {
            if (!State.MessageIds.Contains(messageId))
            {
                State.MessageIds.Add(messageId);
                await WriteStateAsync();
            }
            return true;
        }

        public Task<List<string>> GetMessageIdsAsync() => 
            Task.FromResult(new List<string>(State.MessageIds));

        public async Task<bool> RemoveMessageAsync(string messageId)
        {
            State.MessageIds.Remove(messageId);
            await WriteStateAsync();
            return true;
        }
    }
}
