using EpicMedia.Api.Repository.Implementation;
using EpicMedia.Api.Repository.Interface;
using EpicMedia.Api.Services.Implementation;
using EpicMedia.Api.Services.Interface;
using EpicMedia.Api.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
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
            services.AddScoped<IUserService, UserService>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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
                            ClockSkew = TimeSpan.Zero
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
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseCors(policy =>
policy.WithOrigins("https://localhost:7118", "http://localhost:7118")
.AllowAnyMethod()
.WithHeaders(HeaderNames.ContentType)
);
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
