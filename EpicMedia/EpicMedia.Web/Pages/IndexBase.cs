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
        public AuthenticationStateProvider authProvider { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        public bool CreatePostDialogOpen { get; set; }
        public bool IsAuthinticate { get; set; }
        public List<CommentDto> newComments { get; set; }=new List<CommentDto>();
        public string commentText { get; set; }
        public List<PostDto> Posts { get; set; }
        [Inject]
        public IPostService _postService { get; set; }
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

        protected override async Task OnInitializedAsync()
        {
            var authstate = await authProvider.GetAuthenticationStateAsync();
            IsAuthinticate = authstate.User.Identity.IsAuthenticated;
            Posts =await _postService.GetAllPost();
            newComments.Clear();
        }

        private Dictionary<string, bool> postLikes = new Dictionary<string, bool>();

        public string GetLikeIcon(string postId)
        {
            return postLikes.TryGetValue(postId, out var liked) && liked ? "fa fa-heart" : "fa fa-heart-o";
        }

        public void ToggleLike(string postId)
        {
            if (postLikes.ContainsKey(postId))
            {
                postLikes[postId] = !postLikes[postId];
            }
            else
            {
                postLikes.Add(postId, true);
            }
            var result = _postService.LikeDislikeAsync(postId);
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
                if (!IsAuthinticate)
                {
                    navigationManager.NavigateTo("/login");
                }
                string userId = await GetUserId();
                newComments.Add(new CommentDto
                {
                    User = userId,
                    CreatedAt = DateTime.Now,
                    Text = commentText,
                    postId=postId
                });
                var commentDto = new CommentDto
                {
                    Text = commentText,
                    CreatedAt = DateTime.Now,
                    User = userId
                };
                var response = await _postService.PostCommentAsync(commentDto,postId);
                if (response.Success)
                {
                    commentText = "";
                    StateHasChanged();
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
