using Blog.Shared.Common;
using Blog.Shared.Entities;
using Blog.Shared.Models.RequestModels;
using Blog.Shared.Models.ResponseModels;
using System.Threading.Tasks;

namespace Blog.API.Services.AuthService
{
    public interface IAuthService
    {
        Task<Response<AuthResponseModel>> Authenticate(AuthRequestModel req);
        Task<Response<string>> Register(RegisterRequestModel req);
        Task<Response<string>> ForgotPassword(string emailAddress);
        Task<Response<string>> ChangePassword(ChangePasswordRequestModel req);
    }
}
