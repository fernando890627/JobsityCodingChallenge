using Chat.Core.AppContext.Repository;
using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.IService;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IRepository<ChatRoom> _repository;
        private readonly IRepository<UsersChatRoom> _usersChatRoomRepository;
        private readonly IRepository<Message> _messageRepository;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ChatRoomService(IRepository<ChatRoom> repository,
            IRepository<UsersChatRoom> usersChatRoomRepository,
            IRepository<Message> messageRepository,
            UserManager<ApplicationUser> userManager)
        {
            this._repository = repository;
            this._usersChatRoomRepository = usersChatRoomRepository;
            this._messageRepository = messageRepository;
            _userManager = userManager;
        }
        public async Task<bool> Create(ChatRoom data)
        {
            _repository.Add(data);
            return true;
        }
        public async Task<List<ChatRoom>> GetChatRooms()
        {
            return _repository.GetAll().ToList();
        }

        public async Task<BaseDto> JoinToChat(long chatId,string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            var userChat = _usersChatRoomRepository.Get(c => c.ChatroomId == chatId
                                            && c.ApplicationUserId == user.Id)
                                                .FirstOrDefault();
            if (userChat != null)
            {
                return new BaseDto
                {
                    HasError = true,
                    Message = "You have joined to this chat room already before"
                };
            }
            var data = new UsersChatRoom { ChatroomId = chatId, ApplicationUserId = user.Id };
            _usersChatRoomRepository.Add(data);
            return new BaseDto
            {
                HasError = false,
                Message = "You have joined successfuly"
            };
        }

        public async Task<List<MessageDto>> GetChatRoomMessage(long id)
        {
            var chatRoomMessages = _messageRepository.Get(filter: c => c.ChatroomId == id,
                orderBy: o => o.OrderByDescending(or => or.SendDate)               
                ).Take(50).Select(r=>new MessageDto 
                {
                    Message=r.MessageSent,
                    UserName=r.SenderUser.UserName
                });
            return chatRoomMessages.ToList();
        }
    }
}
