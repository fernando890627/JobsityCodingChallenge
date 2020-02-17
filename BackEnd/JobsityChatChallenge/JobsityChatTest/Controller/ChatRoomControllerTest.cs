using Chat.Api.Controllers;
using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.TokenModels;
using Chat.Service;
using JobsityChatTest.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobsityChatTest.Controller
{
    public class ChatRoomControllerTest
    {

        [Fact]
        public async Task GetChatRooms()
        {
            var _chatRoomService = TestConfig._chatRoomServiceMock;
            _chatRoomService.Setup(x => x.GetChatRooms()).ReturnsAsync(TestConfig.CHAT_ROOM_LIST);
            var controller = new ChatRoomsController(_chatRoomService.Object);
            var valueResult = await controller.GetChatRooms();
            Assert.IsType<OkObjectResult>(valueResult);
        }

        [Fact]
        public async Task Create()
        {
            var _chatRoomService = TestConfig._chatRoomServiceMock;
            _chatRoomService.Setup(x => x.Create(It.IsAny<ChatRoom>())).ReturnsAsync(true);
            var controller = new ChatRoomsController(_chatRoomService.Object);
            var valueResult = await controller.Create(null);
            Assert.IsType<OkObjectResult>(valueResult);
        }

        [Fact]
        public async Task JoinToChat()
        {
            var _chatRoomService = TestConfig._chatRoomServiceMock;
            _chatRoomService.Setup(x => x.JoinToChat(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(TestConfig.BASE_DTO);

            var controller = new ChatRoomsController(_chatRoomService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "username")
                    }, "someAuthTypeName"))
                }
            };
            var valueResult = await controller.JoinToChat(TestConfig.USER_CHAT_ROOM);
            Assert.IsType<OkObjectResult>(valueResult);
        }

        [Fact]
        public async Task GetChatRoomMessage()
        {
            var _chatRoomService = TestConfig._chatRoomServiceMock;
            _chatRoomService.Setup(x => x.GetChatRoomMessage(It.IsAny<long>())).ReturnsAsync(TestConfig.MESSAGES_DTO_LIST);
            var controller = new ChatRoomsController(_chatRoomService.Object);
            var valueResult = await controller.GetChatRoomMessage(It.IsAny<long>());
            Assert.IsType<OkObjectResult>(valueResult);
        }
    }
}
