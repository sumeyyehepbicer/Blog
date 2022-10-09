using Blog.Shared.Common;
using Blog.Shared.Models.RequestModels;
using Blog.Shared.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.UserService
{
    public interface IUserService
    {
        #region GET
        Task<Response<List<UserResponseModel>>> GetAllUser();
        Task<Response<UserResponseModel>> GetUserById(int Id);
        #endregion
        #region CREATE
        Task<Response<UserResponseModel>> Create(CreateUserRequestModel req);

        #endregion
        #region UPDATE
        Task<Response<UserResponseModel>> Update(UpdateUserRequestModel req);
        #endregion
        #region DELETE 
        Task<Response<bool>> Delete(int Id);
        #endregion
    }
}
