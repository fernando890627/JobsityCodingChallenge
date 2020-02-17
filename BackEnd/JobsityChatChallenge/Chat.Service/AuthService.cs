using AutoMapper;
using Chat.Core.Auth;
using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.IService;
using Chat.Core.TokenModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<JsonTokenDto> Login(LoginDto creedentials)
        {
            var user = await _userManager.FindByNameAsync(creedentials.UserName);
            if (user == null)
            {
                return new JsonTokenDto
                {
                    HasError = true,
                    Message = "User not found"
                };
            }
            var correctLogin = await _userManager.CheckPasswordAsync(user, creedentials.Password);
            if (!correctLogin)
            {
                return new JsonTokenDto
                {
                    HasError = true,
                    Message = "Incorrect User Name or Password"
                };
            }
            var jsonWebToken = _tokenService.GenerateJwtToken(creedentials.UserName, user);
            return jsonWebToken;
        }

        public async Task<NewUserResponseDto> Register(NewUserRequestDto data)
        {
            var appUser = await _userManager.FindByEmailAsync(data.Email);
            if (appUser != null)
            {
                return new NewUserResponseDto
                {
                    HasError = true,
                    Message = "Email in use"
                };
            }
            appUser = await _userManager.FindByEmailAsync(data.Email);
            if (appUser != null)
            {
                return new NewUserResponseDto
                {
                    HasError = true,
                    Message = "User in use"
                };
            }            
            appUser = _mapper.Map<ApplicationUser>(data);
            appUser.UserName = $"{data.FirstName}.{data.LastName}";
            var result = await _userManager.CreateAsync(appUser, data.Password);
            if (result.Succeeded)
            {
                return new NewUserResponseDto
                {
                    HasError = false,
                    Message = "User created successfuly",
                    UserId = appUser.Id,
                    UserName = appUser.UserName
                };
            }
            else
            {
                return new NewUserResponseDto
                {
                    HasError = true,
                    Message = "An error ocurred"
                };
            }
        }
    }
}
