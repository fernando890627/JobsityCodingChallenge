using Chat.Core.Entity;
using Chat.Core.TokenModels;

namespace Chat.Core.Auth
{
    public interface ITokenService
    {
        JsonTokenDto GenerateJwtToken(string email, ApplicationUser user);
    }
}