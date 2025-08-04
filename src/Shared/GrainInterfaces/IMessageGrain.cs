using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GrainInterfaces
{
    public interface IMessageGrain : IGrainWithStringKey 
    {
        Task<bool> SetMessageAsync(RegisterMessageDTO dto);
        Task<DisplayMessageDto> GetMessageAsync();
        Task<bool> DeleteMessageAsync();
    }
}
