using Chat.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Api.Hubs
{
    public interface IChatHub
    {
        Task Send(long chatRoomId, string messageBody, string unique_name);
        //Task Send(string name, string message);
    }
}
