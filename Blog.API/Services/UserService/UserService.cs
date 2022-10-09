using AutoMapper;
using Blog.API.BaseContext;
using Blog.API.Helpers;
using Blog.API.Services.AuthService;
using Blog.Shared.Common;
using Blog.Shared.Entities;
using Blog.Shared.Enums;
using Blog.Shared.Models.RequestModels;
using Blog.Shared.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.UserService
{
    public class UserService : IUserService
    {
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(ApplicationDbContext applicationDbContext,
            IMapper mapper,
            ILogger<UserService> logger, 
            IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        #region GET
        public async Task<Response<List<UserResponseModel>>> GetAllUser()
        {
            try
            {              
                var users = await _applicationDbContext
                .Users
                .Where(s => !s.IsDeleted)
                .ToListAsync();
                var mapUser = _mapper.Map<List<UserResponseModel>>(users);
                return new Response<List<UserResponseModel>>(mapUser, "Get All User");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Response<UserResponseModel>> GetUserById(int Id)
        {
            try
            {
                var user = await _applicationDbContext.Users.Where(s => s.Id == Id && !s.IsDeleted).FirstOrDefaultAsync();
                if (user==null)
                    throw new AppException("User not found");

                var mapUser = _mapper.Map<UserResponseModel>(user);
                return new Response<UserResponseModel>(mapUser, $"Get User By For{Id}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region CREATE
        public async Task<Response<UserResponseModel>> Create(CreateUserRequestModel req)
        {
            try
            {
                var userExist = await _applicationDbContext.Users.FirstOrDefaultAsync(s=>s.Email==req.Email && !s.IsDeleted);
                if (userExist != null)
                    throw new AppException("User available");

                await _applicationDbContext.Users.AddAsync(new User
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    Gender = req.Gender,
                    Password = req.Password,
                    PhoneNumber = req.PhoneNumber,
                    Role = Role.User
                });

                await _applicationDbContext.SaveChangesAsync();
                return new Response<UserResponseModel>() { Succeeded = true, Message = "Add user successful" };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }



        #endregion

        #region UPDATE
        public async Task<Response<UserResponseModel>> Update(UpdateUserRequestModel req)
        {
            try
            {
                var userExist = await _applicationDbContext.Users.FirstOrDefaultAsync(s => s.Id == req.Id && !s.IsDeleted);
                if (userExist == null)
                    throw new AppException("User not found");


                _applicationDbContext.Update(new User
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    Gender = req.Gender,
                    Password = req.Password,
                    PhoneNumber = req.PhoneNumber,
                    Role = Role.User
                });
                await _applicationDbContext.SaveChangesAsync();
                return new Response<UserResponseModel>() { Succeeded = true, Message = "Update user successful" };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region DELETE       
        public async Task<Response<bool>> Delete(int Id)
        {
            try
            {
                var user = await _applicationDbContext.Users.Where(s => s.Id == Id && !s.IsDeleted).FirstOrDefaultAsync();
                if (user == null)
                    throw new AppException("User not found");

                _applicationDbContext.Users.Remove(user);
                return new Response<bool>(true);
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
