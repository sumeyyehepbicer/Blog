using AutoMapper;
using Blog.API.BaseContext;
using Blog.Shared.Common;
using Blog.Shared.Entities;
using Blog.Shared.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ArticleService(ApplicationDbContext applicationDbContext,
            IMapper mapper, ILogger<ArticleService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        #region GET
        public async Task<Response<List<ArticleMediaResponseModel>>> GetAll()
        {

            try
            {
                var getAll = await _applicationDbContext
                .Articles
                .Include(s => s.User)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
                List<ArticleMediaResponseModel> responseModels = new List<ArticleMediaResponseModel>();
                ArticleMediaResponseModel responseModel = new ArticleMediaResponseModel();

                foreach (var article in getAll)
                {
                    var getMedia = await _applicationDbContext.FileMedias.FirstOrDefaultAsync(s => s.ArticleId == article.Id);

                    responseModel.Article = article;
                    responseModel.FileMedia = getMedia;
                    responseModels.Add(responseModel);
                }
                return new Response<List<ArticleMediaResponseModel>>(responseModels, "Get All Articles");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Response<ArticleMediaResponseModel>> GetArticleById(int Id)
        {
            try
            {
                var getById = await _applicationDbContext
                .Articles
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Id);
                ArticleMediaResponseModel responseModel = new ArticleMediaResponseModel();
                var getMedia = await _applicationDbContext.FileMedias.FirstOrDefaultAsync(s => s.ArticleId == getById.Id);
                responseModel.Article = getById;
                responseModel.FileMedia = getMedia;
                return new Response<ArticleMediaResponseModel>(responseModel, "Get By Article Id ");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Response<ArticleMediaResponseModel>> GetArticleByUserId(int userId)
        {
            try
            {
                var getByUserId = await _applicationDbContext
                .Articles
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == userId);
                ArticleMediaResponseModel responseModel = new ArticleMediaResponseModel();
                var getMedia = await _applicationDbContext.FileMedias.FirstOrDefaultAsync(s => s.ArticleId == getByUserId.Id);
                responseModel.Article = getByUserId;
                responseModel.FileMedia = getMedia;
                return new Response<ArticleMediaResponseModel>(responseModel, "Get By User Id ");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
