using Chat.Core.DTO;
using Chat.Core.TokenModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.IService
{
    public interface IAuthService
    {
        Task<JsonTokenDto> Login(LoginDto creedentials);
        Task<NewUserResponseDto> Register(NewUserRequestDto data);
    }
}
