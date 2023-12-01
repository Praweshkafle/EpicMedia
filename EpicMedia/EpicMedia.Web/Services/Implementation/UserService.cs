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
                var jsonResult = JsonConvert.SerializeObject(userDto);
                var content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(("api/user/register"), content);
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
