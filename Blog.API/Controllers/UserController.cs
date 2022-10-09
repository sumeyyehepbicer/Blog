using Blog.API.Controllers.Base;
using Blog.API.Filters;
using Blog.API.Services.RedisCacheService;
using Blog.API.Services.UserService;
using Blog.Shared.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [ServiceFilter(typeof(ControlIpActionFilter))]
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRedisCacheManagerService _redisCacheManagerService;
        public UserController(IUserService userService,
            IRedisCacheManagerService redisCacheManagerService)
        {
            _userService = userService;
            _redisCacheManagerService = redisCacheManagerService;
        }

        #region GET
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetAllUser()
        {
            var u = await _redisCacheManagerService.GetUser();
            if (u == null)
            {
                return Ok(await _userService.GetAllUser());
            }
            return Ok(u);
        }

        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserById(int Id)
        {

            return Ok(await _userService.GetUserById(Id));
        }
        #endregion

        #region CREATE

        [Authorize(Roles = "SuperAdmin,User")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUserRequestModel req)
        {
            return Ok(await _userService.Create(req));
        }
        #endregion

        #region UPDATE

        [Authorize(Roles = "SuperAdmin,User")]
        [HttpPost("update")]
        public async Task<IActionResult> Update(UpdateUserRequestModel req)
        {
            return Ok(await _userService.Update(req));
        }
        #endregion

        #region DELETE
        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int Id)
        {

            return Ok(await _userService.Delete(Id));

        }
        #endregion
    }
}
