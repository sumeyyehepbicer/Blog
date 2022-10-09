using Blog.API.BaseContext;
using Blog.API.Services.UserService;
using Blog.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.API.Services.RedisCacheService
{
    public class RedisCacheManagerService : IRedisCacheManagerService
    {
        //.net core domain event nedir öğrenilecek ve daha iyi bir cache servise kurulacak.
        private readonly IDistributedCache _distributedCache;
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RedisCacheManagerService(IDistributedCache distributedCache,
            ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _distributedCache = distributedCache;
            _applicationDbContext = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUser()
        {
            var cacheKey = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("uId").Value);
            User user;
            string json;

            var userFromCache = await _distributedCache.GetAsync(int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("uId").Value).ToString());
            if (userFromCache != null)
            {
                json = Encoding.UTF8.GetString(userFromCache);
                user = JsonConvert.DeserializeObject<User>(json);
            }
            else
            {
                var uId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("uId").Value);
                user = await _applicationDbContext.Users.Where(s => s.Id == uId && !s.IsDeleted).FirstOrDefaultAsync();
                json = JsonConvert.SerializeObject(user);
                userFromCache= Encoding.UTF8.GetBytes(json);
                var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(1)) // belirli bir süre erişilmemiş ise expire eder
                        .SetAbsoluteExpiration(DateTime.Now.AddMonths(1)); // belirli bir süre sonra expire eder.
                await _distributedCache.SetAsync(cacheKey.ToString(), userFromCache, options);
            }
            return user;
        }


    }
}
