using Chat.Core.AppContext.Repository;
using Chat.Core.Entity;
using Chat.StockBot.IStockBot;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Hubs
{
    public class ChatHub:Hub, IChatHub
    {
        private readonly IStockBotService _stockBotService;
        private readonly IRepository<Message> _messageRepository;

        public ChatHub(IStockBotService stockBotService,IRepository<Message> repository)
        {
            this._stockBotService = stockBotService;
            this._messageRepository = repository;
        }
        public async Task Send(Message message)
        {
            var text = message.MessageSent.ToLower();
            var messageText = string.Empty;
            if (text.Contains("/stock_code="))
            {
                text = text.Replace("/stock_code=", "");
                var response = await _stockBotService.GetStock(text.ToString());
                if (response != null)
                {
                    messageText = response.Symbol.ToUpper() + " quote is $" + response.Open + " per share.";
                    await Clients.Group(message.ChatroomId.ToString()).SendAsync("Send", message);
                }
                else
                {
                    message.MessageSent = text.ToString() + " was not found";
                    await Clients.Group(message.ChatroomId.ToString()).SendAsync("OnMetadataMessage", message);
                }
            }
            else
            {
                var sender = _messageRepository.Add(message);
                await Clients.Group(message.ChatroomId.ToString()).SendAsync("Send", message);
            }
        }
    }
}
