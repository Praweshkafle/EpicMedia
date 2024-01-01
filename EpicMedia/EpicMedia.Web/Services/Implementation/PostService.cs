using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using EpicMedia.Web.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace EpicMedia.Web.Services.Implementation
{
    public class PostService : IPostService
    {
        private readonly IHttpClientFactory _httpClientFac;
        public PostService(IHttpClientFactory httpClientFac)
        {
            _httpClientFac = httpClientFac;
            var _httpClient = _httpClientFac.CreateClient("EpicMediaApi");
        }
        public async Task<ApiModel> PostAsync(PostDto post, IBrowserFile file)
        {
            try
            {
                var _httpClient = _httpClientFac.CreateClient("EpicMediaApi");
                var newpost = new PostDto
                {
                    Comments = new List<CommentDto>(),
                    CreatedAt = DateTime.Now,
                    Content = post.Content,
                    Image = "",
                    Likes = new List<string>(),
                    User = "user",
                };
                var jsonPayload = JsonSerializer.Serialize(newpost);
                var content= new StringContent(jsonPayload, Encoding.UTF8,"application/json");
                var response = await _httpClient.PostAsync("api/posts/create", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiModel { Success = true, Message = "Post Created Successfully" };
                }
                else
                {
                    return new ApiModel { Success = false, Message = "Unable To Post" };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
