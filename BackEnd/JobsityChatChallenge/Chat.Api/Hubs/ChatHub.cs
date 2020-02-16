using Chat.Core.AppContext.Repository;
using Chat.Core.Entity;
using Chat.StockBot.IStockBot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Api.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly IStockBotService _stockBotService;
        private readonly IRepository<Message> _messageRepository;
        protected readonly UserManager<ApplicationUser> _userManager;
        public ChatHub(IStockBotService stockBotService,
            IRepository<Message> repository,
             UserManager<ApplicationUser> userManager
            )
        {
            this._stockBotService = stockBotService;
            this._messageRepository = repository;
            this._userManager = userManager;
        }
        public async Task Send(long chatRoomId, string messageBody, string unique_name)
        {
            var text = messageBody.ToLower();
            var user = await _userManager.FindByEmailAsync(unique_name);
            var message = new Message
            {
                ChatroomId = chatRoomId,
                MessageSent = messageBody,
                SendDate = DateTime.Now,
                SenderUserId = user!=null?user.Id:0

            };
            if (text.Contains("/stock_code="))
            {
                text = text.Replace("/stock_code=", "");
                var response = await _stockBotService.GetStock(text.ToString());
                if (response != null)
                {
                    message.MessageSent = response.Symbol.ToUpper() + " quote is $" + response.Open + " per share.";
                    await Clients.All.SendAsync(chatRoomId.ToString(), "Bot_Stock", message);
                    await Clients.Group(message.ChatroomId.ToString()).SendAsync("Bot_Stock", messageBody);
                }
                else
                {
                    message.MessageSent = text.ToString() + " was not found";
                    await Clients.All.SendAsync(chatRoomId.ToString(), "Bot_Stock", message);
                    await Clients.Group(message.ChatroomId.ToString()).SendAsync("OnMetadataMessage", messageBody);
                }
            }
            else
            {
                _messageRepository.Add(message);
                await Clients.All.SendAsync(chatRoomId.ToString(), user?.UserName, message);
                await Clients.Group(message.ChatroomId.ToString()).SendAsync($"User_{unique_name}", unique_name, messageBody);
            }
        }

        //public async Task Send(string name, string message)
        //{
        //    // Call the broadcastMessage method to update clients.
        //    await Clients.All.SendAsync("broadcastMessage", name, message);
        //}
        public Task JoinRoom(string chatroomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, chatroomId);
        }
        public Task LeaveRoom(string chatroomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, chatroomId);
        }
    }
}
