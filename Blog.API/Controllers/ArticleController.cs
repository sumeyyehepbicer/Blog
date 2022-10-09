using Blog.API.Controllers.Base;
using Blog.API.Filters;
using Blog.API.Services.ArticleService;
using Blog.API.Services.RedisCacheService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [ServiceFilter(typeof(ControlIpActionFilter))]
    [Route("api/article")]
    [ApiController]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IRedisCacheManagerService _redisCacheManagerService;
        public ArticleController(IArticleService articleService,
            IRedisCacheManagerService redisCacheManagerService)
        {
            _articleService = articleService;
            _redisCacheManagerService = redisCacheManagerService;
        }
        #region GET
        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("get-all-articles")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _articleService.GetAll());
        }

        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("get-article-by-id")]
        public async Task<IActionResult> GetUserById(int Id)
        {

            return Ok(await _articleService.GetArticleById(Id));
        }

        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("get-article-by-user-id")]
        public async Task<IActionResult> GetArticleByUserId(int userId)
        {

            return Ok(await _articleService.GetArticleByUserId(userId));
        }
        #endregion
    }
}
