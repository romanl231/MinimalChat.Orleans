using Shared.DTOs;
using Shared.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SiloHost.Grains
{
    public class MessageGrainState
    {
        public string SenderId { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    public class MessageGrain : Grain<MessageGrainState>, IMessageGrain
    {
        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            return base.OnActivateAsync(cancellationToken);
        }

        public async Task<bool> SetMessageAsync(RegisterMessageDTO dto)
        {
            State.SenderId = dto.SenderId;
            State.Text = dto.Text;
            await WriteStateAsync();
            return true;
        }

        public Task<DisplayMessageDto> GetMessageAsync()
        {
            return Task.FromResult(MapMessageToDto());
        }

        private DisplayMessageDto MapMessageToDto() => 
            new DisplayMessageDto(
                State.SenderId, 
                State.Text
                );

        public async Task<bool> DeleteMessageAsync()
        {
            State.SenderId = string.Empty;
            State.Text = string.Empty;
            await WriteStateAsync();
            return true;
        }
    }
}
