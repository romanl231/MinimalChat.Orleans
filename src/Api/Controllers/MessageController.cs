using Api.Services.Interfaces;
using Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/chatroom/{chatRoomId}")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Authorize]
        [HttpPost("message/send")]
        public async Task<IActionResult> SendMessageAsync([FromBody] string text, string chatRoomId)
        {
            var userId = UserHelper.GetCurrentUserId(HttpContext);
            var dto = MapMessageDto(userId, chatRoomId, text);
            if (await _messageService.SendMessageAsync(dto)) return Ok();
            return BadRequest();
        }

        private RegisterMessageDTO MapMessageDto(string senderId, string chatId, string text) =>
            new RegisterMessageDTO(senderId, chatId, text);

        [Authorize]
        [HttpGet("message/all")]
        public async Task<IActionResult> GetMessagesFromChat(string chatRoomId)
        {
            var messages = await _messageService.GetChatMessagesAsync(chatRoomId);
            if(messages.Any()) return Ok(messages);
            return BadRequest();
        }

        [Authorize]
        [HttpGet("message/{messageId}")]
        public async Task<IActionResult> GetMessageById(string messageId)
        {
            var message = await _messageService.GetMessageByIdAsync(messageId);
            if(message == null) return BadRequest();
            return Ok(message);
        }
    }
}
