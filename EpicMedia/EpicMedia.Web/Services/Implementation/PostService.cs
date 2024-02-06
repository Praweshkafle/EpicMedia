using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using EpicMedia.Web.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
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
        }



        public async Task<List<PostDto>> GetAllPost()
        {
            try
            {
                var _httpClient = _httpClientFac.CreateClient("EpicMediaApi");

                var response =await _httpClient.GetAsync("api/posts/getallpost");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return new List<PostDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<List<PostDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiModel> LikeDislikeAsync(string postId)
        {
            try
            {
                var _httpClient = _httpClientFac.CreateClient("EpicMediaApi");
                var jsonPayload = JsonSerializer.Serialize(postId);
                var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/posts/like/",requestContent);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiModel { Success = true, Message = "Comment Successfully" };
                }
                else
                {
                    return new ApiModel { Success = false, Message = "Unable To comment" };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiModel> PostAsync(PostDto post, IBrowserFile file)
        {
            try
            {
                var _httpClient = _httpClientFac.CreateClient("EpicMediaApi");

                using (var fileStream = file.OpenReadStream(file.Size))
                {
                    var content = new MultipartFormDataContent();

                    var jsonPayload = JsonSerializer.Serialize(post);
                    content.Add(new StringContent(jsonPayload, Encoding.UTF8, "application/json"), "postdto");

                    content.Add(new StreamContent(fileStream), "file", file.Name);

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
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiModel> PostCommentAsync(CommentDto comment, string postId)
        {
            try
            {
                var _httpClient = _httpClientFac.CreateClient("EpicMediaApi");

                var jsonPayload = JsonSerializer.Serialize(comment);
                var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(("api/posts/comment/"+postId+"/"), requestContent);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiModel { Success = true, Message = "Comment Successfully" };
                }
                else
                {
                    return new ApiModel { Success = false, Message = "Unable To comment" };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
