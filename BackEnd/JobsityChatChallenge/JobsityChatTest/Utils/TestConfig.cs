using AutoMapper;
using Chat.Core.AppContext.Repository;
using Chat.Core.Auth;
using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.IService;
using Chat.Core.TokenModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobsityChatTest.Utils
{
    public static class TestConfig
    {
        public static readonly Mock<IAuthService> _authServiceMock = new Mock<IAuthService>();
        public static readonly Mock<UserManager<ApplicationUser>> _userManageMock = new Mock<UserManager<ApplicationUser>>(
                                                                   new Mock<IUserStore<ApplicationUser>>().Object,
                                                                   new Mock<IOptions<IdentityOptions>>().Object,
                                                                   new Mock<IPasswordHasher<ApplicationUser>>().Object,
                                                                   new IUserValidator<ApplicationUser>[0],
                                                                   new IPasswordValidator<ApplicationUser>[0],
                                                                   new Mock<ILookupNormalizer>().Object,
                                                                   new Mock<IdentityErrorDescriber>().Object,
                                                                   new Mock<IServiceProvider>().Object,
                                                                   new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        public static readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        public static readonly Mock<ITokenService> _tokenServiceMock = new Mock<ITokenService>();
        public static readonly Mock<IChatRoomService> _chatRoomServiceMock = new Mock<IChatRoomService>();
        public static readonly Mock<IRepository<ChatRoom>> _chatRoomRepository = new Mock<IRepository<ChatRoom>>();
        public static readonly Mock<IRepository<UsersChatRoom>> _usersChatRoomRepository = new Mock<IRepository<UsersChatRoom>>();
        public static readonly Mock<IRepository<Message>> _messageRepository = new Mock<IRepository<Message>>();
        public static readonly LoginDto LOGIN_DTO = new LoginDto
        {
            UserName = "Test1",
            Password = "Test1"
        };

        public static readonly ApplicationUser APPLICATION_USER = new ApplicationUser
        {
            UserName = "Test1"
        };


        public static readonly JsonTokenDto JSON_TOKEN_DTO = new JsonTokenDto
        {
            Token = "Test1"
        };

        public static readonly NewUserRequestDto NEW_USER_REQUEST = new NewUserRequestDto
        {
            Email = "Test",
            Password = "Test",
            FirstName = "Test",
            LastName = "Test",
        };

        public static readonly NewUserResponseDto NEW_USER_RESPONSE = new NewUserResponseDto
        {
            Message = "Test",
            UserId = 0,
            UserName = "Test",
            HasError = false,
        };

        public static readonly IdentityResult IDENTITY_RESULT_SUCCESS = IdentityResult.Success;
        public static readonly ChatRoom CHAT_ROOM = new ChatRoom
        {
            Id = 0,
            Name = "Test"
        };
        public static readonly List<ChatRoom> CHAT_ROOM_LIST = new List<ChatRoom>
        {
            new ChatRoom{
                Id = 0,
                Name = "Test"
            },
            new ChatRoom{
                Id = 0,
                Name = "Test"
            }
        };
        public static readonly IQueryable<ChatRoom> CHAT_ROOM_QUERYABLE_LIST = CHAT_ROOM_LIST.AsQueryable();
        public static readonly BaseDto BASE_DTO = new BaseDto
        {
            HasError = false,
            Message = "Test"
        };

        public static readonly List<UsersChatRoom> USER_CHAT_ROOM_LIST = new List<UsersChatRoom>
        {
            new UsersChatRoom{
                ApplicationUser = null,
                Chatroom = null,
                ApplicationUserId=0,
                ChatroomId=0,
                Id=0
            },
            new UsersChatRoom{
                ApplicationUser = null,
                Chatroom = null,
                ApplicationUserId=0,
                ChatroomId=0,
                Id=0
            }
        };

        public static readonly UsersChatRoom USER_CHAT_ROOM = new UsersChatRoom
        {
            ApplicationUser = null,
            Chatroom = null,
            ApplicationUserId = 0,
            ChatroomId = 0,
            Id = 0
        };

        public static readonly IQueryable<UsersChatRoom> USER_CHAT_ROOM_IQUERYABLE = USER_CHAT_ROOM_LIST.AsQueryable();


        public static readonly List<Message> MESSAGES_LIST = new List<Message>
        {
            new Message{
                
            },
            new Message{
                
            }
        };

        public static readonly List<MessageDto> MESSAGES_DTO_LIST = new List<MessageDto>
        {
            new MessageDto{

            },
            new MessageDto{

            }
        };
        public static readonly IQueryable<Message> MESSAGE_IQUERYABLE = MESSAGES_LIST.AsQueryable();

    }
}
