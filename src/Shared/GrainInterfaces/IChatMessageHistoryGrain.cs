using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GrainInterfaces
{
    public interface IChatMessageHistoryGrain : IGrainWithStringKey
    {
        Task<bool> AddMessageAsync(string messageId);
        Task<List<string>> GetMessageIdsAsync();
        Task<bool> RemoveMessageAsync(string messageId);
    }
}
