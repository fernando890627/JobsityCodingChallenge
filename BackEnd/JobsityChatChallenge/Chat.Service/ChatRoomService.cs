using Chat.Core.AppContext.Repository;
using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service
{
    public class ChatRoomService: IChatRoomService
    {
        private readonly IRepository<ChatRoom> _repository;

        public ChatRoomService(IRepository<ChatRoom> repository)
        {
            this._repository = repository;
        }
        public async Task<bool> Create(ChatRoom data)
        {
            _repository.Add(data);
            return true;
        }
        public async Task<List<ChatRoom>> GetChatRooms()
        {
            return  _repository.GetAll().ToList();
        }
    }
}
