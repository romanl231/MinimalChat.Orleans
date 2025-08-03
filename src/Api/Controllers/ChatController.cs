using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shared.DTOs;

namespace Api.Controllers
{
    [Route("api/chatroom")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public ChatRoomController(IClusterClient clusterClient) 
        {
            _clusterClient = clusterClient;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChatRoom(ChatMessage dto)
        {
            return Ok();
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetChatMessages()
        {
            return Ok();
        }

        [HttpPost("{chatId}/join")]
        public async Task<IActionResult> JoinChatRoom()
        {
            return Ok();
        }

        [HttpGet("get-my-chats")]
        public async Task<IActionResult> GetMyChats()
        {
            return Ok();
        }
    }
}
