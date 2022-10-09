using AutoMapper;
using Blog.API.BaseContext;
using Blog.API.Helpers;
using Blog.API.Services.AuthService.Validator;
using Blog.API.Services.MailService;
using Blog.API.Services.RedisCacheService;
using Blog.Shared.Common;
using Blog.Shared.Entities;
using Blog.Shared.Enums;
using Blog.Shared.Models.RequestModels;
using Blog.Shared.Models.ResponseModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.API.Services.AuthService
{
    public class AuthService:IAuthService
    {
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        AuthValidator validAuth = new AuthValidator();
        RegisterValidator validRegister = new RegisterValidator();
        ChangePasswordValidator validChangePassword = new ChangePasswordValidator();

        public AuthService(ApplicationDbContext applicationDbContext, IOptions<AppSettings> appSettings,
            IMapper mapper,
            ILogger<AuthService> logger,
            IMailService mailService, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _logger = logger;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<AuthResponseModel>> Authenticate(AuthRequestModel req)
        {
            try
            {
                await Task.Delay(1);

                #region Validator
                ValidationResult validationResult = validAuth.Validate(req);
                if (validationResult.IsValid == false)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new AppException(error.ErrorMessage, 400);
                    }
                }
                #endregion

                #region Database operations
                var user = _applicationDbContext.Users.SingleOrDefault(s => s.Email == req.Email && s.Password == req.Password);
                if (user == null)
                    throw new AppException("Email and password do not match");
                //return new Response<AuthResponseModel>("Email and password do not match");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("kasjdlkajdlkajlkdjalkjdlkajsd");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("uId", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
                var mapUser = _mapper.Map<AuthResponseModel>(user);
                Constants.Constant.UserId = user.Id;
                #endregion

                #region RETURN
                return new Response<AuthResponseModel>(mapUser, "Login successful");
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
              
            }
            
        }

        public async Task<Response<string>> Register(RegisterRequestModel req)
        {
            try
            {
                #region Validator
                ValidationResult validationResult = validRegister.Validate(req);
                if (validationResult.IsValid == false)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new AppException(error.ErrorMessage, 400);
                    }
                }
                #endregion

                #region Database operations
                var userCheck = await _applicationDbContext.Users.FirstOrDefaultAsync(s=>s.Email==req.Email);
                if (userCheck!=null)
                    throw new AppException("This email is being used");

                var userAdd = await _applicationDbContext.AddAsync(new User
                {
                    FirstName=req.FirstName,
                    LastName=req.LastName,
                    Email=req.Email,
                    Gender=req.Gender,
                    Password=req.Password,
                    PhoneNumber = req.PhoneNumber,
                    Role=Role.User
                });
                await _applicationDbContext.SaveChangesAsync();
                #endregion

                #region RETURN
                return new Response<string>() { Data = "Add user successful", Succeeded = true ,Message= "Add user successful" };
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Response<string>> ForgotPassword(string emailAddress)
        {
            try
            {
                #region Database operations
                var emailCheck = await _applicationDbContext.Users.FirstOrDefaultAsync(s=>s.Email==emailAddress && !s.IsDeleted);
                if (emailCheck==null)
                   throw new AppException("Email not found");                               

                Random rnd = new Random();
                
                EmailRequest emailRequest = new EmailRequest()
                {
                    To = emailCheck.Email,
                    Subject = "Forgot Password",
                    Body= $"Hello, your new password to login to the system: {rnd} ",
                    From=""
                };

                await _mailService.SendEmail(emailRequest);
                #endregion

                #region RETURN
                return new Response<string>() { Data = "Your new password has been sent to your e-mail address.", Succeeded = true, Message = "Successfull" };
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);

            }
        }

        public async Task<Response<string>> ChangePassword(ChangePasswordRequestModel req)
        {
            try
            {
                #region Validator
                ValidationResult validationResult = validChangePassword.Validate(req);
                if (validationResult.IsValid == false)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new AppException(error.ErrorMessage, 400);
                    }
                }
                #endregion

                #region Database operations
                var emailCheck = await _applicationDbContext.Users.FirstOrDefaultAsync(s=>s.Email==req.Email && s.IsDeleted);
                if (emailCheck == null)
                    throw new AppException("Email not found");

                if (req.CurrentPassword!=emailCheck.Password)
                    throw new AppException("Your current password did not match");

                if (req.NewPassword != req.TryNewPassword)
                    throw new AppException("New password and its repetition did not match");

                emailCheck.Password = req.NewPassword;
                _applicationDbContext.Users.Update(emailCheck);
                await _applicationDbContext.SaveChangesAsync();

                //Mail service will be added                
                EmailRequest emailRequest = new EmailRequest()
                {
                    To = emailCheck.Email,
                    Subject = "Change Password",
                    Body = $"Your password has been updated",
                    From = ""
                };

                await _mailService.SendEmail(emailRequest);
                #endregion

                #region RETURN
                return new Response<string>() { Data = "Your password has been updated", Succeeded = true, Message = "Successfull" };
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        
    }
}
