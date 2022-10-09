using Blog.API.BaseContext;
using Blog.API.Extensions.Middleware;
using Blog.API.Filters;
using Blog.API.Models;
using Blog.API.Services.ArticleService;
using Blog.API.Services.AuthService;
using Blog.API.Services.AuthService.Validator;
using Blog.API.Services.MailService;
using Blog.API.Services.MediaService;
using Blog.API.Services.MongoService;
using Blog.API.Services.RedisCacheService;
using Blog.API.Services.UserService;
using Blog.Shared.Common;
using Blog.Shared.Models.RequestModels;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Blog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("AppDbConnectionString")));
            services.AddControllers();

            
            var key = Encoding.ASCII.GetBytes("kasjdlkajdlkajlkdjalkjdlkajsd");
            services.AddAuthentication(x =>
            {

                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #region Dependency Services
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IMediaService, MediaService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IRedisCacheManagerService, RedisCacheManagerService>();
            services.AddSingleton<LogService>();
            services.AddHttpContextAccessor();
            #endregion

            services.Configure<WhiteList>(Configuration.GetSection("WhiteList"));
            services.Configure<MongoDatabaseConnection>(Configuration.GetSection(nameof(MongoDatabaseConnection)));
            services.AddTransient<ControlIpActionFilter>();
            services.AddHttpContextAccessor();

            #region VALIDATOR
            services.AddSingleton<IValidator<AuthRequestModel>, AuthValidator>();
            services.AddSingleton<IValidator<RegisterRequestModel>, RegisterValidator>();
            services.AddSingleton<IValidator<ChangePasswordRequestModel>, ChangePasswordValidator>();
            #endregion
            services.AddStackExchangeRedisCache(action =>
            {
                action.Configuration = "localhost:6379";
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog.API v1"));
            }
           
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
