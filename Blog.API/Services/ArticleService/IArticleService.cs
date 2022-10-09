using Blog.Shared.Common;
using Blog.Shared.Entities;
using Blog.Shared.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.ArticleService
{
    public interface IArticleService
    {
        Task<Response<List<ArticleMediaResponseModel>>> GetAll();
        Task<Response<ArticleMediaResponseModel>> GetArticleByUserId(int userId);
        Task<Response<ArticleMediaResponseModel>> GetArticleById(int Id);
    }
}
