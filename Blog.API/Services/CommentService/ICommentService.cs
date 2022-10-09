using Blog.Shared.Common;
using Blog.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.CommentService
{
    public interface ICommentService
    {
        Task<Response<List<Comment>>> GetAll();
        Task<Response<Comment>> GetCommentByUserId(int userId);
        Task<Response<Comment>> GetCommentByArticleId(int articleId);
    }
}
