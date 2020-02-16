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
        public async Task<StokBotResult> GetStock(string name)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            var strResult = await httpClient
                .GetStringAsync($"https://stooq.com/q/l/?s=" + name + "&f=sd2t2ohlcv&h&e=JSON");
            SymbolsStokBotReslt result = JsonConvert.DeserializeObject<SymbolsStokBotReslt>(strResult);
            return result.Symbols.First();
        }
    }
}
