using AutoMapper;
using Blog.API.BaseContext;
using Blog.Shared.Common;
using Blog.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.CommentService
{
    public class CommentService: ICommentService
    {
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommentService(ApplicationDbContext applicationDbContext, 
            IMapper mapper, 
            ILogger<CommentService> logger, 
            IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<List<Comment>>> GetAll()
        {
            try
            {
                var getAll = await _applicationDbContext
                .Comments
                .Include(s => s.Article)
                .Include(s => s.User)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
                
                return new Response<List<Comment>>(getAll, "Get All Comments");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Response<Comment>> GetCommentByArticleId(int articleId)
        {
            try
            {
                var getById = await _applicationDbContext
                .Comments
                .Include(s => s.User)
                .Include(s => s.Article)
                .FirstOrDefaultAsync(s => !s.IsDeleted && s.ArticleId == articleId);
               
                return new Response<Comment>(getById, "Get By Comment Article Id ");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Response<Comment>> GetCommentByUserId(int userId)
        {
            try
            {
                var getById = await _applicationDbContext
                .Comments
                .Include(s => s.User)
                .Include(s => s.Article)
                .FirstOrDefaultAsync(s => !s.IsDeleted && s.UserId == userId);

                return new Response<Comment>(getById, "Get By Comment UserId ");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
