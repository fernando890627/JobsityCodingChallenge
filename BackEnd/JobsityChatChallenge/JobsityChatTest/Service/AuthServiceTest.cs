using Chat.Core.DTO;
using Chat.Core.Entity;
using Chat.Core.TokenModels;
using Chat.Service;
using JobsityChatTest.Utils;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobsityChatTest.Service
{
    public class AuthServiceTest
    {

        [Fact]
        public async Task Login()
        {
            var _userManagerService = TestConfig._userManageMock;
            _userManagerService.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(TestConfig.APPLICATION_USER);
            _userManagerService.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var _serviceToken = TestConfig._tokenServiceMock;
            _serviceToken.Setup(x => x.GenerateJwtToken(It.IsAny<string>(),It.IsAny<ApplicationUser>()))
                .Returns(TestConfig.JSON_TOKEN_DTO);

            var service = new AuthService(_userManagerService.Object,
                _serviceToken.Object,
                TestConfig._mapperMock.Object);

            var valueResult = service.Login(TestConfig.LOGIN_DTO);
            Assert.IsType<JsonTokenDto>(valueResult.Result);
            Assert.False(valueResult.Result.HasError);
        }

        [Fact]
        public async Task LoginFailed()
        {
            var _userManagerService = TestConfig._userManageMock;
            _userManagerService.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(TestConfig.APPLICATION_USER);
            _userManagerService.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var _serviceToken = TestConfig._tokenServiceMock;
            _serviceToken.Setup(x => x.GenerateJwtToken(It.IsAny<string>(), It.IsAny<ApplicationUser>()))
                .Returns(TestConfig.JSON_TOKEN_DTO);

            var service = new AuthService(_userManagerService.Object,
                _serviceToken.Object,
                TestConfig._mapperMock.Object);

            var valueResult = service.Login(TestConfig.LOGIN_DTO);
            Assert.True(valueResult.Result.HasError);
        }

        [Fact]
        public async Task Register()
        {
            var _userManagerService = TestConfig._userManageMock;
            _userManagerService.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<ApplicationUser>);
            _userManagerService.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(TestConfig.IDENTITY_RESULT_SUCCESS);

            var _mapper = TestConfig._mapperMock;
            _mapper.Setup(x => x.Map<ApplicationUser>(It.IsAny<NewUserRequestDto>()))
                .Returns(TestConfig.APPLICATION_USER);

            var service = new AuthService(_userManagerService.Object,
                TestConfig._tokenServiceMock.Object,
                _mapper.Object);

            var valueResult = service.Register(TestConfig.NEW_USER_REQUEST);
            Assert.IsType<NewUserResponseDto>(valueResult.Result);
            Assert.False(valueResult.Result.HasError);
        }

        [Fact]
        public async Task RegisterFailed()
        {
            var _userManagerService = TestConfig._userManageMock;
            _userManagerService.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(TestConfig.APPLICATION_USER);
            _userManagerService.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(TestConfig.IDENTITY_RESULT_SUCCESS);

            var _mapper = TestConfig._mapperMock;
            _mapper.Setup(x => x.Map<ApplicationUser>(It.IsAny<NewUserRequestDto>()))
                .Returns(TestConfig.APPLICATION_USER);

            var service = new AuthService(_userManagerService.Object,
                TestConfig._tokenServiceMock.Object,
                _mapper.Object);

            var valueResult = service.Register(TestConfig.NEW_USER_REQUEST);
            Assert.True(valueResult.Result.HasError);
        }
    }
}
