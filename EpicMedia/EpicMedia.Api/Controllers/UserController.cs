using EpicMedia.Api.Entity;
using EpicMedia.Api.Repository.Interface;
using EpicMedia.Models.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EpicMedia.Api.Services.Interface;

namespace EpicMedia.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        public UserController(IUserRepository userRepository,
            IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserDto userDto)
        {
            try
            {
                if (userDto != null)
                {
                    var result = await IsDuplicate(userDto.Username, userDto.Email);
                    if (result != null)
                    {
                        throw new Exception("Please choose different username and email.");
                    }
                    var hashedpassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                    var user = new User
                    {
                        Email = userDto.Email,
                        Password = hashedpassword,
                        ProfilePicture = userDto.ProfilePicture,
                        Username = userDto.Username,
                    };
                    await _userRepository.Create(user);
                    return StatusCode(StatusCodes.Status200OK,"user created");
                }
                ModelState.AddModelError("Register Error", "Invalid Credentials");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login( LoginDto loginDto)
        {
            try
            {
                var result= await _userService.LoginAsync(loginDto);
                if (result.IsLoginSuccess)
                {
                    return Ok(result.TokenResponse);
                }
                ModelState.AddModelError("LoginError", "Invalid Credentials");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        private async Task<User> ValidateUser(string username, string password)
        {
            var user = await _userRepository.GetByUserName(username);
            if (user == null)
            {
                return null;
            }
            if (!(password == user.Password))
            {
                return null;
            }
            return user;
        }

        private async Task<User> IsDuplicate(string username, string email)
        {
            var user = await _userRepository.GetByUserName(username);
            if (user == null) return null;
            if (user?.Username == username || user?.Email == email)
            {
                return null;
            }
            return user;
        }
    }
}
