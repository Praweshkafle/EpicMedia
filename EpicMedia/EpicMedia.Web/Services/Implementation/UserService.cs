using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using EpicMedia.Web.ViewModels;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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

        public async Task<(JwtTokenResponse? jwtTokenResponse, string ErrorMessage)> LoginUser(LoginDto loginDto)
        {
            try
            {
                var jsonPayload = JsonSerializer.Serialize(loginDto);
                var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/user/login", requestContent);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
                    var message = "";
                    if (errors.Count > 0)
                    {
                        foreach (var item in errors)
                        {
                            foreach (var errorMessage in item.Value)
                            {
                                message = $"{message} | {errorMessage}";
                            }
                        }
                    }
                    return (null, message);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var token = await response.Content.ReadFromJsonAsync<JwtTokenResponse>();
                    return (token,"Successfully LoggedIn");
                }
                else
                {
                    return (null, "Error Logging IN");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
