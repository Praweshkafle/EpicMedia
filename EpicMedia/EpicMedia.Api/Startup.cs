using EpicMedia.Api.Repository.Implementation;
using EpicMedia.Api.Repository.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver;

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


            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
          .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
          {
              options.Cookie.HttpOnly = false;
              options.SlidingExpiration = true;
              options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
              options.LoginPath = "/login";
              options.LogoutPath = "/logout";
              options.AccessDeniedPath = "/error";
          });
            services.AddAuthorization();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
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
policy.WithOrigins("https://localhost:44351", "http://localhost:44351")
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
