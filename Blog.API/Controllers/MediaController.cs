using Blog.API.Controllers.Base;
using Blog.API.Filters;
using Blog.API.Services.MediaService;
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
    [Route("api/media")]
    [ApiController]
    public class MediaController : BaseController
    {
        private readonly IMediaService _mediaService;
        private readonly IRedisCacheManagerService _redisCacheManagerService;
        public MediaController(IMediaService mediaService,
            IRedisCacheManagerService redisCacheManagerService)
        {
            _mediaService = mediaService;
            _redisCacheManagerService = redisCacheManagerService;
        }
        #region GET
        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("get-all-articles")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _mediaService.GetAll());
        }

        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("get-media-by--article-id")]
        public async Task<IActionResult> GetMediaByArticleId(int articleId)
        {

            return Ok(await _mediaService.GetMediaByArticleId(articleId));
        }

        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("get-media-by-id")]
        public async Task<IActionResult> GetMediaById(int Id)
        {

            return Ok(await _mediaService.GetMediaById(Id));
        }
        #endregion
    }
}
