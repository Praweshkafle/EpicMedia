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
        public string UserId { get; set; }
        public List<PostDto> Posts { get; set; }
        [Inject]
        public IPostService _postService { get; set; }
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

        protected override async Task OnInitializedAsync()
        {
            var authstate = await authProvider.GetAuthenticationStateAsync();
            IsAuthinticate = authstate.User.Identity.IsAuthenticated;
            Posts =await _postService.GetAllPost();
            UserId = await GetUserId();
            newComments.Clear();
        }


        private Dictionary<string, bool> postLikes = new Dictionary<string, bool>();


        public string selectedCommentId = null;

        // Variable to hold the reply text
        public string replyText = "";

        // Method to toggle the reply section
        public void ToggleReplySection(string commentId)
        {
            selectedCommentId = selectedCommentId == commentId ? null : commentId;
            replyText = ""; // Reset reply text when toggling
        }

        // Method to post a reply to a comment
        public async Task PostReply(string postId, string commentId)
        {
            // Call your service method to post the reply
            // Ensure to pass postId, commentId, and replyText to the service method

            // Reset selected comment after posting reply
            selectedCommentId = null;
        }


        public string GetLikeIcon(string postId)
        {
            var post = Posts.FirstOrDefault(p => p.Id == postId);
            if (post != null && post.Likes.Contains(UserId))
            {
                return "fa fa-heart";
            }
            return "fa fa-heart-o";
        }

        public async void ToggleLike(PostDto post)
        {
            if (post.Likes.Contains(UserId))
            {
                post.Likes.Remove(UserId);
            }
            else
            {
                post.Likes.Add(UserId);
            }
            var result =await _postService.LikeDislikeAsync(post.Id);
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
