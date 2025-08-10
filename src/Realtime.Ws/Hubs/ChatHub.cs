using Microsoft.AspNetCore.SignalR;
using Shared.DTOs;

namespace Realtime.Ws.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinRoom(string chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        }

        public async Task LeaveRoom(string chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        }

        public async Task SendMessage(DisplayMessageDto messageDto)
        {
            await Clients.Group(messageDto.ChatRoomId)
                         .SendAsync("ReceiveMessage", messageDto.SenderId, messageDto.Text);
        }
    }
}
