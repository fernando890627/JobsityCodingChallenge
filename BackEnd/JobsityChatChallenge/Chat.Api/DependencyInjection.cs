using Chat.Core.AppContext.Repository;
using Chat.Core.Auth;
using Chat.Core.IService;
using Chat.Service;
using Chat.Api.Hubs;
using Chat.StockBot.IStockBot;
using Chat.StockBot.StockBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Api
{
    public class DependencyInjection
    {
        public void Map(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IStockBotService), typeof(StockBotService));
            services.AddScoped(typeof(IChatHub), typeof(ChatHub));
            services.AddScoped(typeof(IChatRoomService), typeof(ChatRoomService));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
        }
    }
}
