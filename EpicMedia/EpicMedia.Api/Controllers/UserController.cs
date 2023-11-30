using EpicMedia.Api.Entity;
using EpicMedia.Api.Repository.Interface;
using EpicMedia.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EpicMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserDto userDto)
        {
            try
            {
                if (userDto != null)
                {
                    var user = new User
                    {
                        Email = userDto.Email,
                        Password = userDto.Password,
                        ProfilePicture = userDto.ProfilePicture,
                        Username = userDto.Username,
                    };
                    await _userRepository.Create(user);
                    return Ok();
                }
                throw new Exception("Something went wrong");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
