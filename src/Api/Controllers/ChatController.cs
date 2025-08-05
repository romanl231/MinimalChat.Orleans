using Api.Services.Interfaces;
using Api.Utils;
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
        private readonly IChatRoomService _chatRoomService;

        public ChatRoomController(IChatRoomService chatRoomService) 
        {
            _chatRoomService = chatRoomService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateChatRoom([FromBody]ChatDTO dto)
        {
            var userId = UserHelper.GetCurrentUserId(HttpContext);
            var mappedDto = Map(dto, userId);
            if (await _chatRoomService.CreateRoomAsync(dto)) return Ok();
            return BadRequest();
        }

        private ChatDTO Map(ChatDTO dto, string userId) => 
            new ChatDTO(dto.Title, dto.Description, userId, dto.MemberIds);
        
        [Authorize]
        [HttpPost("{chatId}/join")]
        public async Task<IActionResult> JoinChatRoom(string chatId, [FromBody] string memberId) => 
            await _chatRoomService.AddMemberAsync(chatId, memberId) ? 
            Ok() : BadRequest();

        [Authorize]
        [HttpGet("get-my-chats")]
        public async Task<ActionResult<List<string>>> GetMyChats()
        {
            var userId = UserHelper.GetCurrentUserId(HttpContext);
            var result = await _chatRoomService.GetMemberChatIds(userId);

            if(!result.Any())
                return BadRequest();

            return Ok(result);
        }
    }
}
