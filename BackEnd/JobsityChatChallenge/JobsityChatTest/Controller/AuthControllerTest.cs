using Chat.Api.Controllers;
using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.TokenModels;
using Chat.Service;
using JobsityChatTest.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobsityChatTest.Controller
{
    public class AuthControllerTest
    {

        [Fact]
        public async Task Login()
        {
            var _authService = TestConfig._authServiceMock;
            _authService.Setup(x => x.Login(It.IsAny<LoginDto>())).ReturnsAsync(TestConfig.JSON_TOKEN_DTO);
            var controller = new AuthController(_authService.Object);
            var valueResult = await controller.Login(TestConfig.LOGIN_DTO);
            Assert.IsType<OkObjectResult>(valueResult);
        }

        [Fact]
        public async Task LoginFailed()
        {
            var _authService = TestConfig._authServiceMock;
            _authService.Setup(x => x.Login(It.IsAny<LoginDto>())).ReturnsAsync(TestConfig.JSON_TOKEN_DTO);
            var controller = new AuthController(_authService.Object);
            var valueResult = await controller.Login(null);
            Assert.IsNotType<OkObjectResult>(valueResult);
        }

        [Fact]
        public async Task Register()
        {
            var _authService = TestConfig._authServiceMock;
            _authService.Setup(x => x.Register(It.IsAny<NewUserRequestDto>())).ReturnsAsync(TestConfig.NEW_USER_RESPONSE);
            var controller = new AuthController(_authService.Object);
            var valueResult = await controller.Register(null);
            Assert.IsNotType<OkObjectResult>(valueResult);            
        }

        [Fact]
        public async Task RegisterFailed()
        {
            var _authService = TestConfig._authServiceMock;
            _authService.Setup(x => x.Register(It.IsAny<NewUserRequestDto>())).ReturnsAsync(TestConfig.NEW_USER_RESPONSE);
            var controller = new AuthController(_authService.Object);
            var valueResult = await controller.Register(null);
            Assert.IsNotType<OkObjectResult>(valueResult);
        }
    }
}
