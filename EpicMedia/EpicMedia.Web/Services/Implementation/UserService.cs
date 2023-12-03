using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace EpicMedia.Web.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> AddUser(UserDto userDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserDto>(("api/user/register"), userDto);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> LoginUser(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<LoginDto>(("api/user/login"), loginDto);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
