using Blazored.Modal.Services;
using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EpicMedia.Web.Pages
{
    public class IndexBase:ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider authProvider { get; set; }
        public bool CreatePostDialogOpen { get; set; }
        public string commentText { get; set; }
        public List<PostDto> Posts { get; set; }
        [Inject]
        public IPostService _postService { get; set; }
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

        protected override async Task OnInitializedAsync()
        {
            Posts =await _postService.GetAllPost();

        }
        async Task<string> GetUserId()
        {
            var authState = await authProvider
                 .GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                claims = user.Claims.ToList();
                var userid = claims.Where(a => a.Type == "Id").Select(b => b.Value).FirstOrDefault();
                return userid;
            }
            return "";
        }
        public async Task PostComment(string postId)
        {
            try
            {
                string userId = await GetUserId();
                var commentDto = new CommentDto
                {
                    Text = commentText,
                    CreatedAt = DateTime.Now,
                    User = userId
                };
                var response = await _postService.PostCommentAsync(commentDto,postId);
                if (response.Success)
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void OnCreatePostDialogClose(bool accepted)
        {
            CreatePostDialogOpen = false;
            StateHasChanged();
        }

        public void OpenCreatePostDialog()
        {
            CreatePostDialogOpen = true;
            StateHasChanged();
        }
    }
}
