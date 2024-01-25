using EpicMedia.Api.Filemanager;
using EpicMedia.Api.Repository.Implementation;
using EpicMedia.Api.Repository.Interface;
using EpicMedia.Api.Services.Implementation;
using EpicMedia.Api.Services.Interface;
using EpicMedia.Api.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

namespace EpicMedia.Api
{
    public class Startup
    {
        public IConfiguration Configuration
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Key Auth", Version = "v1" });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "ApiKey must appear in header",
                    Type = SecuritySchemeType.Http,
                    Name = "XApiKey",
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
                c.AddSecurityRequirement(requirement);
            });
            services.Configure<TokenSettings>(Configuration.GetSection(nameof(TokenSettings)));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var tokenSettings= Configuration.GetSection(nameof(TokenSettings)).Get<TokenSettings>();
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = tokenSettings.Issuer,
                            ValidateAudience = true,
                            ValidAudience = tokenSettings.Audience,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.SecretKey)),
                            ClockSkew = TimeSpan.Zero,
                           
                        };
                    });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseCors(policy =>
policy.WithOrigins("https://localhost:7118", "http://localhost:7118").AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
