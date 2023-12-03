using EpicMedia.Api.Entity;
using EpicMedia.Api.Repository.Interface;
using EpicMedia.Api.Services.Interface;
using EpicMedia.Api.Settings;
using EpicMedia.Models.Dto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EpicMedia.Api.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenSettings _tokenSettings;
        public UserService(IUserRepository userRepository,
            IOptions<TokenSettings> tokenSettings)
        {
            _userRepository = userRepository;
            _tokenSettings = tokenSettings.Value;
        }
        public async Task<(bool IsLoginSuccess,JwtTokenDto TokenResponse)> LoginAsync(LoginDto loginDto)
        {
            if(string.IsNullOrEmpty(loginDto.UserName) || string.IsNullOrEmpty(loginDto.Password))
            {
                return (false,null);
            }

            var user = await _userRepository.GetByUserName(loginDto.UserName);
            if(user == null) { return (false,null); }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return (false,null);
            }

            var jwtAccessToken = new JwtTokenDto
            {
                AccessToken = GenerateJwtToken(user)
            };
            return (true, jwtAccessToken);
        }


        private string GenerateJwtToken(User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));

            var credentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim("UserName", user.Username.ToString()));
            claims.Add(new Claim("ProfilePicture", user.ProfilePicture.ToString()));
            claims.Add(new Claim("Email", user.Email.ToString()));

            var securityToken = new JwtSecurityToken(
                    issuer:_tokenSettings.Issuer,
                    audience:_tokenSettings.Audience,
                    expires:DateTime.Now.AddMinutes(30),
                    signingCredentials:credentials,
                    claims:claims
                );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

    }
}
