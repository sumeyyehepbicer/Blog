using Blog.Shared.Common;
using Blog.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.MediaService
{
    public interface IMediaService
    {
        Task<Response<List<FileMedia>>> GetAll();
        Task<Response<FileMedia>> GetMediaByArticleId(int articleId);
        Task<Response<FileMedia>> GetMediaById(int Id);
    }
}
