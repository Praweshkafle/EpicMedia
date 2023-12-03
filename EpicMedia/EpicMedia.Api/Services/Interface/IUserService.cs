using EpicMedia.Models.Dto;

namespace EpicMedia.Api.Services.Interface
{
    public interface IUserService
    {
        Task<(bool IsLoginSuccess, JwtTokenDto TokenResponse)> LoginAsync(LoginDto loginDto);
    }
}
