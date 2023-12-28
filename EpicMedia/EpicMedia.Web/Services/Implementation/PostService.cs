using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using EpicMedia.Web.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
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
        public async Task<ApiModel> PostAsync(PostDto post, IBrowserFile file)
        {
            try
            {
                var _httpClient = _httpClientFac.CreateClient("EpicMediaApi");

               // var jsonPayload = JsonSerializer.Serialize(post);

                //var fileContent = new StreamContent(file.OpenReadStream());
                //fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                //var formData = new MultipartFormDataContent();
                //formData.Add(new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                //formData.Add(content: fileContent, name: "file");
                var jsonPayload = JsonSerializer.Serialize(post);
                var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(("api/post/create"), requestContent);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return new ApiModel { Success=false,Message="Unable To Post" };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
