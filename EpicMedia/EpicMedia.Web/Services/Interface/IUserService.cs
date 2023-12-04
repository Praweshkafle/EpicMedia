using EpicMedia.Models.Dto;

namespace EpicMedia.Web.Services.Interface
{
    public interface IUserService
    {
        Task<bool> AddUser(UserDto userDtp);
        Task<(bool isValid, string ErrorMessage)> LoginUser(LoginDto loginDto)
    }
}
