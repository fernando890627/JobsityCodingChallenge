using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.Core.Entity
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public long ChatroomId { get; set; }

        public string MessageSent { get; set; }

        public long SenderUserId { get; set; }

        public DateTime SendDate { get; set; }

        public virtual ApplicationUser SenderUser { get; set; }

        public virtual ChatRoom Chatroom { get; set; }
    }
}
