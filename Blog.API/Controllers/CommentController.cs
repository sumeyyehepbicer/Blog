using Blog.API.Controllers.Base;
using Blog.API.Filters;
using Blog.API.Services.CommentService;
using Blog.API.Services.RedisCacheService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [ServiceFilter(typeof(ControlIpActionFilter))]
    [Route("api/comment")]
    [ApiController]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly IRedisCacheManagerService _redisCacheManagerService;
        public CommentController(ICommentService commentService,
            IRedisCacheManagerService redisCacheManagerService)
        {
            _commentService = commentService;
            _redisCacheManagerService = redisCacheManagerService;
        }
    }
}
