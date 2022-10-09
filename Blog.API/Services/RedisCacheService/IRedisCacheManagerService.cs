using Blog.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.RedisCacheService
{
    public interface IRedisCacheManagerService
    {
        Task<User> GetUser();
    }
}
