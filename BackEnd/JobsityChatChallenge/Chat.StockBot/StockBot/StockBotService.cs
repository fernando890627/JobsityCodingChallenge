using Chat.StockBot.DTO;
using Chat.StockBot.IStockBot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Chat.StockBot.StockBot
{
    public class StockBotService: IStockBotService
    {
        public async Task<StokBotResult> GetStock(string stockName)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringResult = await client
                .GetStringAsync($"https://stooq.com/q/l/?s=" + stockName + "&f=sd2t2ohlcv&h&e=JSON");

            SymbolsStokBotReslt result = JsonConvert.DeserializeObject<SymbolsStokBotReslt>(stringResult);

            return result.Symbols.First();
        }
    }
}
