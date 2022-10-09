using Blog.API.Controllers.Base;
using Blog.API.Filters;
using Blog.API.Services.AuthService;
using Blog.API.Services.RedisCacheService;
using Blog.Shared.Models.RequestModels;
using Blog.Shared.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [ServiceFilter(typeof(ControlIpActionFilter))]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IRedisCacheManagerService _redisCacheManagerService;
        public AuthController(IAuthService authService, IRedisCacheManagerService redisCacheManagerService)
        {
            _authService = authService;
            _redisCacheManagerService = redisCacheManagerService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequestModel req)
        {
            return Ok(await _authService.Authenticate(req));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel req)
        {
            return Ok(await _authService.Register(req));
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string emailAddress)
        {            
            return Ok(await _authService.ForgotPassword(emailAddress));
        }

        [AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel req)
        {
            return Ok(await _authService.ChangePassword(req));
        }
    }
}
