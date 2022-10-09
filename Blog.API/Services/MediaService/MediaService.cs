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

namespace Blog.API.Services.MediaService
{
    public class MediaService: IMediaService
    {
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<MediaService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MediaService(ApplicationDbContext applicationDbContext,
            IMapper mapper,
            ILogger<MediaService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        #region GET
        public async Task<Response<List<FileMedia>>> GetAll()
        {
            try
            {
                var getAll = await _applicationDbContext
                .FileMedias
                .Where(s => !s.IsDeleted)
                .ToListAsync();
                return new Response<List<FileMedia>>(getAll, "Get All Media");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Response<FileMedia>> GetMediaByArticleId(int articleId)
        {
            try
            {
                var getByArticleId = await _applicationDbContext
                .FileMedias
                .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == articleId);

                return new Response<FileMedia>(getByArticleId, "Get By Article Id ");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Response<FileMedia>> GetMediaById(int Id)
        {
            try
            {
                var getMediaById = await _applicationDbContext
                .FileMedias
                .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Id);

                return new Response<FileMedia>(getMediaById, "Get By Media Id ");
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
