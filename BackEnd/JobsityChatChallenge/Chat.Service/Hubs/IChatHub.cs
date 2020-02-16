using Chat.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Hubs
{
    public interface IChatHub
    {
        Task Send(Message message);
    }
}
