using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shared.DTOs;

namespace Api.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        public ChatController() { }

        [Authorize]
        [HttpPost("{chatId}/send")]
        public async Task<IActionResult> SendMessage(string chatId)
        {
            return Ok();
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
