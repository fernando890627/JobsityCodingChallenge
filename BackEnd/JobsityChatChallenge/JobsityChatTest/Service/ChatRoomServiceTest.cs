using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.TokenModels;
using Chat.Service;
using JobsityChatTest.Utils;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobsityChatTest.Service
{
    public class ChatRoomServiceTest
    {

        [Fact]
        public async Task Create()
        {
            var _repository = TestConfig._chatRoomRepository;
            _repository.Setup(x => x.Add(It.IsAny<ChatRoom>())).Returns(TestConfig.CHAT_ROOM);
            var service = new ChatRoomService(_repository.Object,
                TestConfig._usersChatRoomRepository.Object,
                TestConfig._messageRepository.Object,
                TestConfig._userManageMock.Object);

            var valueResult = service.Create(TestConfig.CHAT_ROOM);
            Assert.IsType<bool>(valueResult.Result);
        }
        [Fact]
        public async Task GetChatRooms()
        {
            var _repository = TestConfig._chatRoomRepository;
            _repository.Setup(x => x.GetAll()).Returns(TestConfig.CHAT_ROOM_QUERYABLE_LIST);
            var service = new ChatRoomService(_repository.Object,
                TestConfig._usersChatRoomRepository.Object,
                TestConfig._messageRepository.Object,
                TestConfig._userManageMock.Object);
            var valueResult = service.GetChatRooms();
            Assert.IsType<List<ChatRoom>>(valueResult.Result);
        }

        [Fact]
        public async Task JoinToChat()
        {
            var _repository = TestConfig._usersChatRoomRepository;
            _repository.Setup(x => x.Get(c => c.ChatroomId == It.IsAny<long>()
                                            && c.ApplicationUserId == It.IsAny<long>(), null, null)).Returns(TestConfig.USER_CHAT_ROOM_IQUERYABLE);

            var _userManager = TestConfig._userManageMock;
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(TestConfig.APPLICATION_USER);
            _repository.Setup(x => x.Add(It.IsAny<UsersChatRoom>())).Returns(TestConfig.USER_CHAT_ROOM);
            var service = new ChatRoomService(TestConfig._chatRoomRepository.Object,
                _repository.Object,
                TestConfig._messageRepository.Object,
                _userManager.Object);
            var valueResult = service.JoinToChat(It.IsAny<long>(), It.IsAny<string>());
            Assert.IsType<BaseDto>(valueResult.Result);
            Assert.False(valueResult.Result.HasError);
        }

        [Fact]
        public async Task GetChatRoomMessage()
        {
            var _repository = TestConfig._messageRepository;
            _repository.Setup(x => x.Get(null, null, null)).Returns(TestConfig.MESSAGE_IQUERYABLE);
            var service = new ChatRoomService(TestConfig._chatRoomRepository.Object,
                TestConfig._usersChatRoomRepository.Object,
                _repository.Object,
                TestConfig._userManageMock.Object);
            var valueResult = service.GetChatRoomMessage(It.IsAny<long>());
            Assert.IsType<List<MessageDto>>(valueResult.Result);
        }
    }
}
