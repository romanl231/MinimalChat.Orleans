using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GrainInterfaces
{
    public interface IUserChatMembershipGrain : IGrainWithStringKey
    {
        Task<bool> AddChatAsync(string chatId);
        Task<bool> RemoveChatAsync(string chatId);
        Task<List<string>> GetChatIdsAsync();
    }
}
