using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chat.Core.Entity
{
    public class UsersChatRoom
    {
        [Key]
        public long Id { get; set; }
        public long ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public long ChatroomId { get; set; }
        public ChatRoom Chatroom { get; set; }
    }
}
