using Blog.API.BaseContext;
using Blog.API.Services.MongoService;
using Blog.Shared.Common;
using Blog.Shared.MongoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Blog.API.Filters
{
    public class ControlIpActionFilter : ActionFilterAttribute
    {
        private readonly WhiteList _whiteList;
        IConfiguration _configuration;
        private readonly LogService _logService;
        private readonly ILogger<ControlIpActionFilter> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly ApplicationDbContext _applicationDbContext;

        public ControlIpActionFilter(IConfiguration configuration,
            IOptions<WhiteList> whiteList,
            ILogger<ControlIpActionFilter> logger, LogService logService,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext applicationDbContext)
        {
            _whiteList = whiteList.Value;
            _configuration = configuration;
            _logger = logger;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            //herhangi bir eylemin filtresinden önce çalışır.
            //nereye geldiğini yaz
            var foundIpAddres = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var whiteListFind = _whiteList.IpList;
            foreach (var whiteIp in whiteListFind)
            {
                if (whiteIp != foundIpAddres)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }
            }

            var uId = _httpContextAccessor.HttpContext?.User?.FindFirst("uId");
            if (uId==null)
            {
                var utcNow = DateTime.Now;
                LogModel lm = new LogModel()
                {
                    IpAddress = foundIpAddres,
                    FirstName= "Anonymous",
                    LastName= "Anonymous",
                    CreatedAt= new BsonDateTime(utcNow)
            };
                await _logService.CreateAsync(lm);
            }
            else
            {
                var findUId = await _applicationDbContext.Users.Where(s => s.Id == int.Parse(uId.Value) && !s.IsDeleted).FirstOrDefaultAsync(); 
                LogModel lm = new LogModel()
                {
                    IpAddress = foundIpAddres,
                    FirstName = findUId.FirstName,
                    LastName = findUId.LastName,
                    CreatedAt=findUId.CreatedAt
                };
                await _logService.CreateAsync(lm);
            }
            

           
            base.OnActionExecuting(context);
        }

    }
}
