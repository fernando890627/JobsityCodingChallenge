using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.ActionFilters;
using Chat.Core.DTO;
using Chat.Core.IService;
using Chat.Core.TokenModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace Chat.Api.Controllers
{
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }
        // GET: Auth
        [HttpPost("Login")]
        [ValidateModel]
        [AllowAnonymous]
        [SwaggerResponse(200, Type = typeof(JsonTokenDto))]
        public async Task<IActionResult> Login([FromBody] LoginDto credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.Login(credentials);
            return Ok(result);
        }

        [HttpGet("Logout")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(bool))]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogOut();
            return Ok(result);
        }
        [HttpPost("Register")]
        [ValidateModel]
        [AllowAnonymous]
        [SwaggerResponse(200, Type = typeof(NewUserResponseDto))]
        public async Task<IActionResult> Register([FromBody] NewUserRequestDto data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.Register(data);
            return Ok(result);
        }
    }
}