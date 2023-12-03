using EpicMedia.Models.Dto;

namespace EpicMedia.Web.Services.Interface
{
    public interface IUserService
    {
        Task<bool> AddUser(UserDto userDtp);
        Task<bool> LoginUser(LoginDto loginDto);
    }
}
