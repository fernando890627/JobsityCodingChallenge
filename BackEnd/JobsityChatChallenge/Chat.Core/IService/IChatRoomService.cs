using Chat.Core.DTO;
using Chat.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.IService
{
    public interface IChatRoomService
    {
        Task<bool> Create(ChatRoom data);
        Task<List<ChatRoom>> GetChatRooms();
        Task<BaseDto> JoinToChat(long chatId, string userName);
        Task<BaseDto> LeaveChat(long chatId, string userName);
        Task<List<MessageDto>> GetChatRoomMessage(long id);
    }
}
