using EpicMedia.Models.Dto;
using EpicMedia.Web.ViewModels;

namespace EpicMedia.Web.Services.Interface
{
    public interface IUserService
    {
        Task<bool> AddUser(UserDto userDtp);
        Task<(JwtTokenResponse? jwtTokenResponse, string ErrorMessage)> LoginUser(LoginDto loginDto);
    }
}
