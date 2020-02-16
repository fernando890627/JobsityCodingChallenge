using Chat.StockBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.StockBot.IStockBot
{
    public interface IStockBotService
    {
        Task<StokBotResult> GetStock(string stockName);
    }
}
