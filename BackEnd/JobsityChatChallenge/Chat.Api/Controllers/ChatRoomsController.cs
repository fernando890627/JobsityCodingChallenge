using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.ActionFilters;
using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.IService;
using Chat.Core.TokenModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace Chat.Api.Controllers
{
    [Route("api/[Controller]")]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IChatRoomService _chatRooms;        
        public ChatRoomsController(IChatRoomService chatRooms)
        {
            this._chatRooms = chatRooms;
        }
        // GET: Auth
        [HttpGet("GetChatRooms")]
        [ValidateModel]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(List<ChatRoom>))]
        public async Task<IActionResult> GetChatRooms()
        {            
            var result = await _chatRooms.GetChatRooms();
            return Ok(result);
        }

        [HttpPost("Create")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(bool))]
        public async Task<IActionResult> Create([FromBody] ChatRoom data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _chatRooms.Create(data);
            return Ok(result);
        }

        [HttpPost("JoinToChat")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(bool))]
        public async Task<IActionResult> JoinToChat([FromBody] UsersChatRoom data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userName = User.Identity.Name;
            var result = await _chatRooms.JoinToChat(data.ChatroomId, userName);
            return Ok(result);
        }

        [HttpPost("LeaveChat")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(bool))]
        public async Task<IActionResult> LeaveChat([FromBody] UsersChatRoom data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userName = User.Identity.Name;
            var result = await _chatRooms.LeaveChat(data.ChatroomId, userName);
            return Ok(result);
        }

        [HttpGet("GetChatRoomMessage")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(MessageDto))]
        public async Task<IActionResult> GetChatRoomMessage(long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _chatRooms.GetChatRoomMessage(id);
            return Ok(result);
        }
    }
}